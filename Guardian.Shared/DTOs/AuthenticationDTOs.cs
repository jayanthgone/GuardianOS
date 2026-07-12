namespace Guardian.Shared.DTOs;

/// <summary>
/// Request DTO for user login with credentials.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Administrator email address.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Administrator password (will be hashed on backend).
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Device identifier for session tracking.
    /// </summary>
    public required string DeviceId { get; set; }
}

/// <summary>
/// Response DTO for successful authentication.
/// </summary>
public class AuthenticationResponse
{
    /// <summary>
    /// JWT access token for API requests.
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Refresh token for obtaining new access tokens.
    /// </summary>
    public required string RefreshToken { get; set; }

    /// <summary>
    /// Access token expiration in seconds.
    /// </summary>
    public int ExpiresIn { get; set; } = 3600;

    /// <summary>
    /// User details.
    /// </summary>
    public required UserDto User { get; set; }
}

/// <summary>
/// Request DTO for refreshing access token.
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Current refresh token.
    /// </summary>
    public required string RefreshToken { get; set; }
}

/// <summary>
/// User data transfer object.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Unique user identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Administrator email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Full name of the administrator.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Account creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
