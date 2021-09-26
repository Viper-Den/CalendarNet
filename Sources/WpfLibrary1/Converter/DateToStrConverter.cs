using System;
using System.Globalization;
using System.Windows.Data;

namespace Converters
{
    public class DateToStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("dd.MM.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse((string)value);
        }
    }
}
