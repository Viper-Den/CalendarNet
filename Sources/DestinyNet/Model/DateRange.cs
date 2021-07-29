using System;
using System.Collections.Generic;
using System.Text;
using UIMonthControl;
using System.Windows.Media;

namespace DestinyNet
{
    public class DateRange: IDateRange
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public SolidColorBrush Color { get => Calendar.Color; }
        public Calendar Calendar { get; set; }
        public string Info { get; set; }
    }
}
