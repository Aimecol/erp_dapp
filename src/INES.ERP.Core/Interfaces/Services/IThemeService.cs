namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Theme service interface for managing application themes
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Gets the current theme
    /// </summary>
    Theme CurrentTheme { get; }

    /// <summary>
    /// Gets all available themes
    /// </summary>
    IEnumerable<Theme> AvailableThemes { get; }

    /// <summary>
    /// Sets the application theme
    /// </summary>
    /// <param name="themeName">Name of the theme to apply</param>
    /// <returns>True if theme was applied successfully</returns>
    Task<bool> SetThemeAsync(string themeName);

    /// <summary>
    /// Sets the application theme
    /// </summary>
    /// <param name="theme">Theme to apply</param>
    /// <returns>True if theme was applied successfully</returns>
    Task<bool> SetThemeAsync(Theme theme);

    /// <summary>
    /// Toggles between light and dark themes
    /// </summary>
    /// <returns>The new theme that was applied</returns>
    Task<Theme> ToggleThemeAsync();

    /// <summary>
    /// Gets a theme by name
    /// </summary>
    /// <param name="themeName">Name of the theme</param>
    /// <returns>Theme or null if not found</returns>
    Theme? GetTheme(string themeName);

    /// <summary>
    /// Saves the current theme preference
    /// </summary>
    /// <returns>Task</returns>
    Task SaveThemePreferenceAsync();

    /// <summary>
    /// Loads the saved theme preference
    /// </summary>
    /// <returns>Task</returns>
    Task LoadThemePreferenceAsync();

    /// <summary>
    /// Event raised when theme changes
    /// </summary>
    event EventHandler<ThemeChangedEventArgs>? ThemeChanged;
}

/// <summary>
/// Represents an application theme
/// </summary>
public class Theme
{
    /// <summary>
    /// Theme name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display name for the theme
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Theme description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this is a dark theme
    /// </summary>
    public bool IsDark { get; set; }

    /// <summary>
    /// Primary color
    /// </summary>
    public string PrimaryColor { get; set; } = string.Empty;

    /// <summary>
    /// Secondary color
    /// </summary>
    public string SecondaryColor { get; set; } = string.Empty;

    /// <summary>
    /// Background color
    /// </summary>
    public string BackgroundColor { get; set; } = string.Empty;

    /// <summary>
    /// Surface color
    /// </summary>
    public string SurfaceColor { get; set; } = string.Empty;

    /// <summary>
    /// Text color
    /// </summary>
    public string TextColor { get; set; } = string.Empty;

    /// <summary>
    /// Resource dictionary URI for the theme
    /// </summary>
    public string? ResourceUri { get; set; }
}

/// <summary>
/// Theme changed event arguments
/// </summary>
public class ThemeChangedEventArgs : EventArgs
{
    /// <summary>
    /// Previous theme
    /// </summary>
    public Theme? PreviousTheme { get; set; }

    /// <summary>
    /// New theme
    /// </summary>
    public Theme NewTheme { get; set; } = null!;
}
