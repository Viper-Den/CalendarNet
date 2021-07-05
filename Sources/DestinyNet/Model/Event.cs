using System;
using MonthEvent;
using System.Windows.Media;

namespace DestinyNet
{
    public class Event : IEvent
    {
        public string Caption { get; set; }
        public DateTime Date { get; set; }
        public SolidColorBrush Color { get; set; }
    }
}
