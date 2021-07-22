using System.Collections.ObjectModel;

namespace DestinyNet
{
    public interface ICalendarsEditor
    {
        public ObservableCollection<ICalendar> Calendars { get; }
        public ICalendar SelectedCalendar { get; set; }
    }
}
