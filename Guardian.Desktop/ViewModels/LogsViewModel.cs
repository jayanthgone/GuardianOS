using CommunityToolkit.Mvvm.ComponentModel;
using Guardian.Shared.DTOs;

namespace Guardian.Desktop.ViewModels;

/// <summary>
/// View model for activity logs.
/// </summary>
public partial class LogsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<AuditEventDto> auditLogs = new();

    [ObservableProperty]
    private string searchQuery = "";

    [ObservableProperty]
    private string selectedEventType = "All";

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private int totalLogsCount;

    /// <summary>
    /// Load audit logs.
    /// </summary>
    public async Task LoadLogs()
    {
        try
        {
            IsLoading = true;
            
            // TODO: Fetch logs from API
            // var logs = await _apiService.GetAuditLogs(selectedEventType, searchQuery);
            // AuditLogs = new ObservableCollection<AuditEventDto>(logs);
            
            TotalLogsCount = AuditLogs.Count;
            IsLoading = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading logs: {ex.Message}");
            IsLoading = false;
        }
    }

    /// <summary>
    /// Export logs to CSV.
    /// </summary>
    public async Task ExportLogsToCSV(string filePath)
    {
        try
        {
            // TODO: Implement CSV export
            // Export AuditLogs to CSV file
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error exporting logs: {ex.Message}");
        }
    }
}
