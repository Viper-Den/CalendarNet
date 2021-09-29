using System;
using System.Windows;

namespace Destiny.Core
{
    public class PalleteYear : Palette
    {
        public override void PaintTitle(ITitleControl control, DateTime date)
        {
            switch (control.Type)
            {
                case TitleControlType.Title:
                    control.Text = date.ToString("MMMM");
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
                    control.Visibility = Visibility.Hidden;
                    break;
                default:
                    throw new ArgumentException("Type not supports");
            }
        }
        public override void PaintDay(IDayControl control, DateTime date)
        {
            if (control.Date.Month != date.Month)
            {
                control.Visibility = Visibility.Collapsed;
                return;
            }
            base.PaintDay(control, date);
        }
    }
}
