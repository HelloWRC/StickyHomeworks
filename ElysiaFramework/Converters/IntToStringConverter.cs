using System;
using System.Globalization;
using System.Windows.Data;

namespace ElysiaFramework.Converters;

public class IntToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var r = System.Convert.ToString(value);
        return r ?? "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return int.TryParse((string)value, out _) ? int.Parse((string)value) : (double.TryParse((string)value, out _)
            ? double.Parse((string)value)
            : 0);
    }
}