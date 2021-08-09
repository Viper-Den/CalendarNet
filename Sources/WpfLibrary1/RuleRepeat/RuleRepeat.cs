using System;

namespace Destiny.Core
{
    public enum CompareResult
    {
        Less,
        None,
        More
    }
    public class RuleRepeat
    {
        protected const int MAX_PLANNING_HORIZONT = 100;
        protected const int DAYS_IN_WEEK = 7;
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public int Step { get; set; }
        public int RepeatCount { get; set; }

        private bool CompareDate(DateTime a, DateTime b)
        {
            return ((a.Day == b.Day) && (a.Month == b.Month) && (a.Year == b.Year));
            //|| CompareDate(date, Start) || CompareDate(date, Finish)
        }
        public virtual bool IsDate(DateTime date)
        {
            return ((Start.Date <= date.Date) && (date.Date <= Finish.Date));
        }
    }
}
