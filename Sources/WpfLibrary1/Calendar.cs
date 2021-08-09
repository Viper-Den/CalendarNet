using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Destiny.Core
{
    public class Calendar
    {
        public Calendar()
        {
            GUID = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public SolidColorBrush Color { get; set; }
        public Boolean Enabled { get; set; }
        public string GUID { get; set; }
    }
}
