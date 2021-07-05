using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DestinyNet
{
    public class ToolPanelViewModel : ViewModeDateBase
    {
        private readonly IDialogViewsManager _dialogViewsManager;
        private readonly ViewModelBase _viewModelCalendar;
        private ICalendar _selectCalendar;

        public ToolPanelViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data)
        {
            //if(dialogViewsManager == null) 
            _dialogViewsManager = dialogViewsManager;
            _viewModelCalendar = new ViewModelCalendar();
            Calendars = new ObservableCollection<ICalendar>();
            AddCalendar = new RelayCommand(DoAddCalendar, o => true);
            SelectCalendar = new RelayCommand(DoSelectedCalendar, o => true);
            Update();
        }

        private void DoSelectedCalendar(object o)
        {
            SelectedCalendar = o as ICalendar;
            if (SelectedCalendar != null)
            {
                _dialogViewsManager.ShowDialogView(_viewModelCalendar, true);
            }
            {
                _dialogViewsManager.ShowDialogView(null);
            }
        }
        private void DoAddCalendar(object o)
        {
            Calendars.Add(new Calendar());
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

        public ICommand AddCalendar { get; }
        public ICommand DelateCalendar { get; }
        public ICommand EditCalendar { get; }
        public ICommand SelectCalendar { get; }
        public ICalendar SelectedCalendar
        { 
            get => _selectCalendar;
            set => SetField(ref _selectCalendar, value);
        }
        
        public ObservableCollection<ICalendar> Calendars  { get; }
    }
}
