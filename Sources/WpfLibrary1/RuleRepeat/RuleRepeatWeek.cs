using System;

namespace Destiny.Core
{
    public class RuleRepeatWeek : RuleRepeat
    {
        public DateTime EndDate
        {
            get
            {
                if (RepeatCount == 0)
                    return Finish.AddYears(MAX_PLANNING_HORIZONT);
                else
                    return Start.AddDays(DAYS_IN_WEEK * Step * RepeatCount);
            }
        }
        public override bool IsDate(DateTime date)
        {
            if (date < Start)
                return false;
            if ((RepeatCount != 0) && (date > EndDate))
                return false;

            return ((date - Start).TotalDays % (Step*DAYS_IN_WEEK) == 0);
        }
    }
}
