using System.Windows;
using System.Windows.Controls;

namespace INES.ERP.WPF.Services;

/// <summary>
/// Navigation service for WPF application
/// </summary>
public class NavigationService
{
    private readonly Dictionary<string, Type> _pages = new();
    private Frame? _mainFrame;
    private string? _currentView;

    /// <summary>
    /// Event raised when navigation occurs
    /// </summary>
    public event EventHandler<NavigationEventArgs>? Navigated;

    /// <summary>
    /// Checks if navigation back is possible
    /// </summary>
    public bool CanGoBack => _mainFrame?.CanGoBack ?? false;

    /// <summary>
    /// Gets the current view name
    /// </summary>
    public string? CurrentView => _currentView;

    /// <summary>
    /// Registers a page type with a key
    /// </summary>
    /// <param name="key">Page key</param>
    /// <param name="pageType">Page type</param>
    public void RegisterPage(string key, Type pageType)
    {
        _pages[key] = pageType;
    }

    /// <summary>
    /// Sets the main frame for navigation
    /// </summary>
    /// <param name="frame">Main frame</param>
    public void SetMainFrame(Frame frame)
    {
        _mainFrame = frame;
    }

    /// <summary>
    /// Navigates to a page
    /// </summary>
    /// <param name="pageKey">Page key</param>
    /// <param name="parameter">Navigation parameter</param>
    /// <returns>True if navigation was successful</returns>
    public async Task<bool> NavigateToAsync(string pageKey, object? parameter = null)
    {
        if (_mainFrame == null)
            return false;

        if (!_pages.TryGetValue(pageKey, out var pageType))
            return false;

        try
        {
            var page = Activator.CreateInstance(pageType);
            if (page is Page wpfPage)
            {
                if (parameter != null && wpfPage.DataContext != null)
                {
                    // Set parameter if the DataContext supports it
                    var parameterProperty = wpfPage.DataContext.GetType().GetProperty("Parameter");
                    parameterProperty?.SetValue(wpfPage.DataContext, parameter);
                }

                var previousView = _currentView;
                _mainFrame.Navigate(wpfPage);
                _currentView = pageKey;

                // Raise navigation event
                Navigated?.Invoke(this, new NavigationEventArgs
                {
                    ViewName = pageKey,
                    Parameter = parameter,
                    PreviousView = previousView
                });

                return true;
            }
        }
        catch
        {
            // Log error in real implementation
        }

        return false;
    }

    /// <summary>
    /// Navigates to a specific view model type
    /// </summary>
    /// <typeparam name="TViewModel">Type of the view model</typeparam>
    /// <param name="parameter">Optional parameter to pass to the view model</param>
    /// <returns>True if navigation was successful</returns>
    public async Task<bool> NavigateToAsync<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var viewModelName = typeof(TViewModel).Name;
        var pageKey = viewModelName.Replace("ViewModel", "");
        return await NavigateToAsync(pageKey, parameter);
    }

    /// <summary>
    /// Navigates back to the previous view
    /// </summary>
    /// <returns>True if navigation was successful</returns>
    public async Task<bool> GoBackAsync()
    {
        return await Task.FromResult(GoBack());
    }

    /// <summary>
    /// Navigates back
    /// </summary>
    /// <returns>True if navigation was successful</returns>
    public bool GoBack()
    {
        if (_mainFrame?.CanGoBack == true)
        {
            var previousView = _currentView;
            _mainFrame.GoBack();

            // Raise navigation event
            Navigated?.Invoke(this, new NavigationEventArgs
            {
                ViewName = "Previous",
                PreviousView = previousView
            });

            return true;
        }
        return false;
    }

    /// <summary>
    /// Navigates forward
    /// </summary>
    /// <returns>True if navigation was successful</returns>
    public bool GoForward()
    {
        if (_mainFrame?.CanGoForward == true)
        {
            _mainFrame.GoForward();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Clears navigation history
    /// </summary>
    public void ClearHistory()
    {
        if (_mainFrame != null)
        {
            while (_mainFrame.CanGoBack)
            {
                _mainFrame.RemoveBackEntry();
            }
        }
    }
}

/// <summary>
/// Event arguments for navigation events
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
    /// Name of the previous view
    /// </summary>
    public string? PreviousView { get; set; }
}
