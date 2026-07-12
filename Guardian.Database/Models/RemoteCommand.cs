namespace Guardian.Database.Models;

/// <summary>
/// Remote command entity for tracking sent commands.
/// </summary>
public class RemoteCommand
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Target device ID (foreign key).
    /// </summary>
    public required Guid DeviceId { get; set; }

    /// <summary>
    /// Command type.
    /// </summary>
    public required string CommandType { get; set; }

    /// <summary>
    /// Command execution status.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Command parameters (JSON).
    /// </summary>
    public string? ParametersJson { get; set; }

    /// <summary>
    /// Command result/response.
    /// </summary>
    public string? Result { get; set; }

    /// <summary>
    /// Command priority (0-10).
    /// </summary>
    public int Priority { get; set; } = 5;

    /// <summary>
    /// Command creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Command execution timestamp.
    /// </summary>
    public DateTime? ExecutedAt { get; set; }

    /// <summary>
    /// Command completion timestamp.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    /// <summary>
    /// Device that received this command.
    /// </summary>
    public Device? Device { get; set; }
}
