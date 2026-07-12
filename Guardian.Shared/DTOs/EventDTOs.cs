namespace Guardian.Shared.DTOs;

/// <summary>
/// Event types for audit logging.
/// </summary>
public enum EventType
{
    /// <summary>User login event.</summary>
    UserLogin,
    
    /// <summary>User logout event.</summary>
    UserLogout,
    
    /// <summary>Application launch attempt.</summary>
    ApplicationLaunchAttempt,
    
    /// <summary>Application launch denied.</summary>
    ApplicationLaunchDenied,
    
    /// <summary>Application launch approved.</summary>
    ApplicationLaunchApproved,
    
    /// <summary>Application blocked.</summary>
    ApplicationBlocked,
    
    /// <summary>Application unblocked.</summary>
    ApplicationUnblocked,
    
    /// <summary>Password incorrect attempt.</summary>
    PasswordIncorrect,
    
    /// <summary>Protection enabled.</summary>
    ProtectionEnabled,
    
    /// <summary>Protection disabled.</summary>
    ProtectionDisabled,
    
    /// <summary>Remote command executed.</summary>
    RemoteCommandExecuted,
    
    /// <summary>Settings changed.</summary>
    SettingsChanged
}

/// <summary>
/// Audit event DTO.
/// </summary>
public class AuditEventDto
{
    /// <summary>
    /// Unique event identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Type of event.
    /// </summary>
    public EventType EventType { get; set; }

    /// <summary>
    /// Device where event occurred.
    /// </summary>
    public required string DeviceId { get; set; }

    /// <summary>
    /// Computer name.
    /// </summary>
    public string? ComputerName { get; set; }

    /// <summary>
    /// Current Windows user.
    /// </summary>
    public string? WindowsUser { get; set; }

    /// <summary>
    /// Event description.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Additional event data (JSON).
    /// </summary>
    public Dictionary<string, object>? AdditionalData { get; set; }

    /// <summary>
    /// When event occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Is this a critical/security event.
    /// </summary>
    public bool IsSecurityEvent { get; set; }
}
