using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Guardian.Shared.DTOs;

namespace Guardian.Desktop.ViewModels;

/// <summary>
/// View model for application management.
/// </summary>
public partial class ApplicationManagerViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<BlockedApplicationDto> blockedApplications = new();

    [ObservableProperty]
    private string applicationNameToBlock = "";

    [ObservableProperty]
    private string blockReason = "";

    [ObservableProperty]
    private bool requiresPassword;

    [ObservableProperty]
    private bool isLoading;

    /// <summary>
    /// Load blocked applications.
    /// </summary>
    [RelayCommand]
    public async Task LoadBlockedApplications()
    {
        try
        {
            IsLoading = true;
            
            // TODO: Fetch blocked applications from API
            // blockedApplications = await _apiService.GetBlockedApplications();
            
            IsLoading = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading applications: {ex.Message}");
            IsLoading = false;
        }
    }

    /// <summary>
    /// Add blocked application.
    /// </summary>
    [RelayCommand]
    public async Task AddBlockedApplication()
    {
        if (string.IsNullOrWhiteSpace(ApplicationNameToBlock))
            return;

        try
        {
            var request = new AddBlockedApplicationRequest
            {
                ApplicationName = ApplicationNameToBlock,
                RequiresPassword = RequiresPassword,
                BlockReason = BlockReason
            };

            // TODO: Send to API
            // var result = await _apiService.AddBlockedApplication(request);
            
            ApplicationNameToBlock = string.Empty;
            BlockReason = string.Empty;
            RequiresPassword = false;
            
            await LoadBlockedApplications();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error adding application: {ex.Message}");
        }
    }

    /// <summary>
    /// Remove blocked application.
    /// </summary>
    [RelayCommand]
    public async Task RemoveBlockedApplication(BlockedApplicationDto app)
    {
        try
        {
            // TODO: Call API to remove application
            // await _apiService.RemoveBlockedApplication(app.Id);
            
            BlockedApplications.Remove(app);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error removing application: {ex.Message}");
        }
    }
}
