using System;
using System.Windows.Media;

namespace UIDayMonth
{
    public interface IEvent
    {
        public string Caption { get; set; }
        public DateTime Date { get; set; }
        public SolidColorBrush Color { get; set; }
    }
}
