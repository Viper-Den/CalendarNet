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


        public static EventEditorViewModel EventEditorViewModelEdit(ICommand closeWindowCommand, Data data, Event editEvent) 
        {
            var e = new EventEditorViewModel(closeWindowCommand, data, editEvent);
            e.SelectedCalendar = editEvent.Calendar;
            e.Caption = editEvent.Caption;
            e.SelectedRepeatRules.RuleType = editEvent.RuleType;
            e.IsAllDay = editEvent.IsAllDay;
            e.Start = editEvent.Rule.Start;
            e.Finish = editEvent.Rule.Finish;
            e.StartTime = editEvent.Rule.Start;
            e.FinishTime = editEvent.Rule.Finish;
            if (e.SelectedRepeatViewModel != null)
            {
                e.SelectedRepeatViewModel.Step = editEvent.Rule.Step;
                e.SelectedRepeatViewModel.FinishRepeatDate = editEvent.Rule.FinishRepeatDate;
                if (editEvent.RuleType == RuleRepeatTypes.Days)
                {
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsDayStep = (editEvent.Rule as RuleRepeatDay).IsDayStep;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsWorkDayStep = (editEvent.Rule as RuleRepeatDay).IsWorkDayStep;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsRepeatDay = (editEvent.Rule as RuleRepeatDay).IsRepeatDay;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsMonday = (editEvent.Rule as RuleRepeatDay).IsMonday;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsTuesday = (editEvent.Rule as RuleRepeatDay).IsTuesday;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsWednesday = (editEvent.Rule as RuleRepeatDay).IsWednesday;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsThursday = (editEvent.Rule as RuleRepeatDay).IsThursday;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsFriday = (editEvent.Rule as RuleRepeatDay).IsFriday;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsSaturday = (editEvent.Rule as RuleRepeatDay).IsSaturday;
                    (e.SelectedRepeatViewModel as DaysEditorViewModel).IsSunday = (editEvent.Rule as RuleRepeatDay).IsSunday;
                }
            }
            return e;
        }
        public static EventEditorViewModel EventEditorViewModelNewAllDay(ICommand closeWindowCommand, Data data, DateTime date) 
        {
            var e = new EventEditorViewModel(closeWindowCommand, data, new Event());

            e.Caption = "New Event";
            e.IsAllDay = true;
            e.Start = date;
            e.Finish = date;
            e.StartTime = e.Start;
            e.FinishTime = e.Start.AddHours(1);
            return e;
        }
        public static EventEditorViewModel EventEditorViewModelEditWeek(ICommand closeWindowCommand, Data data, DateTime start)
        {
            var e = new EventEditorViewModel(closeWindowCommand, data, new Event());

            e.Caption = "New Event";
            e.IsAllDay = false;
            e.Start = start;
            e.Finish = start;
            e.StartTime = start;
            e.FinishTime = start.AddHours(1);
            return e;
        }
        public EventEditorViewModel(ICommand closeWindowCommand, Data data, Event editEvent) : base(closeWindowCommand)
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


            _Event = editEvent;
            if (_data.Calendars.Count > 0)
                SelectedCalendar = _data.Calendars[0];
        }
        private void OnAddEvent(object o)
        {
            _Event.Calendar = SelectedCalendar;
            _Event.Caption = Caption;
            _Event.RuleType = SelectedRepeatRules.RuleType;
            _Event.IsAllDay = IsAllDay;
            _Event.Rule.Start = new DateTime(Start.Year, Start.Month, Start.Day, StartTime.Hour, StartTime.Minute, 0);
            _Event.Rule.Finish = new DateTime(Start.Year, Start.Month, Start.Day, FinishTime.Hour, FinishTime.Minute, 0);
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