using MonthEvent;
using System;
using System.Windows.Media;

namespace DestinyNet
{
    public class Event: IEvent
    {
        public string Caption { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public SolidColorBrush Color { get => Calendar.Color; }
        public Calendar Calendar { get; set; }
    }
}

