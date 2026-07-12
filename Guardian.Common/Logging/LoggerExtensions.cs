using Microsoft.Extensions.Logging;

namespace Guardian.Common.Logging;

/// <summary>
/// Extension methods for structured logging.
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Log a security event.
    /// </summary>
    public static void LogSecurityEvent(
        this ILogger logger,
        string eventType,
        string description,
        Dictionary<string, object>? data = null)
    {
        var properties = new Dictionary<string, object>
        {
            { "EventType", eventType },
            { "Timestamp", DateTime.UtcNow },
            { "IsSecurityEvent", true }
        };

        if (data != null)
        {
            foreach (var item in data)
            {
                properties.Add(item.Key, item.Value);
            }
        }

        logger.LogWarning(
            "[SECURITY] {Description} - EventType: {EventType}",
            description,
            eventType);
    }

    /// <summary>
    /// Log an audit event.
    /// </summary>
    public static void LogAuditEvent(
        this ILogger logger,
        string action,
        string deviceId,
        string? user = null,
        Dictionary<string, object>? additionalData = null)
    {
        logger.LogInformation(
            "[AUDIT] Action: {Action} | Device: {DeviceId} | User: {User} | Timestamp: {Timestamp}",
            action,
            deviceId,
            user ?? "Unknown",
            DateTime.UtcNow);
    }
}
