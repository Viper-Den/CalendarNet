using System;
using System.Collections.Generic;

namespace Destiny.Core
{
    public class RuleRepeat : BaseViewModel
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
        public virtual RuleRepeat Clone()
        {
            var r = new RuleRepeat();
            r.Start = this.Start;
            r.Finish = this.Finish;
            r.Step = this.Step;
            r.FinishRepeatDate = this.FinishRepeatDate;
            return r;
        }
    }
}
