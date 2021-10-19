using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet.ViewModels
{
    public class ToolYearPanelViewModel : ViewModeDataBase
    {
        private Calendar _selectCalendar;
        private Dictionary<Calendar, CalendarView> _dictionaryCalendarsEvents; 

        public ToolYearPanelViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
            Calendars = new ObservableCollection<CalendarView>();
            _data.Calendars.CollectionChanged += DoCollectionChangedCalendars;
            _dictionaryCalendarsEvents = new Dictionary<Calendar, CalendarView>();
            FillEvents();
        }
        public void FillEvents()
        {
            foreach (var c in _dictionaryCalendarsEvents.Values)
            {
                c.SelectedEventAction -= DoSelectedEventAction;
                c.OnMenuOpened -= DoMenuOpened;
            }
            Calendars.Clear();
            _dictionaryCalendarsEvents.Clear();
            foreach (var c in _data.Calendars)
            {
                AddCalendar(c);
            }
        }
        private void DoMenuOpened(CalendarView calendar)
        {
            foreach (var cv in Calendars)
            {
                if (calendar != cv)
                    cv.IsOpened = false;
            }
        }
        private void AddCalendar(Calendar calendar)
        {
            var cv = new CalendarView(calendar, _data.Events);
            Calendars.Add(cv);
            _dictionaryCalendarsEvents.Add(calendar, cv);
            cv.SelectedEventAction += DoSelectedEventAction;
            cv.OnMenuOpened += DoMenuOpened;
        }
        private void DoCollectionChangedCalendars(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var c in e.NewItems)
                    {
                        if (!_dictionaryCalendarsEvents.ContainsKey((Calendar)c))
                            AddCalendar((Calendar)c);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var c in e.OldItems)
                    {
                        if (_dictionaryCalendarsEvents.ContainsKey((Calendar)c))
                        {
                            _dictionaryCalendarsEvents[(Calendar)c].SelectedEventAction -= DoSelectedEventAction;
                            _dictionaryCalendarsEvents[(Calendar)c].OnMenuOpened -= DoMenuOpened;
                            Calendars.Remove(_dictionaryCalendarsEvents[(Calendar)c]);
                            _dictionaryCalendarsEvents.Remove((Calendar)c);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    FillEvents();
                    break;
            }
        }
        public Calendar SelectedCalendar
        {
            get => _selectCalendar;
            set { SetField(ref _selectCalendar, value); }
        }
        private void DoSelectedEventAction(Event ev)
        {
            if (ev == null)
                return;
            SelectedEventAction?.Invoke(ev);
        }
        public Action<Event> SelectedEventAction { get; set; }
        public ObservableCollection<CalendarView> Calendars { get; }
        public ICommand EditEventCommand { get => new ActionCommand(DoEditEvent); }
        public void DoEditEvent(object o)
        {
            EditEventAction?.Invoke((Event)o);
        }
        public Action<Event> EditEventAction { get; set; }

    }
}
