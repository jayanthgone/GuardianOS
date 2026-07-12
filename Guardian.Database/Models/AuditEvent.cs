namespace Guardian.Database.Models;

/// <summary>
/// Audit event entity for logging all security-related actions.
/// </summary>
public class AuditEvent
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Device ID (foreign key).
    /// </summary>
    public required Guid DeviceId { get; set; }

    /// <summary>
    /// Event type (enum stored as string).
    /// </summary>
    public required string EventType { get; set; }

    /// <summary>
    /// Current Windows user when event occurred.
    /// </summary>
    public string? WindowsUser { get; set; }

    /// <summary>
    /// Event description.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Additional event data (JSON).
    /// </summary>
    public string? AdditionalDataJson { get; set; }

    /// <summary>
    /// Is this a critical/security event.
    /// </summary>
    public bool IsSecurityEvent { get; set; }

    /// <summary>
    /// Event timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// Device where event occurred.
    /// </summary>
    public Device? Device { get; set; }
}
