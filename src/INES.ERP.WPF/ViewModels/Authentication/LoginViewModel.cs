using CommunityToolkit.Mvvm.Input;
using INES.ERP.Core.Interfaces.Services;
using INES.ERP.WPF.Views;
using System.Windows;
using System.Windows.Input;

namespace INES.ERP.WPF.ViewModels.Authentication;

/// <summary>
/// View model for the login window
/// </summary>
public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private bool _rememberMe = false;
    private bool _showPassword = false;

    public LoginViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        Title = "INES-Ruhengeri ERP System - Login";
        
        // Initialize commands
        LoginCommand = new AsyncRelayCommand(LoginAsync, CanLogin);
        ForgotPasswordCommand = new RelayCommand(ForgotPassword);
        TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
        ExitCommand = new RelayCommand(Exit);
    }

    #region Properties

    /// <summary>
    /// Username or email for login
    /// </summary>
    public string Username
    {
        get => _username;
        set
        {
            if (SetProperty(ref _username, value))
            {
                ClearError();
                LoginCommand.NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Password for login
    /// </summary>
    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
            {
                ClearError();
                LoginCommand.NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Remember me option
    /// </summary>
    public bool RememberMe
    {
        get => _rememberMe;
        set => SetProperty(ref _rememberMe, value);
    }

    /// <summary>
    /// Show password option
    /// </summary>
    public bool ShowPassword
    {
        get => _showPassword;
        set => SetProperty(ref _showPassword, value);
    }

    #endregion

    #region Commands

    /// <summary>
    /// Command to perform login
    /// </summary>
    public IAsyncRelayCommand LoginCommand { get; }

    /// <summary>
    /// Command to handle forgot password
    /// </summary>
    public IRelayCommand ForgotPasswordCommand { get; }

    /// <summary>
    /// Command to toggle password visibility
    /// </summary>
    public IRelayCommand TogglePasswordVisibilityCommand { get; }

    /// <summary>
    /// Command to exit the application
    /// </summary>
    public IRelayCommand ExitCommand { get; }

    #endregion

    #region Command Implementations

    /// <summary>
    /// Checks if login can be executed
    /// </summary>
    /// <returns>True if login can be executed</returns>
    private bool CanLogin()
    {
        return !IsBusy && 
               !string.IsNullOrWhiteSpace(Username) && 
               !string.IsNullOrWhiteSpace(Password);
    }

    /// <summary>
    /// Performs the login operation
    /// </summary>
    /// <returns>Task</returns>
    private async Task LoginAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Get IP address and user agent (simplified for demo)
            var ipAddress = "127.0.0.1"; // In real app, get actual IP
            var userAgent = "INES ERP Desktop Client";

            // Attempt authentication
            var result = await _authenticationService.AuthenticateAsync(
                Username, 
                Password, 
                ipAddress, 
                userAgent);

            if (result.IsSuccess)
            {
                // Store session information (in real app, use secure storage)
                Properties.Settings.Default.SessionToken = result.SessionToken;
                Properties.Settings.Default.Username = RememberMe ? Username : string.Empty;
                Properties.Settings.Default.Save();

                // Navigate to main window
                await NavigateToMainWindowAsync();
            }
            else if (result.RequiresTwoFactor)
            {
                // Navigate to two-factor authentication
                await NavigateToTwoFactorAsync();
            }
            else
            {
                SetError(result.ErrorMessage ?? "Login failed. Please try again.");
            }
        });
    }

    /// <summary>
    /// Handles forgot password
    /// </summary>
    private void ForgotPassword()
    {
        // Navigate to forgot password window
        MessageBox.Show("Forgot password functionality will be implemented.", 
            "Information", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// Toggles password visibility
    /// </summary>
    private void TogglePasswordVisibility()
    {
        ShowPassword = !ShowPassword;
    }

    /// <summary>
    /// Exits the application
    /// </summary>
    private void Exit()
    {
        Application.Current.Shutdown();
    }

    #endregion

    #region Navigation Methods

    /// <summary>
    /// Navigates to the main window
    /// </summary>
    /// <returns>Task</returns>
    private async Task NavigateToMainWindowAsync()
    {
        await Task.Run(() =>
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Create and show main window
                var mainWindow = new MainWindow();
                mainWindow.Show();

                // Close login window
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType().Name == "LoginWindow")
                    {
                        window.Close();
                        break;
                    }
                }
            });
        });
    }

    /// <summary>
    /// Navigates to two-factor authentication
    /// </summary>
    /// <returns>Task</returns>
    private async Task NavigateToTwoFactorAsync()
    {
        await Task.Run(() =>
        {
            MessageBox.Show("Two-factor authentication will be implemented.", 
                "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        });
    }

    #endregion

    #region Overrides

    protected override void RefreshCommands()
    {
        LoginCommand.NotifyCanExecuteChanged();
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        
        // Load remembered username if available
        if (!string.IsNullOrEmpty(Properties.Settings.Default.Username))
        {
            Username = Properties.Settings.Default.Username;
            RememberMe = true;
        }
    }

    #endregion
}
