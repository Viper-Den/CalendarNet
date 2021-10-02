using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Destiny.Core
{
    public class Palette: IPalette
    {
        private Dictionary<EvementStyleType, EvementStyle> _evementStyleDictionary;
        public Palette()
        {
            EvementStyleCollection = new ObservableCollection<EvementStyle>();
               _evementStyleDictionary = new Dictionary<EvementStyleType, EvementStyle>();
            _evementStyleDictionary[EvementStyleType.DayNoMonth] = new EvementStyle(Brushes.Transparent, Brushes.Gainsboro);
            _evementStyleDictionary[EvementStyleType.Day] = new EvementStyle(Brushes.Transparent, Brushes.Black);
            _evementStyleDictionary[EvementStyleType.DayFinish] = new EvementStyle(new SolidColorBrush(Color.FromArgb(150, 200, 200, 200)), Brushes.Black);
            _evementStyleDictionary[EvementStyleType.DayOff] = new EvementStyle(new SolidColorBrush(Color.FromArgb(180, 200, 200, 200)), Brushes.Black);
            _evementStyleDictionary[EvementStyleType.DayOffFinish] = new EvementStyle(new SolidColorBrush(Color.FromArgb(240, 200, 200, 200)), Brushes.Black);
            _evementStyleDictionary[EvementStyleType.ToDay] = new EvementStyle(new SolidColorBrush(Color.FromRgb(17, 110, 190)), Brushes.White);
            _evementStyleDictionary[EvementStyleType.SelectedDefault] = new EvementStyle(Brushes.Gray, Brushes.White);
            Selected = new EvementStyle(Brushes.Gray, Brushes.White);

            EvementStyleCollection = new ObservableCollection<EvementStyle>(_evementStyleDictionary.Values);
            ViewBorderingMonths = Visibility.Hidden;
        }
        public string Name { get; set; }
        public ObservableCollection<EvementStyle> EvementStyleCollection { get; }
        public EvementStyle DayNoMonth { get => _evementStyleDictionary[EvementStyleType.DayNoMonth]; }
        public EvementStyle DayOff { get => _evementStyleDictionary[EvementStyleType.DayOff]; }
        public EvementStyle DayFinish { get => _evementStyleDictionary[EvementStyleType.DayFinish]; }
        public EvementStyle DayOffFinish { get => _evementStyleDictionary[EvementStyleType.DayOffFinish]; }
        public EvementStyle Selected { get; set; }
        public EvementStyle SelectedDefault { get => _evementStyleDictionary[EvementStyleType.SelectedDefault]; }
        public EvementStyle ToDay { get => _evementStyleDictionary[EvementStyleType.ToDay]; }
        public EvementStyle Day { get => _evementStyleDictionary[EvementStyleType.Day]; }
        public Visibility ViewBorderingMonths { get; set; }
        public virtual void PaintTitle(ITitleControl control, DateTime date)
        {
            switch (control.Type)
            {
                case TitleControlType.Title:
                    control.Text = date.ToString("MMMM yyyy");
                    break;
                case TitleControlType.WeekTitle:
                    control.Text = date.ToString("ddd");
                    control.Foreground = Day.Foreground;
                    control.Background = Day.Background;
                    break;
                case TitleControlType.WeekTitleDayOff:
                    control.Text = date.ToString("ddd");
                    control.Foreground = DayOff.Foreground;
                    control.Background = DayOff.Background;
                    break;
                case TitleControlType.Button:
                    control.Visibility = Visibility.Visible;
                    break;
            }
        }
        public virtual void PaintDay(IDayControl control, DateTime date)
        {
            if (control.Date.Month != date.Month)
            {
                control.Foreground = DayNoMonth.Foreground;
                control.Background = DayNoMonth.Background;
                return;
            }

            switch (control.Type)
            {
                case DayType.Today:
                    control.Foreground = ToDay.Foreground;
                    control.Background = ToDay.Background;
                    break;
                case DayType.Day:
                    control.Foreground = Day.Foreground;
                    control.Background = Day.Background;
                    break;
                case DayType.DayOff:
                    control.Foreground = DayOff.Foreground;
                    control.Background = DayOff.Background;
                    break;
                case DayType.DayFinish:
                    control.Foreground = DayFinish.Foreground;
                    control.Background = DayFinish.Background;
                    break;
                case DayType.DayOffFinish:
                    control.Foreground = DayFinish.Foreground;
                    control.Background = DayOffFinish.Background;
                    break;
            }
        }
        public static DayType GetDateType(DateTime date)
        {

            if (date.Date == DateTime.Today)
                return DayType.Today;
            else if (date.Date < DateTime.Today.Date)
            {
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        return DayType.DayOffFinish;
                    case DayOfWeek.Sunday:
                        return DayType.DayOffFinish;
                    default:
                        return DayType.DayFinish;
                }
            }
            else
            {
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        return DayType.DayOff;
                    case DayOfWeek.Sunday:
                        return DayType.DayOff;
                    default:
                        return DayType.Day;
                }
            }
        }
    }
}
