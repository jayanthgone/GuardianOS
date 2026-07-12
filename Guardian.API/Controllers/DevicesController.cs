using Guardian.Database;
using Guardian.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guardian.API.Controllers;

/// <summary>
/// Devices controller for device management and status.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DevicesController : ControllerBase
{
    private readonly GuardianDbContext _dbContext;
    private readonly ILogger<DevicesController> _logger;

    /// <summary>
    /// Initialize devices controller.
    /// </summary>
    public DevicesController(
        GuardianDbContext dbContext,
        ILogger<DevicesController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all devices for the current user.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
    {
        try
        {
            var devices = await _dbContext.Devices
                .AsNoTracking()
                .ToListAsync();

            var deviceDtos = devices.Select(d => new DeviceDto
            {
                Id = d.Id,
                DeviceId = d.DeviceId,
                ComputerName = d.ComputerName,
                OSVersion = d.OSVersion,
                IsOnline = d.IsOnline,
                LastOnlineAt = d.LastOnlineAt,
                RegisteredAt = d.RegisteredAt,
                ProtectionEnabled = d.ProtectionEnabled
            }).ToList();

            return Ok(deviceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving devices");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get device by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDto>> GetDevice(Guid id)
    {
        try
        {
            var device = await _dbContext.Devices
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (device == null)
                return NotFound();

            var deviceDto = new DeviceDto
            {
                Id = device.Id,
                DeviceId = device.DeviceId,
                ComputerName = device.ComputerName,
                OSVersion = device.OSVersion,
                IsOnline = device.IsOnline,
                LastOnlineAt = device.LastOnlineAt,
                RegisteredAt = device.RegisteredAt,
                ProtectionEnabled = device.ProtectionEnabled
            };

            return Ok(deviceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Register a new device.
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<DeviceDto>> RegisterDevice([FromBody] DeviceRegistrationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // TODO: Implement device registration logic
            return BadRequest("Device registration not yet implemented");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering device");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Update device online status.
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateDeviceStatus(Guid id, [FromBody] DeviceSystemInfoDto systemInfo)
    {
        try
        {
            var device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == id);
            if (device == null)
                return NotFound();

            device.IsOnline = true;
            device.LastOnlineAt = DateTime.UtcNow;
            device.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating device status");
            return StatusCode(500, "Internal server error");
        }
    }
}
