namespace Guardian.Database.Models;

/// <summary>
/// User session entity for tracking authentication sessions.
/// </summary>
public class Session
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// User ID (foreign key).
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// Device where login occurred.
    /// </summary>
    public string? DeviceId { get; set; }

    /// <summary>
    /// Device name/identifier.
    /// </summary>
    public string? DeviceName { get; set; }

    /// <summary>
    /// Refresh token hash.
    /// </summary>
    public required string RefreshTokenHash { get; set; }

    /// <summary>
    /// Session creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Session expiration timestamp.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Session last activity timestamp.
    /// </summary>
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Is session still active/valid.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    /// <summary>
    /// User who owns this session.
    /// </summary>
    public User? User { get; set; }
}
