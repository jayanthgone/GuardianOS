namespace Guardian.Database.Models;

/// <summary>
/// Blocked application entity.
/// </summary>
public class BlockedApplication
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
    /// Application executable name (e.g., "TikTok.exe").
    /// </summary>
    public required string ApplicationName { get; set; }

    /// <summary>
    /// Full path to executable (optional, for exact matching).
    /// </summary>
    public string? ExecutablePath { get; set; }

    /// <summary>
    /// Is password protection enabled for this application.
    /// </summary>
    public bool RequiresPassword { get; set; }

    /// <summary>
    /// Password hash (if RequiresPassword is true).
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// Reason for blocking.
    /// </summary>
    public string? BlockReason { get; set; }

    /// <summary>
    /// Block creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Block last modified timestamp.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// Device that has this blocked application.
    /// </summary>
    public Device? Device { get; set; }
}
