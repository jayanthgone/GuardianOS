namespace Guardian.Database.Models;

/// <summary>
/// Administrator user entity.
/// </summary>
public class User
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Administrator email address (unique).
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Full name of administrator.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Password hash (Argon2 or BCrypt).
    /// </summary>
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Is account active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Account creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last login timestamp.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Account last modified timestamp.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// User's registered devices.
    /// </summary>
    public ICollection<Device> Devices { get; set; } = new List<Device>();

    /// <summary>
    /// User's active sessions.
    /// </summary>
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
