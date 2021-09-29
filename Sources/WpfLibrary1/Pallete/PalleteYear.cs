using System;
using System.Windows;

namespace Destiny.Core
{
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
}
