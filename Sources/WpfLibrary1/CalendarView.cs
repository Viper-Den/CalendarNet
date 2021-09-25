using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Destiny.Core
{
    public class CalendarView : BaseViewModel
    {
        private ObservableCollectionWithItemNotify<Event> _sourceEvents;
        public CalendarView(ObservableCollectionWithItemNotify<Event> source)
        {
            _sourceEvents = source;
            Events = new ObservableCollection<Event>();
            foreach (var ev in _sourceEvents)
            {
                Events.Add(ev);
            }
            _sourceEvents.CollectionChanged += DoCollectionChangedEvents;
        }
        public Calendar Calendar { get; set; }
        public Boolean IsOpen { get; set; }
        public ObservableCollection<Event> Events { get; private set; }
        private void DoCollectionChangedEvents(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var ev in e.OldItems)
                    {
                        if(((Event)ev).Calendar == Calendar)
                            Events.Add((Event)ev);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var ev in e.OldItems)
                    {
                        if (((Event)ev).Calendar == Calendar)
                            Events.Remove((Event)ev);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Events.Clear();
                    foreach (var ev in _sourceEvents)
                    {
                        if (ev.Calendar == Calendar)
                            Events.Add(ev);
                    }
                    break;
            }
        }
    }
}