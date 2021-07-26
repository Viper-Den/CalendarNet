using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace DestinyNet
{
    public class ToolPanelViewModel : ViewModeDataBase, ICalendarsEditor
    {
        private readonly IDialogViewsManager _dialogViewsManager;
        private Calendar _selectCalendar;

        public ICommand AddCalendarCommand { get; }
        public ICommand EditCalendarCommand { get; }

        public ToolPanelViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data)
        {
            _dialogViewsManager = dialogViewsManager;
            EditCalendarCommand = new ActionCommand(EditCalendar);
            AddCalendarCommand = new ActionCommand(AddCalendar);
        }
        private void AddCalendar(object o)
        {
            var c = new Calendar() { Color = new SolidColorBrush(Colors.White), Enabled=true, Name = "New Calendar"};
            Calendars.Add(c);
            SelectedCalendar = c;
            EditCalendar(null);
        }
        private void EditCalendar(object o)
        {
            var c = o as Calendar;
            if (c == null)
                return;

            SelectedCalendar = c;
            _dialogViewsManager.ShowDialogView(new CalendarViewModel(this, _dialogViewsManager.ClosePopUpViewCommand), true);
        }
        public Calendar SelectedCalendar
        { 
            get => _selectCalendar;
            set => SetField(ref _selectCalendar, value);
        }
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
    }
}
