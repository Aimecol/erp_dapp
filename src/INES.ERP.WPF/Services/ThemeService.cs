using System.Windows;
using MaterialDesignThemes.Wpf;

namespace INES.ERP.WPF.Services;

/// <summary>
/// Theme service for WPF application
/// </summary>
public class ThemeService
{
    private readonly PaletteHelper _paletteHelper;
    private string _currentTheme = "Light";

    public ThemeService()
    {
        _paletteHelper = new PaletteHelper();
    }

    /// <summary>
    /// Gets the current theme
    /// </summary>
    public ITheme CurrentTheme => _paletteHelper.GetTheme();

    /// <summary>
    /// Gets the current theme name
    /// </summary>
    public string CurrentThemeName => _currentTheme;

    /// <summary>
    /// Event raised when theme changes
    /// </summary>
    public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

    /// <summary>
    /// Applies a theme by name
    /// </summary>
    /// <param name="themeName">Name of the theme to apply</param>
    public void ApplyTheme(string themeName)
    {
        var previousTheme = _currentTheme;

        switch (themeName.ToLower())
        {
            case "light":
                ApplyLightTheme();
                break;
            case "dark":
                ApplyDarkTheme();
                break;
            default:
                ApplyLightTheme();
                break;
        }

        _currentTheme = themeName;

        // Raise theme changed event
        ThemeChanged?.Invoke(this, new ThemeChangedEventArgs
        {
            NewTheme = themeName,
            PreviousTheme = previousTheme
        });
    }

    /// <summary>
    /// Applies a light theme
    /// </summary>
    public void ApplyLightTheme()
    {
        var theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(Theme.Light);
        _paletteHelper.SetTheme(theme);
    }

    /// <summary>
    /// Applies a dark theme
    /// </summary>
    public void ApplyDarkTheme()
    {
        var theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(Theme.Dark);
        _paletteHelper.SetTheme(theme);
    }

    /// <summary>
    /// Toggles between light and dark theme
    /// </summary>
    public void ToggleTheme()
    {
        var theme = _paletteHelper.GetTheme();
        var isDark = theme.GetBaseTheme() == BaseTheme.Dark;
        theme.SetBaseTheme(isDark ? Theme.Light : Theme.Dark);
        _paletteHelper.SetTheme(theme);
    }

    /// <summary>
    /// Sets the primary color
    /// </summary>
    /// <param name="color">Primary color</param>
    public void SetPrimaryColor(System.Windows.Media.Color color)
    {
        var theme = _paletteHelper.GetTheme();
        theme.SetPrimaryColor(color);
        _paletteHelper.SetTheme(theme);
    }

    /// <summary>
    /// Sets the secondary color
    /// </summary>
    /// <param name="color">Secondary color</param>
    public void SetSecondaryColor(System.Windows.Media.Color color)
    {
        var theme = _paletteHelper.GetTheme();
        theme.SetSecondaryColor(color);
        _paletteHelper.SetTheme(theme);
    }

    /// <summary>
    /// Checks if the current theme is dark
    /// </summary>
    /// <returns>True if dark theme is active</returns>
    public bool IsDarkTheme()
    {
        var theme = _paletteHelper.GetTheme();
        return theme.GetBaseTheme() == BaseTheme.Dark;
    }

    /// <summary>
    /// Applies the INES-Ruhengeri brand colors
    /// </summary>
    public void ApplyBrandTheme()
    {
        var theme = _paletteHelper.GetTheme();
        
        // INES-Ruhengeri brand colors (example)
        var primaryColor = System.Windows.Media.Color.FromRgb(0, 123, 191); // Blue
        var secondaryColor = System.Windows.Media.Color.FromRgb(255, 193, 7); // Amber
        
        theme.SetPrimaryColor(primaryColor);
        theme.SetSecondaryColor(secondaryColor);
        
        _paletteHelper.SetTheme(theme);
    }
}

/// <summary>
/// Event arguments for theme changed events
/// </summary>
public class ThemeChangedEventArgs : EventArgs
{
    /// <summary>
    /// Name of the new theme
    /// </summary>
    public string NewTheme { get; set; } = string.Empty;

    /// <summary>
    /// Name of the previous theme
    /// </summary>
    public string PreviousTheme { get; set; } = string.Empty;
}
