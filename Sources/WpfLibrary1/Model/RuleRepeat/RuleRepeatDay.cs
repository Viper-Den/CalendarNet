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

        public override RuleRepeat Clone()
        {
            var r = new RuleRepeatDay();
            r.Start = this.Start;
            r.Finish = this.Finish;
            r.Step = this.Step;
            r.FinishRepeatDate = this.FinishRepeatDate;
            r.IsDayStep = this.IsDayStep;
            r.IsWorkDayStep = this.IsWorkDayStep;
            r.IsRepeatDay = this.IsRepeatDay;
            r.IsMonday = this.IsMonday;
            r.IsTuesday = this.IsTuesday;
            r.IsWednesday = this.IsWednesday;
            r.IsThursday = this.IsThursday;
            r.IsFriday = this.IsFriday;
            r.IsSaturday = this.IsSaturday;
            r.IsSunday = this.IsSunday;
            return r;
        }
        public override bool IsDate(DateTime date)
        {
            if (Start.Date == date.Date)
                return true;
            if ((date.Date < Start.Date) || (date.Date > FinishRepeatDate.Date))
                return false;

            if (IsDayStep)
            {
                return ((date.Date - Start.Date).TotalDays % Step == 0);
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
                    case DayOfWeek.Saturday:
                        return IsSaturday;
                    case DayOfWeek.Sunday:
                        return IsSunday;
                    default:
                        return false;
                }
            }
            else
                return false;
        }
    }
}
