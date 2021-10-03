using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Destiny.Core
{
    public class Palette: IPalette
    {
        public Palette()
        {
            DayNoMonth = new EvementStyle(Brushes.Transparent, Brushes.Gainsboro, EvementStyleType.DayNoMonth);
            Day = new EvementStyle(Brushes.Transparent, Brushes.Black, EvementStyleType.Day);
            DayFinish = new EvementStyle(new SolidColorBrush(Color.FromArgb(150, 200, 200, 200)), Brushes.Black, EvementStyleType.DayFinish);
            DayOff = new EvementStyle(new SolidColorBrush(Color.FromArgb(180, 200, 200, 200)), Brushes.Black, EvementStyleType.DayOff);
            DayOffFinish = new EvementStyle(new SolidColorBrush(Color.FromArgb(240, 200, 200, 200)), Brushes.Black, EvementStyleType.DayOffFinish);
            ToDay = new EvementStyle(new SolidColorBrush(Color.FromRgb(17, 110, 190)), Brushes.White, EvementStyleType.ToDay);
            SelectedDefault = new EvementStyle(Brushes.Gray, Brushes.White, EvementStyleType.SelectedDefault);
            Selected = new EvementStyle(Brushes.Gray, Brushes.White, EvementStyleType.SelectedDefault);
            ViewBorderingMonths = Visibility.Hidden;
            GUID = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public string GUID { get; set; }
        public EvementStyle DayNoMonth { get; set; }
        public EvementStyle DayOff { get; set; }
        public EvementStyle DayFinish { get; set; }
        public EvementStyle DayOffFinish { get; set; }
        public EvementStyle Selected { get; set; }
        public EvementStyle SelectedDefault { get; set; }
        public EvementStyle ToDay { get; set; }
        public EvementStyle Day { get; set; }
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
