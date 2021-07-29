using MonthEvent;
using UIMonthControl;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DestinyNet
{

    public class Data
    {
        public ObservableCollection<Calendar> Calendars { get; set; }
        public ObservableCollection<IEvent> Events { get; set; }
        public ObservableCollection<IDateRange> DateRanges { get; set; }

        public Data()
        {
            DateRanges = new ObservableCollection<IDateRange>();
            Calendars = new ObservableCollection<Calendar>();
            Events = new ObservableCollection<IEvent>();
        }
    }
}
