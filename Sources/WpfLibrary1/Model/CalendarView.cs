using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Destiny.Core
{
    public class CalendarView : BaseViewModel
    {
        private ObservableCollectionWithItemNotify<Event> _sourceEvents;
        private bool _isOpened;

        public CalendarView(Calendar calendar, ObservableCollectionWithItemNotify<Event> source)
        {
            _sourceEvents = source;
            Calendar = calendar;
            Events = new ObservableCollection<Event>();
            foreach (var ev in _sourceEvents)
            {
                if (ev.Calendar == Calendar)
                    Events.Add(ev);
            }
            _sourceEvents.CollectionChanged += DoCollectionChangedEvents;
            IsOpened = false;
        }
        ~CalendarView()
        {
            if(!(_sourceEvents == null))
                _sourceEvents.CollectionChanged -= DoCollectionChangedEvents;
        }
        public SolidColorBrush BackgroundColor { get => Brushes.Transparent; }// new SolidColorBrush(Color.FromArgb(200, Calendar.Color.Color.R, Calendar.Color.Color.G, Calendar.Color.Color.B)); }
        public Calendar Calendar { get; private set; }
        public bool IsOpened
        {
            get => _isOpened;
            set { 
                SetField(ref _isOpened, value);
                OnPropertyChanged(IsOpeneble);
            }
        }
        public bool IsOpeneble
        {
            get => (Events.Count != 0);
        }

        public ObservableCollection<Event> Events { get; private set; }
        public ICommand SelectedEventCommand { get => new ActionCommand(SelectedEvent); }
        public void SelectedEvent(object o)
        {
            if (!(o is Event))
                return;
            SelectedEventAction?.Invoke((Event)o);
        }
        public Action<Event> SelectedEventAction { get; set; }
        public ICommand OpenEventsViewCommand { get => new ActionCommand(OpenEventsView); }
        public void OpenEventsView(object o)
        {
            IsOpened = !IsOpened;
        }
        public Action<CalendarView> OnMenuOpened { get; set; }
        private void DoCollectionChangedEvents(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var ev in e.NewItems)
                    {
                        if((((Event)ev).Calendar == Calendar)&&(!Events.Contains((Event)ev)))
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