using System;
using System.Windows.Media;

namespace Destiny.Core
{
    public class Event: IEvent
    {
        private RuleRepeatTypes _type;
        public Event()
        {
            _type = RuleRepeatTypes.None;
            Rule = new RuleRepeat();
        }
        public string Caption { get; set; }
        public Calendar Calendar { get; set; }
        public SolidColorBrush Color { get => Calendar.Color; }
        public RuleRepeatTypes RuleType 
        { 
            get => _type; 
            set 
            {
                if (_type == value)
                    return;
                _type = value;
                switch (_type)
                {
                    case RuleRepeatTypes.Days:
                        Rule = new RuleRepeatDay() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, RepeatCount = Rule.RepeatCount };
                    break;
                    case RuleRepeatTypes.Week:
                        Rule = new RuleRepeatWeek() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, RepeatCount = Rule.RepeatCount };
                        break;
                    case RuleRepeatTypes.Mounth:
                        Rule = new RuleRepeatMounth() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, RepeatCount = Rule.RepeatCount };
                    break;
                    case RuleRepeatTypes.Year:
                        Rule = new RuleRepeatYear() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, RepeatCount = Rule.RepeatCount };
                    break;
                    default:
                        Rule = new RuleRepeat() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, RepeatCount = Rule.RepeatCount }; 
                    break;
                }

            } 
        }
        public RuleRepeat Rule { get; private set; }
    }
}

