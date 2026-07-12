using Microsoft.Extensions.Hosting;

namespace Guardian.Service;

/// <summary>
/// Windows Service that monitors running processes and enforces application blocking.
/// Runs with system privileges and can terminate unauthorized applications.
/// </summary>
public class ProcessMonitoringService : BackgroundService
{
    private readonly ILogger<ProcessMonitoringService> _logger;
    private Timer? _timer;
    private readonly HashSet<int> _trackedProcesses = new();

    /// <summary>
    /// Initialize process monitoring service.
    /// </summary>
    public ProcessMonitoringService(ILogger<ProcessMonitoringService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Execute the service on start.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("GuardianOS Process Monitoring Service started at {time}", DateTimeOffset.Now);

        // Initialize monitoring every 2 seconds
        _timer = new Timer(async _ => await MonitorProcesses(), null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    /// <summary>
    /// Monitor running processes and block unauthorized applications.
    /// </summary>
    private async Task MonitorProcesses()
    {
        try
        {
            // TODO: Implement process monitoring logic
            // 1. Get list of running processes using Process.GetProcesses()
            // 2. Compare against blocked applications list (from database or config)
            // 3. For blocked apps:
            //    - Log the attempt
            //    - Terminate the process
            //    - Send notification to backend API
            //    - Store encrypted offline policy
            // 4. Track newly launched processes
            // 5. Handle password-protected apps
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error monitoring processes");
        }
    }

    /// <summary>
    /// Stop the service gracefully.
    /// </summary>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("GuardianOS Process Monitoring Service stopped at {time}", DateTimeOffset.Now);
        _timer?.Dispose();
        await base.StopAsync(cancellationToken);
    }
}
