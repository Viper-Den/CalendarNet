using System;

namespace Destiny.Core
{
    public class RuleRepeatWeek : RuleRepeat
    {
        public override bool IsDate(DateTime date)
        {
            if (Start.Date == date.Date)
                return true;
            if ((date.Date < Start.Date) || (date.Date > FinishRepeatDate.Date))
                return false;

            return ((date.Date - Start.Date).TotalDays % (Step*DateHelper.COUNT_DAYS_IN_WEEK) == 0);
        }
    }
}
