using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace INES.ERP.WPF.Converters;

/// <summary>
/// Converts boolean to visibility (inverse of BooleanToVisibilityConverter)
/// </summary>
public class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }
        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            return visibility != Visibility.Visible;
        }
        return false;
    }
}

/// <summary>
/// Converts boolean to text based on parameter
/// </summary>
public class BooleanToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is string paramString)
        {
            var parts = paramString.Split('|');
            if (parts.Length == 2)
            {
                return boolValue ? parts[0] : parts[1];
            }
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts boolean to icon kind based on parameter
/// </summary>
public class BooleanToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is string paramString)
        {
            var parts = paramString.Split('|');
            if (parts.Length == 2)
            {
                return boolValue ? parts[0] : parts[1];
            }
        }
        return "Help";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts null to visibility
/// </summary>
public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isInverse = parameter?.ToString()?.ToLower() == "inverse";
        bool isNull = value == null;
        
        if (isInverse)
        {
            return isNull ? Visibility.Collapsed : Visibility.Visible;
        }
        else
        {
            return isNull ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts string to visibility
/// </summary>
public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isInverse = parameter?.ToString()?.ToLower() == "inverse";
        bool isEmpty = string.IsNullOrWhiteSpace(value?.ToString());
        
        if (isInverse)
        {
            return isEmpty ? Visibility.Collapsed : Visibility.Visible;
        }
        else
        {
            return isEmpty ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts enum to string
/// </summary>
public class EnumToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            return enumValue.ToString();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && targetType.IsEnum)
        {
            try
            {
                return Enum.Parse(targetType, stringValue);
            }
            catch
            {
                return Enum.GetValues(targetType).GetValue(0);
            }
        }
        return null!;
    }
}

/// <summary>
/// Formats date values
/// </summary>
public class DateFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            string format = parameter?.ToString() ?? "yyyy-MM-dd";
            return dateTime.ToString(format, culture);
        }
        if (value is DateTimeOffset dateTimeOffset)
        {
            string format = parameter?.ToString() ?? "yyyy-MM-dd";
            return dateTimeOffset.ToString(format, culture);
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && DateTime.TryParse(stringValue, culture, DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        return null!;
    }
}

/// <summary>
/// Formats currency values
/// </summary>
public class CurrencyFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal decimalValue)
        {
            string format = parameter?.ToString() ?? "C";
            return decimalValue.ToString(format, culture);
        }
        if (value is double doubleValue)
        {
            string format = parameter?.ToString() ?? "C";
            return doubleValue.ToString(format, culture);
        }
        if (value is float floatValue)
        {
            string format = parameter?.ToString() ?? "C";
            return floatValue.ToString(format, culture);
        }
        return "0.00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Currency, culture, out decimal result))
        {
            return result;
        }
        return 0m;
    }
}

/// <summary>
/// Formats percentage values
/// </summary>
public class PercentageFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal decimalValue)
        {
            return (decimalValue / 100).ToString("P", culture);
        }
        if (value is double doubleValue)
        {
            return (doubleValue / 100).ToString("P", culture);
        }
        if (value is float floatValue)
        {
            return (floatValue / 100).ToString("P", culture);
        }
        return "0%";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && decimal.TryParse(stringValue.Replace("%", ""), out decimal result))
        {
            return result * 100;
        }
        return 0m;
    }
}
