﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Destiny.Core
{
    public class Palette
    {
        private Dictionary<EvementStyleType, ControlStyle> _evementStyles;
        public Palette()
        {
            _evementStyles = new Dictionary<EvementStyleType, ControlStyle>();
            _evementStyles[EvementStyleType.DayNoMonth] = new ControlStyle(Brushes.Transparent, Brushes.Gainsboro);
            _evementStyles[EvementStyleType.Day] = new ControlStyle(Brushes.Transparent, Brushes.Black);
            _evementStyles[EvementStyleType.DayFinish] = new ControlStyle(new SolidColorBrush(Color.FromRgb(230, 230, 230)), Brushes.Black);
            _evementStyles[EvementStyleType.DayOff] = new ControlStyle(new SolidColorBrush(Color.FromRgb(230, 230, 230)), Brushes.Black);
            _evementStyles[EvementStyleType.DayOffFinish] = new ControlStyle(new SolidColorBrush(Color.FromRgb(210, 210, 210)), Brushes.Black);
            _evementStyles[EvementStyleType.ToDay] = new ControlStyle(new SolidColorBrush(Color.FromRgb(17, 110, 190)), Brushes.White);
            _evementStyles[EvementStyleType.SelectedDefault] = new ControlStyle(Brushes.Gray, Brushes.White);
            Selected = SelectedDefault;
            ViewBorderingMonths = Visibility.Hidden;
        }
        public string Name { get; set; }
        public ControlStyle DayNoMonth { get => _evementStyles[EvementStyleType.DayNoMonth]; }
        public ControlStyle DayOff { get => _evementStyles[EvementStyleType.DayOff]; }
        public ControlStyle DayFinish { get => _evementStyles[EvementStyleType.DayFinish]; }
        public ControlStyle DayOffFinish { get => _evementStyles[EvementStyleType.DayOffFinish]; }
        public ControlStyle Selected { get; set; }
        public ControlStyle SelectedDefault { get => _evementStyles[EvementStyleType.SelectedDefault]; }
        public ControlStyle ToDay { get => _evementStyles[EvementStyleType.ToDay]; }
        public ControlStyle Day { get => _evementStyles[EvementStyleType.Day]; }
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
}
