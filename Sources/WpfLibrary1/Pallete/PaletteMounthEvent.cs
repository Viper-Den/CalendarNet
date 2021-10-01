using System;
using System.Windows;

namespace Destiny.Core
{
    public class PaletteMounthEvent : Palette
    {
        public override void PaintTitle(ITitleControl control, DateTime date)
        {
            switch (control.Type)
            {
                case TitleControlType.Title:
                    control.Text = date.ToString("MMMM yyyy");
                    break;
                case TitleControlType.WeekTitle:
                    control.Text = date.ToString("dddd");
                    control.Background = Day.Background;
                    control.Foreground = Day.Foreground;
                    break;
                case TitleControlType.WeekTitleDayOff:
                    control.Text = date.ToString("dddd");
                    control.Background = DayOff.Background;
                    control.Foreground = DayOff.Foreground;
                    break;
                case TitleControlType.Button:
                    control.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentException("Type not supports");
            }
        }
        public override void PaintDay(IDayControl control, DateTime date)
        {
            if (control.Date.Month != date.Month)
            {
                switch (control.Date.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        control.Background = DayOffFinish.Background;
                        control.Foreground = DayOffFinish.Foreground;
                        break;
                    case DayOfWeek.Sunday:
                        control.Background = DayOffFinish.Background;
                        control.Foreground = DayOffFinish.Foreground;
                        break;
                    default:
                        control.Background = DayFinish.Background;
                        control.Foreground = DayFinish.Foreground;
                        break;
                }
                return;
            }

            switch (control.Type)
            {
                case DayType.Today:
                    control.Background = ToDay.Background;
                    control.Foreground = ToDay.Foreground;
                    break;
                case DayType.DayOff:
                    control.Background = DayOff.Background;
                    control.Foreground = DayOff.Foreground;
                    break;
                default:
                    control.Background = Day.Background;
                    control.Foreground = Day.Foreground;
                    break;
            }
        }
    }
}
