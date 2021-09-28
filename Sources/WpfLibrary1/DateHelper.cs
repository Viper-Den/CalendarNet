using System;
using System.Collections.Generic;
using System.Text;

namespace Destiny.Core
{
    public class DateHelper
    {
        private static Dictionary<DayOfWeek, int> _IndexDays;
        public static int COUNT_DAYS_IN_WEEK = 7;
        public static int GetIndexDay(DayOfWeek dayOfWeek)
        {
            return _IndexDays[dayOfWeek];
        }
        public static int GetIndexDay(DateTime date)
        {
            return _IndexDays[date.DayOfWeek];
        }
        public static DateTime GetWeekEndDate(DateTime date)
        {
            return date.Date.AddDays(6 - GetIndexDay(date.Date));
        }
        public const int MIN_IN_HOUR = 60;
        public const int HOUR_IN_DAY = 24;
        public const int MIN_IN_DAY = HOUR_IN_DAY * MIN_IN_HOUR;
        static DateHelper()
        {
            _IndexDays = new Dictionary<DayOfWeek, int>();
            _IndexDays.Add(DayOfWeek.Monday, 0);
            _IndexDays.Add(DayOfWeek.Tuesday, 1);
            _IndexDays.Add(DayOfWeek.Wednesday, 2);
            _IndexDays.Add(DayOfWeek.Thursday, 3);
            _IndexDays.Add(DayOfWeek.Friday, 4);
            _IndexDays.Add(DayOfWeek.Saturday, 5);
            _IndexDays.Add(DayOfWeek.Sunday, 6);
        }
    }
}
