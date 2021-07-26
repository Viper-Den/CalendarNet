using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace DestinyNet
{
    public class ToolPanelViewModel : ViewModeDataBase, ICalendarsEditor
    {
        private readonly IDialogViewsManager _dialogViewsManager;
        private ICalendar _selectCalendar;
        public ICommand AddCalendarCommand { get; }
        public ICommand EditCalendarCommand { get; }

        public ToolPanelViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data)
        {
            _dialogViewsManager = dialogViewsManager;
            Calendars = new ObservableCollection<ICalendar>();
            EditCalendarCommand = new ActionCommand(EditCalendar);
            AddCalendarCommand = new ActionCommand(AddCalendar);
            Update();
        }
        private void AddCalendar(object o)
        {
            var c = new Calendar() { Color = new SolidColorBrush(Colors.White), Enabled=true, Name = "New Calendar"};
            _data.Calendars.Add(c);
            Calendars.Add(c);
            SelectedCalendar = c;
            EditCalendar(null);
        }
        private void EditCalendar(object o)
        {
            ICalendar c = o as ICalendar;
            if (c == null)
                return;

            SelectedCalendar = c;
            _dialogViewsManager.ShowDialogView(new CalendarViewModel(this, _dialogViewsManager.ClosePopUpViewCommand), true);
        }
        public void Update()
        {
            Calendars.Clear();
            foreach (var c in _data.Calendars)
            {
               Calendars.Add(c);
            }
            OnPropertyChanged(nameof(Calendars));
        }

        public ICalendar SelectedCalendar
        { 
            get => _selectCalendar;
            set => SetField(ref _selectCalendar, value);
        }
        public ObservableCollection<ICalendar> Calendars  { get; }
    }
}
