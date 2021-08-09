using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Converters
{
    /// <summary>
    /// Bool value to radius property of blur effect converter
    /// </summary>
    public class BoolToVisibilityUsageConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public BoolToVisibilityUsageConverter()
        {
            // set defaults
            TrueValue = Visibility.Hidden;
            FalseValue = Visibility.Visible; 
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return FalseValue;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
