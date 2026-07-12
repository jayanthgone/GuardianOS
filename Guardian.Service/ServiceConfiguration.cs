namespace Guardian.Service;

/// <summary>
/// Configuration for Windows Service installation.
/// Usage: sc create GuardianOS binPath=\"C:\path\to\Guardian.Service.exe\" start=auto
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// Service name in Windows Service Manager.
    /// </summary>
    public const string ServiceName = "GuardianOS";

    /// <summary>
    /// Service display name.
    /// </summary>
    public const string DisplayName = "GuardianOS Security Service";

    /// <summary>
    /// Service description.
    /// </summary>
    public const string Description = "Monitors and protects your Windows PC from unauthorized applications. Part of GuardianOS security system.";

    /// <summary>
    /// Start type: Auto starts with Windows.
    /// </summary>
    public const string StartType = "auto";
}
