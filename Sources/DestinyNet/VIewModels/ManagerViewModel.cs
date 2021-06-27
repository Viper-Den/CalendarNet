using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using UIMonthControl;
using MonthEvent;
using System.Collections.Generic;

namespace DestinyNet
{

    public class Data
    {
        public ObservableCollection<IDateRange> DateRanges{ get; set; }
        public ObservableCollection<ICalendar> Calendars { get; set; }
        public ObservableCollection<IEvent> Events { get; set; }

        public Data()
        {
            DateRanges = new ObservableCollection<IDateRange>();
            DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-06-08"), Finish = DateTime.Parse("2021-06-13") });
            DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-06-01"), Finish = DateTime.Parse("2021-06-01") });

            Calendars = new ObservableCollection<ICalendar>();
            Calendars.Add(new Calendar() { Name = "Work", Enabled = true, Color = Brushes.Blue });
            Calendars.Add(new Calendar() { Name = "Home", Enabled = true, Color = Brushes.Green });

            Events = new ObservableCollection<IEvent>();
            Events.Add(new Event() { Caption = "Work", Date = DateTime.Parse("2021-06-08"), Color = Brushes.Blue });
            Events.Add(new Event() { Caption = "Work 2", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Green });
            Events.Add(new Event() { Caption = "222", Date = DateTime.Parse("2021-06-11"), Color = Brushes.GreenYellow });
            Events.Add(new Event() { Caption = "33333", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Red });
            Events.Add(new Event() { Caption = "4444", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Bisque });
        }
    }


    public interface IViewModel 
    {
    }

    public class ViewModelBase : IViewModel, INotifyPropertyChanged
    {

        private DateTime _date;
        protected Data _data;

        public ViewModelBase(Data data)
        {
            _date = DateTime.Now;
            _data = data;
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class YearViewModel : ViewModelBase
    {
        public YearViewModel(Data data): base(data)
        {
            
        }
        public ObservableCollection<IDateRange> DateRanges
        {
            get => _data.DateRanges;
        }
    }
    public class MonthViewModel : ViewModelBase
    {
        public MonthViewModel(Data data) : base(data)
        {

        }
        public ObservableCollection<IEvent> Events
        {
            get => _data.Events;
        }
    }
    public class WeekViewModel : ViewModelBase
    {
        public WeekViewModel(Data data) : base(data)
        {

        }
        public ObservableCollection<IEvent> Events
        {
            get => _data.Events;
        }
    }
    public class ToDoViewModel : ViewModelBase
    {
        public ToDoViewModel(Data data) : base(data)
        {

        }
    }
    public class ToolPanelViewModel : ViewModelBase
    {
        public ToolPanelViewModel(Data data) : base(data)
        {

        }
        public ObservableCollection<ICalendar> Calendars
        {
            get => _data.Calendars;
        }
    }
    

    public class ManagerViewModel :  INotifyPropertyChanged
    {
        private DateTime _dateMonthEvents;
        private ViewModelEnum _selectedViewModelEnum;
        private IViewModel _ToolPanelViewModel;
        private Dictionary<ViewModelEnum, IViewModel> _viewModelsDictionary;

        public ManagerViewModel()
        {
            _dateMonthEvents = DateTime.Now;
            //var FilePath = System.Reflection.Assembly.GetExecutingAssembly().Location + "Configuration.json";
            //var s = new AppSettingsBuilder();
            //s.CreateConfiguration(FilePath);
            //var l = s.BuildSettingsModels<DateRange>("DateRanges");
            
            var d = new Data();
            _viewModelsDictionary = new Dictionary<ViewModelEnum, IViewModel>();
            _viewModelsDictionary.Add(ViewModelEnum.Month, new MonthViewModel(d));
            _viewModelsDictionary.Add(ViewModelEnum.Week, new WeekViewModel(d));
            _viewModelsDictionary.Add(ViewModelEnum.Year, new YearViewModel(d));
            _viewModelsDictionary.Add(ViewModelEnum.ToDo, new ToDoViewModel(d));
            _selectedViewModelEnum = ViewModelEnum.Month;
            _ToolPanelViewModel = new ToolPanelViewModel(d);
        }


        public IViewModel SelectedViewModel
        {
            get { return _viewModelsDictionary[_selectedViewModelEnum]; }
        }
        public IViewModel ToolPanel
        {
            get { return _ToolPanelViewModel; }
        }

        public ViewModelEnum SelectiewModelEnum
        {
            get { return (_selectedViewModelEnum); }
            set
            {
                _selectedViewModelEnum = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }


    public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
