using Guardian.Service;
using Microsoft.Extensions.Hosting.WindowsServices;
using Serilog;

var options = new ServiceCollection()
    .AddLogging()
    .BuildServiceProvider();

Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(
        path: "C:\\ProgramData\\GuardianOS\\Logs\\guardian-service-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = Host.CreateApplicationBuilder(args);

// Use Windows Service if running as service
if (WindowsServiceHelpers.IsWindowsService())
{
    builder.Services.AddWindowsService();
}

builder.Services.AddHostedService<ProcessMonitoringService>();
builder.Services.AddHostedService<CommandReceiverService>();

var host = builder.Build();
await host.RunAsync();
