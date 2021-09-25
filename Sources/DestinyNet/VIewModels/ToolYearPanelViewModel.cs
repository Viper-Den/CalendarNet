using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Destiny.Core;

namespace DestinyNet
{
    public class ToolYearPanelViewModel : ViewModeDataBase
    {
        private Calendar _selectCalendar;
        private ObservableCollection<CalendarView> _CalendarViewCollection;
        private Dictionary<Calendar, CalendarView> _dictionaryCalendarsEvents; 

        public ToolYearPanelViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
            _CalendarViewCollection = new ObservableCollection<CalendarView>();
            _data.Calendars.CollectionChanged += DoCollectionChangedCalendars;
            _dictionaryCalendarsEvents = new Dictionary<Calendar, CalendarView>();
            FillEvents();
        }
        public void FillEvents()
        {
            _CalendarViewCollection.Clear();
            _dictionaryCalendarsEvents.Clear();
            foreach (var c in _data.Calendars)
            {
                var cv = new CalendarView(_data.Events) { Calendar = c, IsOpen = false };
                _dictionaryCalendarsEvents.Add(c, cv);
                _CalendarViewCollection.Add(cv);
            }
        }
        private void DoCollectionChangedCalendars(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var c in e.OldItems)
                    {
                        if(!_dictionaryCalendarsEvents.ContainsKey((Calendar)c))
                            _dictionaryCalendarsEvents.Add((Calendar)c, new CalendarView(_data.Events) { Calendar = (Calendar)c, IsOpen = false });
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var c in e.OldItems)
                    {
                        if (_dictionaryCalendarsEvents.ContainsKey((Calendar)c))
                            _dictionaryCalendarsEvents.Remove((Calendar)c);
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
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
    }
}
