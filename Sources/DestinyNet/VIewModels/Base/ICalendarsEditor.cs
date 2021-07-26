using System.Collections.ObjectModel;

namespace DestinyNet
{
    public interface ICalendarsEditor
    {
        public ObservableCollection<Calendar> Calendars { get; }
        public Calendar SelectedCalendar { get; set; }
    }
}
