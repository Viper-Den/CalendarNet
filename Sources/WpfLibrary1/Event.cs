using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Destiny.Core
{
    public class Event: BaseViewModel
    {
        private RuleRepeatTypes _type;
        private Calendar _Calendar;
        private string _Caption;
        public Event()
        {
            _type = RuleRepeatTypes.None;
            Rule = new RuleRepeat();
            _Calendar = null;
    }
        public string Caption
        {
                get => _Caption;
                set => SetField(ref _Caption, value);
        }
        public Calendar Calendar
        {
            get => _Calendar;
            set 
            {
                SetField(ref _Calendar, value);
                OnPropertyChanged(nameof(Color));
            }
        }
        public bool IsAllDay { get; set; }
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
                        Rule = new RuleRepeatDay() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, FinishRepeatDate = Rule.FinishRepeatDate };
                    break;
                    case RuleRepeatTypes.Week:
                        Rule = new RuleRepeatWeek() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, FinishRepeatDate = Rule.FinishRepeatDate };
                        break;
                    case RuleRepeatTypes.Mounth:
                        Rule = new RuleRepeatMounth() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, FinishRepeatDate = Rule.FinishRepeatDate };
                    break;
                    case RuleRepeatTypes.Year:
                        Rule = new RuleRepeatYear() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, FinishRepeatDate = Rule.FinishRepeatDate };
                    break;
                    default:
                        Rule = new RuleRepeat() { Start = Rule.Start, Finish = Rule.Finish, Step = Rule.Step, FinishRepeatDate = Rule.FinishRepeatDate }; 
                    break;
                }

            } 
        }
        public RuleRepeat Rule { get; private set; }
    }
}

