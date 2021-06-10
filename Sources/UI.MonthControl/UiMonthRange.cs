using System;
using System.Collections.Generic;
using System.Text;

namespace UIMonthControl
{
    public interface IDateRange
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

    }
}
