using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Destiny.Core
{
    public enum TitleControlType
    {
        Title,
        Button,
        WeekTitle,
        WeekTitleDayOff
    }

    public enum DayType
    {
        Day,
        DayFinish,
        Today,
        DayOff,
        DayOffFinish
    }
    public interface ITitleControl
    {
        Brush Background { get; set; }
        Brush Foreground { get; set; }
        string Text { get; set; }
        TitleControlType Type { get; set; }
        Visibility Visibility { get; set; }

    }
    public interface IDayControl
    {
        Visibility Visibility { get; set; }
        Brush Foreground { get; set; }
        Brush Background { get; set; }
        DateTime Date { get; set; }
        DayType Type { get; }
    }
    public class ControlBrush
    {
        public ControlBrush(Brush background, Brush foreground)
        {
            Background = background;
            Foreground = foreground;
        }
        public Brush Foreground { get; set; }
        public Brush Background { get; set; }
    }
    public enum EvementStyle
    {
        DayNoMonth,
        Day,
        DayFinish,
        DayOff,
        DayOffFinish,
        ToDay,
        SelectedDefault
    }

    public class Palette
    {
        private Dictionary<EvementStyle, ControlBrush> _evementStyles;
        public Palette()
        {
            _evementStyles = new Dictionary<EvementStyle, ControlBrush>();
            _evementStyles[EvementStyle.DayNoMonth] = new ControlBrush(Brushes.Transparent, Brushes.Gainsboro);
            _evementStyles[EvementStyle.Day] = new ControlBrush(Brushes.Transparent, Brushes.Black);
            _evementStyles[EvementStyle.DayFinish] = new ControlBrush(new SolidColorBrush(Color.FromRgb(230, 230, 230)), Brushes.Black);
            _evementStyles[EvementStyle.DayOff] = new ControlBrush(new SolidColorBrush(Color.FromRgb(230, 230, 230)), Brushes.Black);
            _evementStyles[EvementStyle.DayOffFinish] = new ControlBrush(new SolidColorBrush(Color.FromRgb(210, 210, 210)), Brushes.Black);
            _evementStyles[EvementStyle.ToDay] = new ControlBrush(new SolidColorBrush(Color.FromRgb(17, 110, 190)), Brushes.White);
            _evementStyles[EvementStyle.SelectedDefault] = new ControlBrush(Brushes.Gray, Brushes.White);
            Selected = SelectedDefault;
            ViewBorderingMonths = Visibility.Hidden;
        }
        public string Name { get; set; }
        public ControlBrush DayNoMonth { get => _evementStyles[EvementStyle.DayNoMonth]; }
        public ControlBrush DayOff { get => _evementStyles[EvementStyle.DayOff]; }
        public ControlBrush DayFinish { get => _evementStyles[EvementStyle.DayFinish]; }
        public ControlBrush DayOffFinish { get => _evementStyles[EvementStyle.DayOffFinish]; }
        public ControlBrush Selected { get; set; }
        public ControlBrush SelectedDefault { get => _evementStyles[EvementStyle.SelectedDefault]; }
        public ControlBrush ToDay { get => _evementStyles[EvementStyle.ToDay]; }
        public ControlBrush Day { get => _evementStyles[EvementStyle.Day]; }
        public Visibility ViewBorderingMonths { get; set; }
        public virtual void PaintTitle(ITitleControl title, DateTime date)
        {
            switch (title.Type)
            {
                case TitleControlType.Title:
                    title.Text = date.ToString("MMMM yyyy");
                    break;
                case TitleControlType.WeekTitle:
                    title.Text = date.ToString("ddd");
                    title.Foreground = Day.Foreground;
                    title.Background = Day.Background;
                    break;
                case TitleControlType.WeekTitleDayOff:
                    title.Text = date.ToString("ddd");
                    title.Foreground = DayOff.Foreground;
                    title.Background = DayOff.Background;
                    break;
                case TitleControlType.Button:
                    title.Visibility = Visibility.Visible;
                    break;
            }
        }
        public virtual void PaintDay(IDayControl day, DateTime date)
        {
            if (day.Date.Month != date.Month)
            {
                day.Foreground = DayNoMonth.Foreground;
                day.Background = DayNoMonth.Background;
                return;
            }

            switch (day.Type)
            {
                case DayType.Today:
                    day.Foreground = ToDay.Foreground;
                    day.Background = ToDay.Background;
                    break;
                case DayType.Day:
                    day.Foreground = Day.Foreground;
                    day.Background = Day.Background;
                    break;
                case DayType.DayOff:
                    day.Foreground = DayOff.Foreground;
                    day.Background = DayOff.Background;
                    break;
                case DayType.DayFinish:
                    day.Foreground = DayFinish.Foreground;
                    day.Background = DayFinish.Background;
                    break;
                case DayType.DayOffFinish:
                    day.Foreground = DayFinish.Foreground;
                    day.Background = DayOffFinish.Background;
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
    public class PalleteYear : Palette
    {

        public override void PaintTitle(ITitleControl title, DateTime date)
        {
            switch (title.Type)
            {
                case TitleControlType.Title:
                    title.Text = date.ToString("MMMM");
                    break;
                case TitleControlType.WeekTitle:
                    title.Text = date.ToString("ddd");
                    title.Foreground = Day.Foreground;
                    title.Background = Day.Background;
                    break;
                case TitleControlType.WeekTitleDayOff:
                    title.Text = date.ToString("ddd");
                    title.Foreground = DayOff.Foreground;
                    title.Background = DayOff.Background;
                    break;
                case TitleControlType.Button:
                    title.Visibility = Visibility.Hidden;
                    break;
            }
        }
        public override void PaintDay(IDayControl day, DateTime date)
        {
            if (day.Date.Month != date.Month)
            {
                day.Visibility = Visibility.Collapsed;
                return;
            }
            base.PaintDay(day, date);
        }
    }
    public class PalleteMounthEvent : Palette
    {

        public override void PaintTitle(ITitleControl title, DateTime date)
        {
            switch (title.Type)
            {
                case TitleControlType.Title:
                    title.Text = date.ToString("MMMM yyyy");
                    break;
                case TitleControlType.WeekTitle:
                    title.Text = date.ToString("dddd");
                    title.Background = Day.Background;
                    title.Foreground = Day.Foreground;
                    break;
                case TitleControlType.WeekTitleDayOff:
                    title.Text = date.ToString("dddd");
                    title.Background = DayOff.Background;
                    title.Foreground = DayOff.Foreground;
                    break;
            }
        }
        public override void PaintDay(IDayControl day, DateTime date)
        {
            if (day.Date.Month != date.Month)
            {
                switch (day.Date.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        day.Background = DayOffFinish.Background;
                        day.Foreground = DayOffFinish.Foreground;
                        break;
                    case DayOfWeek.Sunday:
                        day.Background = DayOffFinish.Background;
                        day.Foreground = DayOffFinish.Foreground;
                        break;
                    default:
                        day.Background = DayFinish.Background;
                        day.Foreground = DayFinish.Foreground;
                        break;
                }
                return;
            }

            switch (day.Type)
            {
                case DayType.Today:
                    day.Background = ToDay.Background;
                    day.Foreground = ToDay.Foreground;
                    break;
                case DayType.DayOff:
                    day.Background = DayOff.Background;
                    day.Foreground = DayOff.Foreground;
                    break;
                default:
                    day.Background = Day.Background;
                    day.Foreground = Day.Foreground;
                    break;
            }
        }
    }
}
