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
        public ObservableCollectionWithItemNotify<Event> Events { get; set; }
        public ObservableCollection<DTask> Tasks { get; set; }

        public Data()
        {
            Calendars = new ObservableCollection<Calendar>();
            Events = new ObservableCollectionWithItemNotify<Event>();
            Tasks = new ObservableCollection<DTask>();
        }
    }
}
