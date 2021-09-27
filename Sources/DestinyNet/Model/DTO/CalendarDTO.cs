using System;
using System.Windows.Media;

namespace DestinyNet
{
    public class CalendarDTO
    {
        public CalendarDTO()
        {
            GUID = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public SolidColorBrush Color { get; set; }
        public bool Enabled { get; set; }
        public string GUID { get; set; }
    }
}
