using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet
{
    public class CalendarViewModel : BaseViewModel
    {
        private readonly ICalendarsEditor _calendarEditor;

        public ICommand DeleteCalendarCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public Calendar SelectedCalendar { get => _calendarEditor.SelectedCalendar; }
        public CalendarViewModel(ICalendarsEditor calendarEditor, ICommand closeWindowCommand)
        {
            CloseWindowCommand = closeWindowCommand;
            _calendarEditor = calendarEditor;
            DeleteCalendarCommand = new ActionCommand(DelateCalendar);
        }
        private void DelateCalendar(object o)
        {
            if (_calendarEditor.SelectedCalendar == null)
                return;
            var c = _calendarEditor.SelectedCalendar;
            _calendarEditor.SelectedCalendar = null;
            _calendarEditor.Calendars.Remove(c);
            CloseWindowCommand?.Execute(null);
        }

    }

}
