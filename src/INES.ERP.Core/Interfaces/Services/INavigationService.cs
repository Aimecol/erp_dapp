namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Navigation service interface for managing application navigation
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to a specific view
    /// </summary>
    /// <param name="viewName">Name of the view to navigate to</param>
    /// <param name="parameter">Optional parameter to pass to the view</param>
    /// <returns>True if navigation was successful</returns>
    Task<bool> NavigateToAsync(string viewName, object? parameter = null);

    /// <summary>
    /// Navigates to a specific view model type
    /// </summary>
    /// <typeparam name="TViewModel">Type of the view model</typeparam>
    /// <param name="parameter">Optional parameter to pass to the view model</param>
    /// <returns>True if navigation was successful</returns>
    Task<bool> NavigateToAsync<TViewModel>(object? parameter = null) where TViewModel : class;

    /// <summary>
    /// Navigates back to the previous view
    /// </summary>
    /// <returns>True if navigation was successful</returns>
    Task<bool> GoBackAsync();

    /// <summary>
    /// Checks if navigation back is possible
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// Gets the current view name
    /// </summary>
    string? CurrentView { get; }

    /// <summary>
    /// Clears the navigation history
    /// </summary>
    void ClearHistory();

    /// <summary>
    /// Event raised when navigation occurs
    /// </summary>
    event EventHandler<NavigationEventArgs>? Navigated;
}

/// <summary>
/// Navigation event arguments
/// </summary>
public class NavigationEventArgs : EventArgs
{
    /// <summary>
    /// Name of the view being navigated to
    /// </summary>
    public string ViewName { get; set; } = string.Empty;

    /// <summary>
    /// Parameter passed to the view
    /// </summary>
    public object? Parameter { get; set; }

    /// <summary>
    /// Previous view name
    /// </summary>
    public string? PreviousView { get; set; }
}
