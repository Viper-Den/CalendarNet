using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Converters
{
    public class ColorToFontColorConverter
    {
        private const int DEF_MAX_VALUE = 150;
        public SolidColorBrush DartValue { get; set; }
        public SolidColorBrush WhiteValue { get; set; }

        public ColorToFontColorConverter()
        {
            // set defaults
            DartValue = Brushes.DarkGray;
            WhiteValue = Brushes.White;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SolidColorBrush))
                return DartValue;
            var v = (SolidColorBrush)value;
            if ((v.Color.R > DEF_MAX_VALUE) && (v.Color.G > DEF_MAX_VALUE) && (v.Color.B > DEF_MAX_VALUE))
                return WhiteValue;
            else
                return DartValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
