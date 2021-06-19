using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using UIMonthControl;
using MonthEvent;
using System.Collections.Generic;

namespace DestinyNet
{
    public interface IViewModel 
    {
    }

    public class WeekViewModelBase : IViewModel, INotifyPropertyChanged
    {

        private DateTime _date;

        public WeekViewModelBase()
        {
            _date = DateTime.Now;
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

    public class YearViewModel : WeekViewModelBase
    {    }
    public class MonthViewModel : WeekViewModelBase
    { }
    public class WeekViewModel : WeekViewModelBase
    { }
    public class ToDoViewModel : WeekViewModelBase
    { }

    public class ManagerViewModel :  INotifyPropertyChanged
    {
        private ObservableCollection<IDateRange> _dateRanges;
        private ObservableCollection<ICalendar> _calendas;
        private ObservableCollection<IEvent> _events;
        private DateTime _dateMonthEvents;
        private ViewModelEnum _selectedViewModelEnum;
        private Dictionary<ViewModelEnum, IViewModel> _viewModelsDictionary;

        public ManagerViewModel()
        {
            _dateMonthEvents = DateTime.Now;

            _viewModelsDictionary = new Dictionary<ViewModelEnum, IViewModel>();
            _viewModelsDictionary.Add(ViewModelEnum.Month, new MonthViewModel());
            _viewModelsDictionary.Add(ViewModelEnum.Week, new WeekViewModel());
            _viewModelsDictionary.Add(ViewModelEnum.Year, new YearViewModel());
            _viewModelsDictionary.Add(ViewModelEnum.ToDo, new ToDoViewModel());
            _selectedViewModelEnum = ViewModelEnum.Month;


            _dateRanges = new ObservableCollection<IDateRange>();
            _dateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-11-08"), Finish = DateTime.Parse("2021-11-11") });
            _dateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-10-01"), Finish = DateTime.Parse("2021-10-11") });

            _calendas = new ObservableCollection<ICalendar>();
            _calendas.Add(new Calendar() { Name = "Work", Enabled = true, Color = Brushes.Blue });
            _calendas.Add(new Calendar() { Name = "Home", Enabled = true, Color = Brushes.Green });
            
            _events = new ObservableCollection<IEvent>();
            _events.Add(new Event() { Caption = "Work", Date = DateTime.Parse("2021-06-08"), Color = Brushes.Blue });
            _events.Add(new Event() { Caption = "Work 2", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Green });
            _events.Add(new Event() { Caption = "222", Date = DateTime.Parse("2021-06-11"), Color = Brushes.GreenYellow});
            _events.Add(new Event() { Caption = "33333", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Red });
            _events.Add(new Event() { Caption = "4444", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Bisque });
        }


        public IViewModel SelectedViewModel
        {
            get { return _viewModelsDictionary[_selectedViewModelEnum]; }
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
        public ObservableCollection<IDateRange> DateRanges
        {
            get { return _dateRanges; }
            set
            {
                _dateRanges = value;
                OnPropertyChanged("DateRanges");
            }
        }
        public ObservableCollection<ICalendar> Calendars
        {
            get { return _calendas; }
            set
            {
                _calendas = value;
                OnPropertyChanged("Calendars");
            }
        }
        public ObservableCollection<IEvent> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged("Events");
            }
        }
        public DateTime DateMonthEvents
        {
            get { return _dateMonthEvents; }
            set
            {
                _dateMonthEvents = value;
                OnPropertyChanged("DateMonthEvents");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
