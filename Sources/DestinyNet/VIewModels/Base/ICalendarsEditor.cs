using System.Collections.ObjectModel;
using Destiny.Core;

namespace DestinyNet.ViewModels
{
    public interface ICalendarsEditor
    {
        public ObservableCollection<Calendar> Calendars { get; }
        public Calendar SelectedCalendar { get; set; }
    }
}
