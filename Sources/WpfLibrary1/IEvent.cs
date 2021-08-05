using System;
using System.Windows.Media;

namespace Destiny.Core
{
    public interface IEvent
    {
        public string Caption { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public SolidColorBrush Color { get; }
    }
}
