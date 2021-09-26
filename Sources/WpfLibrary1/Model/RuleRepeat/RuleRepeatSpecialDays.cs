using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Destiny.Core
{
    public class RuleRepeatSpecialDays: RuleRepeat
    {
        public RuleRepeatSpecialDays()
        {
            SpecialDays = new List<DateTime>();
        }
        public List<DateTime> SpecialDays { get; set; }
        public override bool IsDate(DateTime date)
        {
            return SpecialDays.Contains(date);
        }
    }
}
