using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using UIMonthControl;
using UIDayMonth;

namespace DestinyNet
{

    public class DateRange : IDateRange
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }

    public class Event : IEvent
    {
        public string Caption { get; set; }
        public DateTime Date { get; set; }
        public SolidColorBrush Color { get; set; }
    }

    public class VMCalendar : INotifyPropertyChanged
    {
        private ObservableCollection<IDateRange> _dateRanges;
        private ObservableCollection<ICalendar> _calendas;
        private ObservableCollection<IEvent> _events;
        private DateTime _dateYear;
        private DateTime _dateMonth;
        private DateTime _dateMonthEvents; 

        public VMCalendar()
        {
            _dateYear = DateTime.Now;
            _dateMonth = DateTime.Now;
            _dateMonthEvents = DateTime.Now;

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

        public DateTime DateYear
        {
            get { return _dateYear; }
            set
            {
                _dateYear = value;
                OnPropertyChanged("DateYear");
            }
        }
        public DateTime DateMonth
        {
            get { return _dateMonth; }
            set
            {
                _dateMonth = value;
                OnPropertyChanged("DateMonth");
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
