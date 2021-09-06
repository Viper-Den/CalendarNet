using System;

namespace Destiny.Core
{
    public class RuleRepeat
    {
        protected const int MAX_PLANNING_HORIZONT = 100;
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public int Step { get; set; }
        public DateTime FinishRepeatDate { get; set; }

        public virtual bool IsDate(DateTime date)
        {
            return (Start.Date == date.Date);
        }
    }
}
