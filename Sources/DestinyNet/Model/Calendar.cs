using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace DestinyNet
{
    public interface ICalendar
    {
        public string Name { get; set; }
        public SolidColorBrush Color { get; set; }
        public Boolean Enabled { get; set; }

    }
    public class Calendar : ICalendar
    {
        public string Name { get; set; }
        public SolidColorBrush Color { get; set; }
        public Boolean Enabled { get; set; }
    }
}
