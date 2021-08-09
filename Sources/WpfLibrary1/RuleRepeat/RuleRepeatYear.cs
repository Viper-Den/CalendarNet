using System;

namespace Destiny.Core
{
    public class RuleRepeatYear : RuleRepeat
    {
        public DateTime EndDate
        {
            get
            {
                if (RepeatCount == 0)
                    return Finish.AddYears(MAX_PLANNING_HORIZONT);
                else
                    return Start.AddYears(Step * RepeatCount);
            }
        }
        public override bool IsDate(DateTime date)
        {
            if (date < Start)
                return false;
            if ((RepeatCount != 0) && (date > EndDate))
                return false;

            return ((date.Day == Start.Day) && ((date.Year - Start.Year) % Step == 0));
        }
    }
}
