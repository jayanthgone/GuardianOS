namespace Guardian.Database.Models;

/// <summary>
/// Protected device entity.
/// </summary>
public class Device
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Unique hardware device identifier (MachineGuid from Registry).
    /// </summary>
    public required string DeviceId { get; set; }

    /// <summary>
    /// Administrator user ID (foreign key).
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// Computer/device name.
    /// </summary>
    public required string ComputerName { get; set; }

    /// <summary>
    /// Windows operating system version.
    /// </summary>
    public required string OSVersion { get; set; }

    /// <summary>
    /// Total system RAM in GB.
    /// </summary>
    public int TotalMemoryGb { get; set; }

    /// <summary>
    /// Processor name/model.
    /// </summary>
    public string? ProcessorName { get; set; }

    /// <summary>
    /// Is device currently online.
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// Last known online timestamp.
    /// </summary>
    public DateTime? LastOnlineAt { get; set; }

    /// <summary>
    /// Is protection enabled on device.
    /// </summary>
    public bool ProtectionEnabled { get; set; } = true;

    /// <summary>
    /// Device registration timestamp.
    /// </summary>
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Device last modified timestamp.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// User who owns this device.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Blocked applications on this device.
    /// </summary>
    public ICollection<BlockedApplication> BlockedApplications { get; set; } = new List<BlockedApplication>();

    /// <summary>
    /// Audit events on this device.
    /// </summary>
    public ICollection<AuditEvent> AuditEvents { get; set; } = new List<AuditEvent>();

    /// <summary>
    /// Remote commands sent to this device.
    /// </summary>
    public ICollection<RemoteCommand> RemoteCommands { get; set; } = new List<RemoteCommand>();
}
