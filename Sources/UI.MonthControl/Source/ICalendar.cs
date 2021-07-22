using System;
using System.Windows.Media;

namespace UIMonthControl
{
    public interface ICalendar
    {
        public string Name { get; set; }
        public SolidColorBrush Color { get; set; }
        public Boolean Enabled { get; set; }

    }
}
