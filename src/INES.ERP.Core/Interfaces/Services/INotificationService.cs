namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Notification service interface for displaying user notifications
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Shows an information notification
    /// </summary>
    /// <param name="message">Notification message</param>
    /// <param name="title">Optional title</param>
    /// <param name="duration">Duration in milliseconds (0 for persistent)</param>
    void ShowInfo(string message, string? title = null, int duration = 5000);

    /// <summary>
    /// Shows a success notification
    /// </summary>
    /// <param name="message">Notification message</param>
    /// <param name="title">Optional title</param>
    /// <param name="duration">Duration in milliseconds (0 for persistent)</param>
    void ShowSuccess(string message, string? title = null, int duration = 5000);

    /// <summary>
    /// Shows a warning notification
    /// </summary>
    /// <param name="message">Notification message</param>
    /// <param name="title">Optional title</param>
    /// <param name="duration">Duration in milliseconds (0 for persistent)</param>
    void ShowWarning(string message, string? title = null, int duration = 8000);

    /// <summary>
    /// Shows an error notification
    /// </summary>
    /// <param name="message">Notification message</param>
    /// <param name="title">Optional title</param>
    /// <param name="duration">Duration in milliseconds (0 for persistent)</param>
    void ShowError(string message, string? title = null, int duration = 10000);

    /// <summary>
    /// Shows a confirmation dialog
    /// </summary>
    /// <param name="message">Confirmation message</param>
    /// <param name="title">Dialog title</param>
    /// <param name="confirmText">Confirm button text</param>
    /// <param name="cancelText">Cancel button text</param>
    /// <returns>True if confirmed, false if cancelled</returns>
    Task<bool> ShowConfirmationAsync(
        string message, 
        string title = "Confirm", 
        string confirmText = "Yes", 
        string cancelText = "No");

    /// <summary>
    /// Shows an input dialog
    /// </summary>
    /// <param name="message">Input prompt message</param>
    /// <param name="title">Dialog title</param>
    /// <param name="defaultValue">Default input value</param>
    /// <param name="placeholder">Input placeholder text</param>
    /// <returns>Input value or null if cancelled</returns>
    Task<string?> ShowInputAsync(
        string message, 
        string title = "Input", 
        string? defaultValue = null, 
        string? placeholder = null);

    /// <summary>
    /// Clears all notifications
    /// </summary>
    void ClearAll();

    /// <summary>
    /// Event raised when a notification is shown
    /// </summary>
    event EventHandler<NotificationEventArgs>? NotificationShown;
}

/// <summary>
/// Notification types
/// </summary>
public enum NotificationType
{
    Info,
    Success,
    Warning,
    Error
}

/// <summary>
/// Notification event arguments
/// </summary>
public class NotificationEventArgs : EventArgs
{
    /// <summary>
    /// Notification type
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Notification message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Notification title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Duration in milliseconds
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Timestamp when notification was created
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
