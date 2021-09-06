using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Destiny.Core;
using System.Collections.Specialized;

namespace DestinyNet
{
    public class RepeatRuleViewModel
    {
        public RepeatRuleViewModel(RuleRepeatTypes ruleType, string name)
        {
            RuleType = ruleType;
            Name = name;
        }
        public RuleRepeatTypes RuleType { get; set; }
        public string Name { get; set; }
    }

    public class EventEditorViewModel : DialogBaseViewMode
    {
        private readonly Data _data;
        private RepeatRuleViewModel _selectedRepeatRules;
        private Dictionary<RuleRepeatTypes, EditorViewModel> _viewModelsDictionary;
        private Event _Event;
        public EventEditorViewModel(ICommand closeWindowCommand, Data data, DateTime date, Event editEvent = null) : base(closeWindowCommand)
        {
            _data = data;
            _viewModelsDictionary = new Dictionary<RuleRepeatTypes, EditorViewModel>();
            _viewModelsDictionary.Add(RuleRepeatTypes.None, null);
            _viewModelsDictionary.Add(RuleRepeatTypes.Days, new DaysEditorViewModel());
            _viewModelsDictionary.Add(RuleRepeatTypes.Week, new WeekEditorViewModel());
            _viewModelsDictionary.Add(RuleRepeatTypes.Mounth, new MounthEditorViewModel());
            _viewModelsDictionary.Add(RuleRepeatTypes.Year, new YearEditorViewModel());

            RepeatRules = new ObservableCollection<RepeatRuleViewModel>();
            RepeatRules.Add(new RepeatRuleViewModel(RuleRepeatTypes.None, ""));
            RepeatRules.Add(new RepeatRuleViewModel(RuleRepeatTypes.Days, "Повторять по дням"));
            RepeatRules.Add(new RepeatRuleViewModel(RuleRepeatTypes.Week, "Повторять по неделям"));
            RepeatRules.Add(new RepeatRuleViewModel(RuleRepeatTypes.Mounth, "Повторять по месяцам"));
            RepeatRules.Add(new RepeatRuleViewModel(RuleRepeatTypes.Year, "Повторять по годам"));
            SelectedRepeatRules = RepeatRules[0];


            if (editEvent == null)
            {
                _Event = new Event();

                Caption = "New Event";
                Start = date;
                Finish = date;
                IsAllDay = true;
                StartTime = DateTime.Now;
                FinishTime = DateTime.Now;
                if (_data.Calendars.Count > 0)
                    SelectedCalendar = _data.Calendars[0];
            }
            else
            {
                _Event = editEvent;
                SelectedCalendar = _Event.Calendar;
                Caption = _Event.Caption;
                SelectedRepeatRules.RuleType = _Event.RuleType;
                IsAllDay = _Event.IsAllDay;
                Start = _Event.Rule.Start;
                Finish =_Event.Rule.Finish;
                if (SelectedRepeatViewModel != null)
                {
                    SelectedRepeatViewModel.Step = _Event.Rule.Step;
                    SelectedRepeatViewModel.FinishRepeatDate = _Event.Rule.FinishRepeatDate;
                    if (_Event.RuleType == RuleRepeatTypes.Days)
                    {
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsDayStep = (_Event.Rule as RuleRepeatDay).IsDayStep;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsWorkDayStep = (_Event.Rule as RuleRepeatDay).IsWorkDayStep;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsRepeatDay = (_Event.Rule as RuleRepeatDay).IsRepeatDay;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsMonday = (_Event.Rule as RuleRepeatDay).IsMonday;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsTuesday = (_Event.Rule as RuleRepeatDay).IsTuesday;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsWednesday = (_Event.Rule as RuleRepeatDay).IsWednesday;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsThursday = (_Event.Rule as RuleRepeatDay).IsThursday;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsFriday = (_Event.Rule as RuleRepeatDay).IsFriday;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsSaturday = (_Event.Rule as RuleRepeatDay).IsSaturday;
                        (SelectedRepeatViewModel as DaysEditorViewModel).IsSunday = (_Event.Rule as RuleRepeatDay).IsSunday;   
                    }
                }
            }
        }
        private void OnAddEvent(object o)
        {
            _Event.Calendar = SelectedCalendar;
            _Event.Caption = Caption;
            _Event.RuleType = SelectedRepeatRules.RuleType;
            _Event.IsAllDay = IsAllDay;
            _Event.Rule.Start = new DateTime(Start.Year, Start.Month, Start.Day, StartTime.Hour, StartTime.Minute, 0);
            _Event.Rule.Finish = _Event.Rule.Start;// new DateTime(Finish.Year, Finish.Month, Finish.Day, FinishTime.Hour, FinishTime.Minute, 0);
            if(SelectedRepeatViewModel != null) 
            {
                _Event.Rule.Step = SelectedRepeatViewModel.Step;
                _Event.Rule.FinishRepeatDate = SelectedRepeatViewModel.FinishRepeatDate;
                if (_Event.RuleType == RuleRepeatTypes.Days)
                {
                    (_Event.Rule as RuleRepeatDay).IsDayStep = (SelectedRepeatViewModel as DaysEditorViewModel).IsDayStep;
                    (_Event.Rule as RuleRepeatDay).IsWorkDayStep = (SelectedRepeatViewModel as DaysEditorViewModel).IsWorkDayStep;
                    (_Event.Rule as RuleRepeatDay).IsRepeatDay = (SelectedRepeatViewModel as DaysEditorViewModel).IsRepeatDay;
                    (_Event.Rule as RuleRepeatDay).IsMonday = (SelectedRepeatViewModel as DaysEditorViewModel).IsMonday;
                    (_Event.Rule as RuleRepeatDay).IsTuesday = (SelectedRepeatViewModel as DaysEditorViewModel).IsTuesday;
                    (_Event.Rule as RuleRepeatDay).IsWednesday = (SelectedRepeatViewModel as DaysEditorViewModel).IsWednesday;
                    (_Event.Rule as RuleRepeatDay).IsThursday = (SelectedRepeatViewModel as DaysEditorViewModel).IsThursday;
                    (_Event.Rule as RuleRepeatDay).IsFriday = (SelectedRepeatViewModel as DaysEditorViewModel).IsFriday;
                    (_Event.Rule as RuleRepeatDay).IsSaturday = (SelectedRepeatViewModel as DaysEditorViewModel).IsSaturday;
                    (_Event.Rule as RuleRepeatDay).IsSunday = (SelectedRepeatViewModel as DaysEditorViewModel).IsSunday;
                }
            }
            if (!_data.Events.Contains(_Event))
                _data.Events.Add(_Event);
            else
                _data.Events.UpdateItem(_Event);// , new List<Event>() {_Event }
            CloseWindowCommand?.Execute(o);
        }
        
