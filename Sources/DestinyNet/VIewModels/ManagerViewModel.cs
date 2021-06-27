using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using UIMonthControl;
using MonthEvent;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace DestinyNet
{

    public class Data
    {
        public List<DateRange> DateRanges{ get; set; }
        public List<Calendar> Calendars { get; set; }
        public List<Event> Events { get; set; }

        public Data()
        {
            DateRanges = new List<DateRange>();
            Calendars = new List<Calendar>();
            Events = new List<Event>();
            //DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-06-08"), Finish = DateTime.Parse("2021-06-13") });
            //DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-06-01"), Finish = DateTime.Parse("2021-06-01") });

            //Calendars.Add(new Calendar() { Name = "Work", Enabled = true, Color = Brushes.Blue });
            //Calendars.Add(new Calendar() { Name = "Home", Enabled = true, Color = Brushes.Green });

            //Events.Add(new Event() { Caption = "Work", Date = DateTime.Parse("2021-06-08"), Color = Brushes.Blue });
            //Events.Add(new Event() { Caption = "Work 2", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Green });
            //Events.Add(new Event() { Caption = "222", Date = DateTime.Parse("2021-06-11"), Color = Brushes.GreenYellow });
            //Events.Add(new Event() { Caption = "33333", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Red });
            //Events.Add(new Event() { Caption = "4444", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Bisque });
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
        private ObservableCollection<IDateRange> _DateRanges;
        public YearViewModel(Data data) : base(data)
        {
            _DateRanges = new ObservableCollection<IDateRange>();
            Update();
        }
        public void Update()
        {
            _DateRanges.Clear();
            foreach(var r in _data.DateRanges)
            {
              _DateRanges.Add(r);
            }
            OnPropertyChanged("DateRanges");
        }
        public ObservableCollection<IDateRange> DateRanges
        {
            get => _DateRanges;
        }
    }
    public class MonthViewModel : ViewModelBase
    {
        private ObservableCollection<IEvent> _Events;
        public MonthViewModel(Data data) : base(data)
        {
            _Events = new ObservableCollection<IEvent>();
            Update();
        }
        public ObservableCollection<IEvent> Events
        {
            get => _Events;
        }
        public void Update()
        {
            _Events.Clear();
            foreach (var e in _data.Events)
            {
                _Events.Add(e);
            }
            OnPropertyChanged("Events");
        }
    }

    public class WeekViewModel : ViewModelBase
    {
        private ObservableCollection<IEvent> _Events;
        public WeekViewModel(Data data) : base(data)
        {
            _Events = new ObservableCollection<IEvent>();
            Update();
        }
        public ObservableCollection<IEvent> Events
        {
            get => _Events;
        }
        public void Update()
        {
            _Events.Clear();
            foreach (var e in _data.Events)
            {
                _Events.Add(e);
            }
            OnPropertyChanged("Events");
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
        private ObservableCollection<ICalendar> _Calendars;
        public ToolPanelViewModel(Data data) : base(data)
        {
            _Calendars = new ObservableCollection<ICalendar>();
            Update();
        }
        public ObservableCollection<ICalendar> Calendars
        {
            get => _Calendars;
        }
        public void Update()
        {
            _Calendars.Clear();
            foreach (var c in _data.Calendars)
            {
                _Calendars.Add(c);
            }
            OnPropertyChanged("Calendars");
        }
    }
    

    public class ManagerViewModel :  INotifyPropertyChanged
    {
        private ViewModelEnum _selectedViewModelEnum;
        private IViewModel _ToolPanelViewModel;
        private Dictionary<ViewModelEnum, IViewModel> _viewModelsDictionary;

        public ManagerViewModel()
        {

            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Configuration.json";
            var data = new Data();
            if (!File.Exists(path))
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            else
            {
                var s = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<Data>(s); 
            }


            _viewModelsDictionary = new Dictionary<ViewModelEnum, IViewModel>();
            _viewModelsDictionary.Add(ViewModelEnum.Month, new MonthViewModel(data));
            _viewModelsDictionary.Add(ViewModelEnum.Week, new WeekViewModel(data));
            _viewModelsDictionary.Add(ViewModelEnum.Year, new YearViewModel(data));
            _viewModelsDictionary.Add(ViewModelEnum.ToDo, new ToDoViewModel(data));
            _selectedViewModelEnum = ViewModelEnum.Month;
            _ToolPanelViewModel = new ToolPanelViewModel(data);
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
