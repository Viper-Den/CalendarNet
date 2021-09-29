using System;

namespace Destiny.Core
{
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
