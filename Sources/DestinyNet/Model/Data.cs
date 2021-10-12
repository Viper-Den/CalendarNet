using System.Collections.ObjectModel;
using Destiny.Core;

namespace DestinyNet
{

    public class Data
    {
        public ObservableCollection<Calendar> Calendars { get; private set; }
        public ObservableCollectionWithItemNotify<Event> Events { get; private set; }
        public ObservableCollection<DTask> Tasks { get; private set; }
        public ObservableCollection<Person> People { get; private set; }

        public Data()
        {
            Calendars = new ObservableCollection<Calendar>();
            Events = new ObservableCollectionWithItemNotify<Event>();
            Tasks = new ObservableCollection<DTask>();
            People = new ObservableCollection<Person>();
        }
    }
}
