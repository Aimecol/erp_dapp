using System.Windows;

namespace INES.ERP.WPF.Services;

/// <summary>
/// Notification service for WPF application
/// </summary>
public class NotificationService
{
    /// <summary>
    /// Shows a success notification
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="title">Title of the notification</param>
    /// <param name="duration">Duration in milliseconds</param>
    public void ShowSuccess(string message, string title = "Success", int duration = 3000)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        });
    }

    /// <summary>
    /// Shows an error notification
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="title">Title of the notification</param>
    /// <param name="duration">Duration in milliseconds</param>
    public void ShowError(string message, string title = "Error", int duration = 5000)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        });
    }

    /// <summary>
    /// Shows a warning notification
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="title">Title of the notification</param>
    /// <param name="duration">Duration in milliseconds</param>
    public void ShowWarning(string message, string title = "Warning", int duration = 4000)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        });
    }

    /// <summary>
    /// Shows an info notification
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="title">Title of the notification</param>
    /// <param name="duration">Duration in milliseconds</param>
    public void ShowInfo(string message, string title = "Information", int duration = 3000)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        });
    }

    /// <summary>
    /// Shows a confirmation dialog
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="title">Title of the dialog</param>
    /// <returns>True if user confirmed, false otherwise</returns>
    public bool ShowConfirmation(string message, string title = "Confirm")
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }

    /// <summary>
    /// Shows a question dialog with three options
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="title">Title of the dialog</param>
    /// <returns>MessageBoxResult indicating user choice</returns>
    public MessageBoxResult ShowQuestion(string message, string title = "Question")
    {
        return MessageBox.Show(message, title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
    }
}
