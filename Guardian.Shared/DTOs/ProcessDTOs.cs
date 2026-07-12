namespace Guardian.Shared.DTOs;

/// <summary>
/// Running process information.
/// </summary>
public class ProcessDto
{
    /// <summary>
    /// Process identifier (PID).
    /// </summary>
    public int ProcessId { get; set; }

    /// <summary>
    /// Process name/executable name.
    /// </summary>
    public required string ProcessName { get; set; }

    /// <summary>
    /// Full path to the executable.
    /// </summary>
    public string? ExecutablePath { get; set; }

    /// <summary>
    /// Memory usage in MB.
    /// </summary>
    public double MemoryUsageMb { get; set; }

    /// <summary>
    /// CPU usage percentage.
    /// </summary>
    public double CpuUsagePercent { get; set; }

    /// <summary>
    /// Process start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Process user (owner).
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Is process blocked by GuardianOS.
    /// </summary>
    public bool IsBlocked { get; set; }
}

/// <summary>
/// Blocked application DTO.
/// </summary>
public class BlockedApplicationDto
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Application executable name (e.g., "TikTok.exe").
    /// </summary>
    public required string ApplicationName { get; set; }

    /// <summary>
    /// Full path to executable (optional, for exact matching).
    /// </summary>
    public string? ExecutablePath { get; set; }

    /// <summary>
    /// Is password protection enabled for this app.
    /// </summary>
    public bool RequiresPassword { get; set; }

    /// <summary>
    /// Reason for blocking.
    /// </summary>
    public string? BlockReason { get; set; }

    /// <summary>
    /// When the block was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Add blocked application request.
/// </summary>
public class AddBlockedApplicationRequest
{
    /// <summary>
    /// Application executable name.
    /// </summary>
    public required string ApplicationName { get; set; }

    /// <summary>
    /// Full executable path.
    /// </summary>
    public string? ExecutablePath { get; set; }

    /// <summary>
    /// Password protection enabled.
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
}
