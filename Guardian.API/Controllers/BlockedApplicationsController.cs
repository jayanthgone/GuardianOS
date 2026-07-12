using Guardian.Database;
using Guardian.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guardian.API.Controllers;

/// <summary>
/// Blocked applications controller for managing blocked app rules.
/// </summary>
[ApiController]
[Route("api/devices/{deviceId}/blocked-apps")]
[Authorize]
public class BlockedApplicationsController : ControllerBase
{
    private readonly GuardianDbContext _dbContext;
    private readonly ILogger<BlockedApplicationsController> _logger;

    /// <summary>
    /// Initialize blocked applications controller.
    /// </summary>
    public BlockedApplicationsController(
        GuardianDbContext dbContext,
        ILogger<BlockedApplicationsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all blocked applications for a device.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlockedApplicationDto>>> GetBlockedApplications(Guid deviceId)
    {
        try
        {
            var blockedApps = await _dbContext.BlockedApplications
                .Where(b => b.DeviceId == deviceId)
                .AsNoTracking()
                .ToListAsync();

            var dtos = blockedApps.Select(a => new BlockedApplicationDto
            {
                Id = a.Id,
                ApplicationName = a.ApplicationName,
                ExecutablePath = a.ExecutablePath,
                RequiresPassword = a.RequiresPassword,
                BlockReason = a.BlockReason,
                CreatedAt = a.CreatedAt
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving blocked applications");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Add a blocked application.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BlockedApplicationDto>> AddBlockedApplication(
        Guid deviceId,
        [FromBody] AddBlockedApplicationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // TODO: Implement add blocked application logic
            return BadRequest("Add blocked application not yet implemented");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding blocked application");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Remove a blocked application.
    /// </summary>
    [HttpDelete("{appId}")]
    public async Task<IActionResult> RemoveBlockedApplication(Guid deviceId, Guid appId)
    {
        try
        {
            var blockedApp = await _dbContext.BlockedApplications
                .FirstOrDefaultAsync(a => a.Id == appId && a.DeviceId == deviceId);

            if (blockedApp == null)
                return NotFound();

            _dbContext.BlockedApplications.Remove(blockedApp);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing blocked application");
            return StatusCode(500, "Internal server error");
        }
    }
}
