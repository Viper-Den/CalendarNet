using System;

namespace Destiny.Core
{
    public class RuleRepeatMounth : RuleRepeat
    {
        public DateTime EndDate
        {
            get
            {
                if (RepeatCount == 0)
                    return Finish.AddYears(MAX_PLANNING_HORIZONT);
                else
                    return Start.AddMonths(Step * RepeatCount);
            }
        }
        public override bool IsDate(DateTime date)
        {
            if ((date < Start) || ((RepeatCount != 0) && (date > EndDate)))
                return false;

            return ((date.Day == Start.Day) && ((date.Month - Start.Month) % Step == 0));
        }
    }
}
