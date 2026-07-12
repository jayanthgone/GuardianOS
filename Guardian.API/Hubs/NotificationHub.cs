using Microsoft.AspNetCore.SignalR;

namespace Guardian.API.Hubs;

/// <summary>
/// SignalR hub for real-time notifications to connected clients.
/// </summary>
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    /// <summary>
    /// Initialize notification hub.
    /// </summary>
    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Called when a client connects to the hub.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation($"Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a client disconnects from the hub.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Subscribe to device notifications.
    /// </summary>
    public async Task SubscribeToDevice(string deviceId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"device-{deviceId}");
        _logger.LogInformation($"Client {Context.ConnectionId} subscribed to device {deviceId}");
    }

    /// <summary>
    /// Unsubscribe from device notifications.
    /// </summary>
    public async Task UnsubscribeFromDevice(string deviceId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"device-{deviceId}");
        _logger.LogInformation($"Client {Context.ConnectionId} unsubscribed from device {deviceId}");
    }
}
