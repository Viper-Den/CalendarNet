using System;

namespace Destiny.Core
{
    public class RuleRepeatDay : RuleRepeat
    {
        public bool IsDayStep { get; set; }
        public bool IsWorkDayStep { get; set; }
        public bool IsRepeatDay { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public bool IsSunday { get; set; }

        public DateTime EndDate { 
            get
            {
                if (RepeatCount == 0)
                    return Finish.AddYears(MAX_PLANNING_HORIZONT);

                if (IsDayStep)
                {
                    return Start.AddDays(Step*RepeatCount);
                }
                else if (IsWorkDayStep)
                {
                    return Start.AddDays(DAYS_IN_WEEK * RepeatCount);
                }
                else if (IsRepeatDay)
                {
                    return Start.AddDays(DAYS_IN_WEEK * RepeatCount);
                }
                else
                    return Finish;
            } 
        } 
        public override bool IsDate(DateTime date)
        {
            if (date < Start)
                return false;
            if ((RepeatCount != 0) && (date > EndDate))
                return false;

            if (IsDayStep)
            {
                return ((date - Start).TotalDays % Step == 0);
            }
            else if (IsWorkDayStep)
            {
                var dayOfWeek = date.DayOfWeek;
                return (dayOfWeek == DayOfWeek.Monday || 
                        dayOfWeek == DayOfWeek.Tuesday || 
                        dayOfWeek == DayOfWeek.Wednesday ||
                        dayOfWeek == DayOfWeek.Thursday ||
                        dayOfWeek == DayOfWeek.Friday);
            }
            else if (IsRepeatDay)
            {
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return IsMonday;
                    case DayOfWeek.Tuesday:
                        return IsTuesday;
                    case DayOfWeek.Wednesday:
                        return IsWednesday;
                    case DayOfWeek.Thursday:
                        return IsThursday;
                    case DayOfWeek.Friday:
                        return IsFriday;
                    default:
                        return false;
                }
            }
            else
                return false;
        }
    }
}
