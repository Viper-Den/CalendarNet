using System;
using System.Globalization;
using System.Windows.Data;

namespace Converters
{
    /// <summary>
    /// Bool value to radius property of blur effect converter
    /// </summary>
    public class BoolToBlurUsageConverter : IValueConverter
    {
        public double TrueValue { get; set; }
        public double FalseValue { get; set; }

        public BoolToBlurUsageConverter()
        {
            // set defaults
            TrueValue = 5;
            FalseValue = 0;
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
