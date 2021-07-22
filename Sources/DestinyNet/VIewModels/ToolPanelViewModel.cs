using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DestinyNet
{
    public class ToolPanelViewModel : ViewModeDateBase, ICalendarsEditor
    {
        private readonly IDialogViewsManager _dialogViewsManager;
        private ICalendar _selectCalendar;

        public ToolPanelViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data)
        {
            _dialogViewsManager = dialogViewsManager;
            Calendars = new ObservableCollection<ICalendar>();
            SelectCalendar = new ActionCommand(DoSelectedCalendar);
            Update();
        }

        private void DoSelectedCalendar(object o)
        {
            SelectedCalendar = o as ICalendar;
            if (SelectedCalendar != null)
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

        public ICommand SelectCalendar { get; }
        public ICalendar SelectedCalendar
        { 
            get => _selectCalendar;
            set => SetField(ref _selectCalendar, value);
        }
        public ObservableCollection<ICalendar> Calendars  { get; }
    }
}
