using MonthEvent;
using UIMonthControl;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Destiny.Core;

namespace DestinyNet
{

    public class Data
    {
        public ObservableCollection<Calendar> Calendars { get; set; }
        public ObservableCollection<IEvent> Events { get; set; }

        public Data()
        {
            Calendars = new ObservableCollection<Calendar>();
            Events = new ObservableCollection<IEvent>();
        }
    }
}
