using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Guardian.Desktop.ViewModels;

/// <summary>
/// View model for application settings.
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool autoStartEnabled;

    [ObservableProperty]
    private bool notificationsEnabled = true;

    [ObservableProperty]
    private bool offlineModeEnabled;

    [ObservableProperty]
    private string currentPassword = "";

    [ObservableProperty]
    private string newPassword = "";

    [ObservableProperty]
    private string confirmPassword = "";

    [ObservableProperty]
    private bool biometricSupported;

    [ObservableProperty]
    private bool biometricEnabled;

    /// <summary>
    /// Save settings.
    /// </summary>
    [RelayCommand]
    public async Task SaveSettings()
    {
        try
        {
            // TODO: Implement settings save logic
            // 1. Validate settings
            // 2. Update local configuration
            // 3. Send to API
            // 4. Show confirmation
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
        }
    }

    /// <summary>
    /// Change password.
    /// </summary>
    [RelayCommand]
    public async Task ChangePassword()
    {
        if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword != ConfirmPassword)
        {
            System.Diagnostics.Debug.WriteLine("Passwords do not match");
            return;
        }

        try
        {
            // TODO: Implement password change
            // 1. Verify current password
            // 2. Send new password to API
            // 3. Clear password fields
            
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error changing password: {ex.Message}");
        }
    }
}
