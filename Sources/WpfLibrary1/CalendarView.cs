using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Destiny.Core
{
    public class CalendarView : BaseViewModel
    {
        private ObservableCollectionWithItemNotify<Event> _sourceEvents;
        private Boolean _isOpened;
        public CalendarView(Calendar calendar, ObservableCollectionWithItemNotify<Event> source)
        {
            _sourceEvents = source;
            Calendar = calendar;
            IsOpened = false;
            Events = new ObservableCollection<Event>();
            foreach (var ev in _sourceEvents)
            {
                if (ev.Calendar == Calendar)
                    Events.Add(ev);
            }
            _sourceEvents.CollectionChanged += DoCollectionChangedEvents;
        }
        public void OpenEventsView(object o)
        {
            IsOpened = !IsOpened;
        }
        public Calendar Calendar { get; private set; }
        public Boolean IsOpened
        {
            get => _isOpened;
            set { SetField(ref _isOpened, value); }
        }
        public ObservableCollection<Event> Events { get; private set; }
        public ICommand OpenEventsViewCommand { get => new ActionCommand(OpenEventsView); }
        private void DoCollectionChangedEvents(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var ev in e.NewItems)
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