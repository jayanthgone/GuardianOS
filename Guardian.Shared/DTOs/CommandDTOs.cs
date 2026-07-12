namespace Guardian.Shared.DTOs;

/// <summary>
/// Remote command types.
/// </summary>
public enum RemoteCommandType
{
    /// <summary>Lock the computer.</summary>
    LockComputer,
    
    /// <summary>Restart the computer.</summary>
    Restart,
    
    /// <summary>Shutdown the computer.</summary>
    Shutdown,
    
    /// <summary>Log off current user.</summary>
    LogOff,
    
    /// <summary>Put computer to sleep.</summary>
    Sleep,
    
    /// <summary>Hibernate the computer.</summary>
    Hibernate,
    
    /// <summary>Enable protection on device.</summary>
    EnableProtection,
    
    /// <summary>Disable protection on device.</summary>
    DisableProtection,
    
    /// <summary>Block an application.</summary>
    BlockApplication,
    
    /// <summary>Unblock an application.</summary>
    UnblockApplication,
    
    /// <summary>Update security policy.</summary>
    UpdatePolicy,
    
    /// <summary>Approve pending application.</summary>
    ApprovePendingApplication,
    
    /// <summary>Deny pending application.</summary>
    DenyPendingApplication,
    
    /// <summary>Kill process.</summary>
    KillProcess,
    
    /// <summary>Update Guardian client.</summary>
    UpdateGuardian
}

/// <summary>
/// Remote command execution request.
/// </summary>
public class RemoteCommandRequest
{
    /// <summary>
    /// Type of command to execute.
    /// </summary>
    public RemoteCommandType CommandType { get; set; }

    /// <summary>
    /// Target device identifier.
    /// </summary>
    public required string TargetDeviceId { get; set; }

    /// <summary>
    /// Command-specific data (JSON).
    /// </summary>
    public Dictionary<string, object>? Parameters { get; set; }

    /// <summary>
    /// Priority level (0-10, higher = more urgent).
    /// </summary>
    public int Priority { get; set; } = 5;
}

/// <summary>
/// Remote command response.
/// </summary>
public class RemoteCommandResponse
{
    /// <summary>
    /// Unique command execution ID.
    /// </summary>
    public Guid CommandId { get; set; }

    /// <summary>
    /// Command status.
    /// </summary>
    public RemoteCommandStatus Status { get; set; }

    /// <summary>
    /// Command result message.
    /// </summary>
    public string? Result { get; set; }

    /// <summary>
    /// Execution timestamp.
    /// </summary>
    public DateTime ExecutedAt { get; set; }
}

/// <summary>
/// Remote command execution status.
/// </summary>
public enum RemoteCommandStatus
{
    /// <summary>Pending execution.</summary>
    Pending,
    
    /// <summary>Currently executing.</summary>
    Executing,
    
    /// <summary>Successfully completed.</summary>
    Success,
    
    /// <summary>Execution failed.</summary>
    Failed,
    
    /// <summary>Command was cancelled.</summary>
    Cancelled,
    
    /// <summary>Command timeout.</summary>
    Timeout
}
