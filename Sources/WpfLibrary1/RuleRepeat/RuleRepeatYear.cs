using System;

namespace Destiny.Core
{
    public class RuleRepeatYear : RuleRepeat
    {
        public override bool IsDate(DateTime date)
        {
            if (Start.Date == date.Date)
                return true;
            if ((date.Date < Start.Date) || (date.Date > FinishRepeatDate.Date))
                return false;

            return ((date.Day == Start.Day) && ((date.Year - Start.Year) % Step == 0));
        }
    }
}
