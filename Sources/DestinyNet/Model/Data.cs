using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DestinyNet
{

    public class Data
    {
        public ObservableCollection<Calendar> Calendars { get; set; }
        public ObservableCollection<Event> Events { get; set; }
        public ObservableCollection<DateRange> DateRanges { get; set; }

        public Data()
        {
            DateRanges = new ObservableCollection<DateRange>();
            Calendars = new ObservableCollection<Calendar>();
            Events = new ObservableCollection<Event>();
        }
    }
}
