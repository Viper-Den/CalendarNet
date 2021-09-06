using Destiny.Core;
using System;
namespace DestinyNet
{
    public class EventDTO
    {
        public string Caption { get; set; }
        public string CalendarGUID { get; set; }
        public RuleRepeatTypes Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public int Step { get; set; }
        public DateTime FinishRepeatDate { get; set; }
        public bool IsDayStep { get; set; }
        public bool IsAllDay { get; set; }
        public bool IsWorkDayStep { get; set; }
        public bool IsRepeatDay { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public bool IsSunday { get; set; }
    }
}