        public string Caption { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
        public Calendar SelectedCalendar { get; set; }

        public ICommand AddEventCommand { get => new ActionCommand(OnAddEvent); }
        public EditorViewModel SelectedRepeatViewModel { get => _viewModelsDictionary[SelectedRepeatRules.RuleType]; }

        public RepeatRuleViewModel SelectedRepeatRules
        {
            get => _selectedRepeatRules;
            set
            {
                SetField(ref _selectedRepeatRules, value);
                OnPropertyChanged(nameof(SelectedRepeatViewModel));
            }
        }
        public ObservableCollection<RepeatRuleViewModel> RepeatRules { get; set; }

    }
    public class EditorViewModel : BaseViewModel
    {
        public EditorViewModel()
        {
            Step = 1;
            FinishRepeatDate = DateTime.Now;
        }
        public int Step { get; set; }
        public DateTime FinishRepeatDate { get; set; }
        public virtual void Init(RuleRepeat rule)
        {
            var r = rule as RuleRepeat;
            if (r != null)
            {
                Step = r.Step;
                FinishRepeatDate = r.FinishRepeatDate;
            }
        }
    }
    public class DaysEditorViewModel : EditorViewModel
    {
        public DaysEditorViewModel(): base()
        {
            IsDayStep = true;
        }
        public bool IsDayStep { get; set; }
        public bool IsWorkDayStep { get; set; }
        public bool IsRepeatDay { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public bool IsSunday { get; set; }
        public override void Init(RuleRepeat rule)
        {
            var r = rule as RuleRepeatDay;
            if (r != null)
            {
                Step = r.Step;
                IsDayStep = r.IsDayStep;
                IsWorkDayStep = r.IsWorkDayStep;
                FinishRepeatDate = r.FinishRepeatDate;
                IsMonday = r.IsMonday;
                IsTuesday = r.IsTuesday;
                IsWednesday = r.IsWednesday;
                IsThursday = r.IsThursday;
                IsFriday = r.IsFriday;
                IsSaturday = r.IsSaturday;
                IsSunday = r.IsSunday;
            }
        }
    }
    public class WeekEditorViewModel : EditorViewModel
    { }
    public class MounthEditorViewModel : EditorViewModel
    { }
    public class YearEditorViewModel : EditorViewModel
    { }

}