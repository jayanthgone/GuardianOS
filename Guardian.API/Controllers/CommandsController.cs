using Guardian.Database;
using Guardian.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guardian.API.Controllers;

/// <summary>
/// Remote commands controller for sending commands to devices.
/// </summary>
[ApiController]
[Route("api/commands")]
[Authorize]
public class CommandsController : ControllerBase
{
    private readonly GuardianDbContext _dbContext;
    private readonly ILogger<CommandsController> _logger;

    /// <summary>
    /// Initialize commands controller.
    /// </summary>
    public CommandsController(
        GuardianDbContext dbContext,
        ILogger<CommandsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Send a remote command to a device.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RemoteCommandResponse>> SendCommand([FromBody] RemoteCommandRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // TODO: Implement send command logic
            return BadRequest("Send command not yet implemented");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending command");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get command status by ID.
    /// </summary>
    [HttpGet("{commandId}")]
    public async Task<ActionResult<RemoteCommandResponse>> GetCommandStatus(Guid commandId)
    {
        try
        {
            var command = await _dbContext.RemoteCommands
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == commandId);

            if (command == null)
                return NotFound();

            var response = new RemoteCommandResponse
            {
                CommandId = command.Id,
                Status = Enum.Parse<RemoteCommandStatus>(command.Status),
                Result = command.Result,
                ExecutedAt = command.ExecutedAt ?? DateTime.UtcNow
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving command status");
            return StatusCode(500, "Internal server error");
        }
    }
}
