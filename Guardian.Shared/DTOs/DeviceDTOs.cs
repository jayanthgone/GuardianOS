namespace Guardian.Shared.DTOs;

/// <summary>
/// Device registration request.
/// </summary>
public class DeviceRegistrationRequest
{
    /// <summary>
    /// Unique device identifier (MachineGuid from Windows Registry).
    /// </summary>
    public required string DeviceId { get; set; }

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
}

/// <summary>
/// Device data transfer object.
/// </summary>
public class DeviceDto
{
    /// <summary>
    /// Unique device identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Device unique hardware identifier.
    /// </summary>
    public required string DeviceId { get; set; }

    /// <summary>
    /// Computer name.
    /// </summary>
    public required string ComputerName { get; set; }

    /// <summary>
    /// Windows operating system version.
    /// </summary>
    public required string OSVersion { get; set; }

    /// <summary>
    /// Is device currently online.
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// Last known online timestamp.
    /// </summary>
    public DateTime? LastOnlineAt { get; set; }

    /// <summary>
    /// Device registration timestamp.
    /// </summary>
    public DateTime RegisteredAt { get; set; }

    /// <summary>
    /// Protection is enabled/disabled.
    /// </summary>
    public bool ProtectionEnabled { get; set; }
}

/// <summary>
/// Device system information.
/// </summary>
public class DeviceSystemInfoDto
{
    /// <summary>
    /// CPU usage percentage (0-100).
    /// </summary>
    public double CpuUsagePercent { get; set; }

    /// <summary>
    /// RAM usage in MB.
    /// </summary>
    public long RamUsageMb { get; set; }

    /// <summary>
    /// Total RAM in MB.
    /// </summary>
    public long TotalRamMb { get; set; }

    /// <summary>
    /// Available disk space in GB.
    /// </summary>
    public double AvailableDiskSpaceGb { get; set; }

    /// <summary>
    /// Total disk space in GB.
    /// </summary>
    public double TotalDiskSpaceGb { get; set; }

    /// <summary>
    /// Network upload speed in Mbps.
    /// </summary>
    public double NetworkUploadMbps { get; set; }

    /// <summary>
    /// Network download speed in Mbps.
    /// </summary>
    public double NetworkDownloadMbps { get; set; }

    /// <summary>
    /// Current logged-in username.
    /// </summary>
    public string? CurrentUser { get; set; }

    /// <summary>
    /// Timestamp of this information.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
