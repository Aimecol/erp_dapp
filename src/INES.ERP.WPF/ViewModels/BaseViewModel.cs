using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace INES.ERP.WPF.ViewModels;

/// <summary>
/// Base view model class that provides common functionality
/// </summary>
public abstract class BaseViewModel : ObservableObject
{
    private bool _isBusy;
    private string _title = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _hasError;

    /// <summary>
    /// Indicates if the view model is busy performing an operation
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                OnPropertyChanged(nameof(IsNotBusy));
                // Refresh commands that depend on IsBusy
                RefreshCommands();
            }
        }
    }

    /// <summary>
    /// Indicates if the view model is not busy
    /// </summary>
    public bool IsNotBusy => !IsBusy;

    /// <summary>
    /// Title of the view or operation
    /// </summary>
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    /// <summary>
    /// Error message to display to the user
    /// </summary>
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (SetProperty(ref _errorMessage, value))
            {
                HasError = !string.IsNullOrEmpty(value);
            }
        }
    }

    /// <summary>
    /// Indicates if there is an error
    /// </summary>
    public bool HasError
    {
        get => _hasError;
        private set => SetProperty(ref _hasError, value);
    }

    /// <summary>
    /// Clears the error message
    /// </summary>
    public void ClearError()
    {
        ErrorMessage = string.Empty;
    }

    /// <summary>
    /// Sets an error message
    /// </summary>
    /// <param name="message">Error message</param>
    public void SetError(string message)
    {
        ErrorMessage = message;
    }

    /// <summary>
    /// Executes an async operation with busy state management and error handling
    /// </summary>
    /// <param name="operation">Operation to execute</param>
    /// <param name="showErrorDialog">Whether to show error dialog on exception</param>
    /// <returns>True if successful, false if failed</returns>
    protected async Task<bool> ExecuteAsync(Func<Task> operation, bool showErrorDialog = true)
    {
        if (IsBusy) return false;

        try
        {
            IsBusy = true;
            ClearError();
            await operation();
            return true;
        }
        catch (Exception ex)
        {
            var errorMessage = GetUserFriendlyErrorMessage(ex);
            SetError(errorMessage);
            
            if (showErrorDialog)
            {
                await ShowErrorDialogAsync(errorMessage);
            }
            
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Executes an async operation with return value, busy state management and error handling
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="operation">Operation to execute</param>
    /// <param name="defaultValue">Default value to return on error</param>
    /// <param name="showErrorDialog">Whether to show error dialog on exception</param>
    /// <returns>Operation result or default value</returns>
    protected async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, T defaultValue = default!, bool showErrorDialog = true)
    {
        if (IsBusy) return defaultValue;

        try
        {
            IsBusy = true;
            ClearError();
            return await operation();
        }
        catch (Exception ex)
        {
            var errorMessage = GetUserFriendlyErrorMessage(ex);
            SetError(errorMessage);
            
            if (showErrorDialog)
            {
                await ShowErrorDialogAsync(errorMessage);
            }
            
            return defaultValue;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Gets a user-friendly error message from an exception
    /// </summary>
    /// <param name="exception">Exception</param>
    /// <returns>User-friendly error message</returns>
    protected virtual string GetUserFriendlyErrorMessage(Exception exception)
    {
        return exception switch
        {
            ArgumentException => "Invalid input provided. Please check your data and try again.",
            UnauthorizedAccessException => "You don't have permission to perform this action.",
            TimeoutException => "The operation timed out. Please try again.",
            InvalidOperationException => "This operation cannot be performed at this time.",
            _ => "An unexpected error occurred. Please try again or contact support if the problem persists."
        };
    }

    /// <summary>
    /// Shows an error dialog to the user
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Task</returns>
    protected virtual async Task ShowErrorDialogAsync(string message)
    {
        // This will be implemented with a proper dialog service
        await Task.Run(() =>
        {
            System.Windows.MessageBox.Show(message, "Error", 
                System.Windows.MessageBoxButton.OK, 
                System.Windows.MessageBoxImage.Error);
        });
    }

    /// <summary>
    /// Shows an information dialog to the user
    /// </summary>
    /// <param name="message">Information message</param>
    /// <returns>Task</returns>
    protected virtual async Task ShowInfoDialogAsync(string message)
    {
        await Task.Run(() =>
        {
            System.Windows.MessageBox.Show(message, "Information", 
                System.Windows.MessageBoxButton.OK, 
                System.Windows.MessageBoxImage.Information);
        });
    }

    /// <summary>
    /// Shows a confirmation dialog to the user
    /// </summary>
    /// <param name="message">Confirmation message</param>
    /// <param name="title">Dialog title</param>
    /// <returns>True if user confirmed, false otherwise</returns>
    protected virtual async Task<bool> ShowConfirmationDialogAsync(string message, string title = "Confirm")
    {
        return await Task.Run(() =>
        {
            var result = System.Windows.MessageBox.Show(message, title, 
                System.Windows.MessageBoxButton.YesNo, 
                System.Windows.MessageBoxImage.Question);
            return result == System.Windows.MessageBoxResult.Yes;
        });
    }

    /// <summary>
    /// Refreshes commands that depend on the current state
    /// Override this method to refresh specific commands
    /// </summary>
    protected virtual void RefreshCommands()
    {
        // Override in derived classes to refresh specific commands
    }

    /// <summary>
    /// Called when the view model is being initialized
    /// Override this method to perform initialization logic
    /// </summary>
    public virtual async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Called when the view model is being cleaned up
    /// Override this method to perform cleanup logic
    /// </summary>
    public virtual async Task CleanupAsync()
    {
        await Task.CompletedTask;
    }
}
