using System;
using System.Windows.Media;

namespace Destiny.Core
{
    public interface IEvent
    {
        public string Caption { get; set; }
        public SolidColorBrush Color { get; }
        public RuleRepeatTypes RuleType { get; set; }
        public RuleRepeat Rule { get; }
    }
}
