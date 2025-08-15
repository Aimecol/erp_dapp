using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace INES.ERP.WPF.Services;

/// <summary>
/// Navigation service for WPF application
/// </summary>
public class NavigationService
{
    private readonly Dictionary<string, Type> _pages = new();
    private readonly IServiceProvider _serviceProvider;
    private Frame? _mainFrame;
    private string? _currentView;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

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
        System.Diagnostics.Debug.WriteLine($"NavigateToAsync called with pageKey: {pageKey}");

        if (_mainFrame == null)
        {
            System.Diagnostics.Debug.WriteLine("MainFrame is null!");
            return false;
        }

        if (!_pages.TryGetValue(pageKey, out var pageType))
        {
            System.Diagnostics.Debug.WriteLine($"Page type not found for key: {pageKey}");
            System.Diagnostics.Debug.WriteLine($"Available pages: {string.Join(", ", _pages.Keys)}");
            return false;
        }

        System.Diagnostics.Debug.WriteLine($"Found page type: {pageType.Name}");

        try
        {
            // Use service provider to create the page instance
            System.Diagnostics.Debug.WriteLine($"Creating page instance for type: {pageType.Name}");
            var page = _serviceProvider.GetRequiredService(pageType);
            System.Diagnostics.Debug.WriteLine($"Page instance created: {page?.GetType().Name}");

            if (page is Page wpfPage)
            {
                if (parameter != null && wpfPage.DataContext != null)
                {
                    // Set parameter if the DataContext supports it
                    var parameterProperty = wpfPage.DataContext.GetType().GetProperty("Parameter");
                    parameterProperty?.SetValue(wpfPage.DataContext, parameter);
                }

                var previousView = _currentView;
                System.Diagnostics.Debug.WriteLine($"Navigating to page: {wpfPage.GetType().Name}");
                _mainFrame.Navigate(wpfPage);
                _currentView = pageKey;
                System.Diagnostics.Debug.WriteLine($"Navigation completed. Current view: {_currentView}");

                // Raise navigation event
                Navigated?.Invoke(this, new NavigationEventArgs
                {
                    ViewName = pageKey,
                    Parameter = parameter,
                    PreviousView = previousView
                });

                return true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Page is not a WPF Page: {page?.GetType().Name}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
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
