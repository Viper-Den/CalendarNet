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
        DayType Type {  get; }
    }

    public class Pallete
    {
        public Pallete()
        {
            DayFinish = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            DayOff = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            DayOffFinish = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            ToDay = new SolidColorBrush(Color.FromRgb(17, 110, 190));
            Selected = Brushes.Green;
            ViewBorderingMonths = Visibility.Hidden;
        }
        public SolidColorBrush DayOff { get; set; }
        public SolidColorBrush DayFinish { get; set; }
        public SolidColorBrush DayOffFinish { get; set; }
        public SolidColorBrush Selected { get; set; }
        public SolidColorBrush ToDay { get; set; }
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
                    title.Background = Brushes.Transparent;
                    break;
                case TitleControlType.WeekTitleDayOff:
                    title.Text = date.ToString("ddd");
                    title.Background = DayOff;
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
                day.Foreground = Brushes.Gainsboro;
                day.Background = Brushes.Transparent;
                return;
            }

            switch (day.Type)
            {
                case DayType.Today:
                    day.Foreground = Brushes.White;
                    day.Background = ToDay;
                    break;
                case DayType.Day:
                    day.Foreground = Brushes.Black;
                    day.Background = Brushes.Transparent;
                    break;
                case DayType.DayOff:
                    day.Foreground = Brushes.Black;
                    day.Background = DayOff;
                    break;
                case DayType.DayFinish:
                    day.Foreground = Brushes.Black;
                    day.Background = DayFinish;
                    break;
                case DayType.DayOffFinish:
                    day.Foreground = Brushes.Black;
                    day.Background = DayOffFinish;
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
    public class PalleteYear : Pallete
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
                    title.Background = Brushes.Transparent;
                    break;
                case TitleControlType.WeekTitleDayOff:
                    title.Text = date.ToString("ddd");
                    title.Background = DayOff;
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
    public class PalleteMounthEvent : Pallete
    {

        public override void PaintTitle(ITitleControl title, DateTime date)
        {
            switch (title.Type)
            {
                case TitleControlType.Title:
                    title.Text = date.ToString("MMMM yyyy");
                    break;
                case TitleControlType.WeekTitle:
                    title.Text = date.ToString("ddd");
                    title.Background = Brushes.Transparent;
                    break;
                case TitleControlType.WeekTitleDayOff:
                    title.Text = date.ToString("ddd");
                    title.Background = DayOff;
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
                        day.Foreground = Brushes.Black;
                        day.Background = DayOffFinish;
                        break;
                    case DayOfWeek.Sunday:
                        day.Foreground = Brushes.Black;
                        day.Background = DayOffFinish;
                        break;
                    default:
                        day.Foreground = Brushes.Black;
                        day.Background = DayFinish;
                        break; 
                }
                return;
            }

            switch (day.Type)
            {
                case DayType.Today:
                    day.Foreground = Brushes.White;
                    day.Background = ToDay;
                    break;
                case DayType.Day:
                    day.Foreground = Brushes.Black;
                    day.Background = Brushes.Transparent;
                    break;
                case DayType.DayOff:
                    day.Foreground = Brushes.Black;
                    day.Background = DayOff;
                    break;
                case DayType.DayFinish:
                    day.Foreground = Brushes.Black;
                    day.Background = Brushes.Transparent;
                    break;
                case DayType.DayOffFinish:
                    day.Foreground = Brushes.Black;
                    day.Background = DayOff;
                    break;
            }
        }
    }
}
