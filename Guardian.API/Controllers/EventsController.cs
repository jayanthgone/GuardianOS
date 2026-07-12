using Guardian.Database;
using Guardian.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guardian.API.Controllers;

/// <summary>
/// Events controller for audit log retrieval.
/// </summary>
[ApiController]
[Route("api/devices/{deviceId}/events")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly GuardianDbContext _dbContext;
    private readonly ILogger<EventsController> _logger;

    /// <summary>
    /// Initialize events controller.
    /// </summary>
    public EventsController(
        GuardianDbContext dbContext,
        ILogger<EventsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get audit events for a device.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditEventDto>>> GetEvents(
        Guid deviceId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? eventType = null)
    {
        try
        {
            const int maxPageSize = 100;
            if (pageSize > maxPageSize)
                pageSize = maxPageSize;

            var query = _dbContext.AuditEvents
                .Where(e => e.DeviceId == deviceId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(eventType))
                query = query.Where(e => e.EventType == eventType);

            var events = await query
                .OrderByDescending(e => e.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = events.Select(e => new AuditEventDto
            {
                Id = e.Id,
                EventType = Enum.Parse<EventType>(e.EventType),
                DeviceId = e.DeviceId.ToString(),
                WindowsUser = e.WindowsUser,
                Description = e.Description,
                Timestamp = e.Timestamp,
                IsSecurityEvent = e.IsSecurityEvent
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events");
            return StatusCode(500, "Internal server error");
        }
    }
}
