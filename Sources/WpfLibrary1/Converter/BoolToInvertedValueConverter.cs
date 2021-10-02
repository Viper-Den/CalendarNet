using System;
using System.Windows.Data;

namespace Converters
{
    /// <summary>
    /// Converter, used to provide inverted value for input boolean
    /// </summary>
    public class BoolToInvertedValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return true;

            var boolValue = (bool)value;

            if (boolValue)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            var boolValue = (bool)value;

            if (boolValue)
                return false;

            return false;
        }
    }
}
