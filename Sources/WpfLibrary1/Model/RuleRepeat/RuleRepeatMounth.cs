using System;

namespace Destiny.Core
{
    public class RuleRepeatMounth : RuleRepeat
    {
        public override bool IsDate(DateTime date)
        {
            if (Start.Date == date.Date)
                return true;
            if ((date.Date < Start.Date) || (date.Date > FinishRepeatDate.Date))
                return false;

            return ((date.Day == Start.Day) && ((date.Month - Start.Month) % Step == 0));
        }
    }
}
