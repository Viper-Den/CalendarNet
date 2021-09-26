using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Destiny.Core;
using System.Collections.Specialized;

namespace DestinyNet
{
    public enum EventEditorMode
    {
        Edit,
        New
    }
    public class EventEditorViewModel : DialogBaseViewMode
    {
        private readonly Data _data;
        private RepeatRuleViewModel _selectedRepeatRules;
        private Dictionary<RuleRepeatTypes, EditorViewModel> _viewModelsDictionary;
        private Dictionary<RuleRepeatTypes, RepeatRuleViewModel> _repeatRuleViewModelDictionary;
        private Event _Event;
        private EventEditorMode _mode;

        public static EventEditorViewModel EventEditorViewModelAddCollectionDays(ICommand closeWindowCommand, Data data, ObservableCollection<DateTime> dates)
        {
            var ev = new Event();
            ev.Caption = "New Event";
            ev.IsAllDay = true;
            ev.RuleType = RuleRepeatTypes.SpecialDays;
            ev.Rule.Start = dates[0];
            ev.Rule.Finish = ev.Rule.Start.AddHours(1);
            ((RuleRepeatSpecialDays)ev.Rule).SpecialDays = new List<DateTime>(dates);
            return new EventEditorViewModel(closeWindowCommand, data, ev, EventEditorMode.New);
        }
        public static EventEditorViewModel EventEditorViewModelEditeCollectionDays(ICommand closeWindowCommand, Data data, ObservableCollection<DateTime> dates, Event ev)
        {
            if (ev == null) 
                return null;
            var e = new EventEditorViewModel(closeWindowCommand, data, ev, EventEditorMode.Edit);
            e.RuleRepeatTypes = RuleRepeatTypes.SpecialDays;
            ((SpecialDaysEditorViewModel)e.SelectedRepeatViewModel).SpecialDays = new ObservableCollection<DateTime>(dates);
            return e;
        }
        public static EventEditorViewModel EventEditorViewModelNewAllDay(ICommand closeWindowCommand, Data data, DateTime date)
        {
            var ev = new Event();
            ev.Caption = "New Event";
            ev.IsAllDay = true;
            ev.RuleType = RuleRepeatTypes.SpecialDays;
            ev.Rule.Start = date;
            ev.Rule.Finish = date.AddHours(1);
            return new EventEditorViewModel(closeWindowCommand, data, ev, EventEditorMode.New);
        }
        public static EventEditorViewModel EventEditorViewModelEditWeek(ICommand closeWindowCommand, Data data, DateTime date)
        {
            var ev = new Event();
            ev.Caption = "New Event";
            ev.IsAllDay = false;
            ev.RuleType = RuleRepeatTypes.SpecialDays;
            ev.Rule.Start = date;
            ev.Rule.Finish = date.AddHours(1);
            return new EventEditorViewModel(closeWindowCommand, data, ev, EventEditorMode.Edit);
        }
        public static EventEditorViewModel EventEditorViewModelEdit(ICommand closeWindowCommand, Data data, Event editEvent)
        {
            return new EventEditorViewModel(closeWindowCommand, data, editEvent, EventEditorMode.Edit);
        }
        public EventEditorViewModel(ICommand closeWindowCommand, Data data, Event editEvent, EventEditorMode mode) : base(closeWindowCommand)
        {
            _data = data;
            _Event = editEvent;
            _mode = mode;

            if (_data.Calendars.Count == 0)
                return;

            _viewModelsDictionary = new Dictionary<RuleRepeatTypes, EditorViewModel>();
            _viewModelsDictionary.Add(RuleRepeatTypes.None, null);
            _viewModelsDictionary.Add(RuleRepeatTypes.Days, new DaysEditorViewModel());
            _viewModelsDictionary.Add(RuleRepeatTypes.Week, new WeekEditorViewModel());
            _viewModelsDictionary.Add(RuleRepeatTypes.Mounth, new MounthEditorViewModel());
            _viewModelsDictionary.Add(RuleRepeatTypes.Year, new YearEditorViewModel());
            _viewModelsDictionary.Add(RuleRepeatTypes.SpecialDays, new SpecialDaysEditorViewModel());

            _repeatRuleViewModelDictionary = new Dictionary<RuleRepeatTypes, RepeatRuleViewModel>();
            _repeatRuleViewModelDictionary.Add(RuleRepeatTypes.None, new RepeatRuleViewModel(RuleRepeatTypes.None, ""));
            _repeatRuleViewModelDictionary.Add(RuleRepeatTypes.Days, new RepeatRuleViewModel(RuleRepeatTypes.Days, "Повторять по дням"));
            _repeatRuleViewModelDictionary.Add(RuleRepeatTypes.Week, new RepeatRuleViewModel(RuleRepeatTypes.Week, "Повторять по неделям"));
            _repeatRuleViewModelDictionary.Add(RuleRepeatTypes.Mounth, new RepeatRuleViewModel(RuleRepeatTypes.Mounth, "Повторять по месяцам"));
            _repeatRuleViewModelDictionary.Add(RuleRepeatTypes.Year, new RepeatRuleViewModel(RuleRepeatTypes.Year, "Повторять по годам"));
            _repeatRuleViewModelDictionary.Add(RuleRepeatTypes.SpecialDays, new RepeatRuleViewModel(RuleRepeatTypes.SpecialDays, "Особые дни"));
            RepeatRules  = new ObservableCollection<RepeatRuleViewModel>(_repeatRuleViewModelDictionary.Values);



                SelectedCalendar = _Event.Calendar;
            if ((SelectedCalendar == null) && (_data.Calendars.Count > 0))
                SelectedCalendar = _data.Calendars[0];
            Caption = _Event.Caption;
            SelectedRepeatRules = _repeatRuleViewModelDictionary[_Event.RuleType];
            IsAllDay = _Event.IsAllDay;
            Start = _Event.Rule.Start;
            Finish = _Event.Rule.Finish;
            StartTime = _Event.Rule.Start;
            FinishTime = _Event.Rule.Finish;
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
                if(_Event.RuleType == RuleRepeatTypes.SpecialDays)
                    (SelectedRepeatViewModel as SpecialDaysEditorViewModel).SpecialDays = new ObservableCollection<DateTime>((_Event.Rule as RuleRepeatSpecialDays).SpecialDays);
            }
        }
        private void DoAddEvent(object o)
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
            if (_Event.RuleType == RuleRepeatTypes.SpecialDays)
                (_Event.Rule as RuleRepeatSpecialDays).SpecialDays = new List<DateTime>((SelectedRepeatViewModel as SpecialDaysEditorViewModel).SpecialDays);

            if (!_data.Events.Contains(_Event))
                _data.Events.Add(_Event);
            else
                _data.Events.UpdateItem(_Event);// , new List<Event>() {_Event }
            CloseWindowCommand?.Execute(null);
        }
        public RuleRepeatTypes RuleRepeatTypes 
        { 
            get => SelectedRepeatRules.RuleType; 
            set { SelectedRepeatRules = _repeatRuleViewModelDictionary[value]; } 
        }
        public string Caption { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
        public Calendar SelectedCalendar { get; set; }
        public bool IsDeletebel { get => (_mode == EventEditorMode.Edit); }
        
        public ICommand AddCommand { get => new ActionCommand(DoAddEvent); }

        public ICommand AddDeleteCommand { get => new ActionCommand(DoDeleteEvent); }

        private void DoDeleteEvent(object o)
        {
            if (_data.Events.Contains(_Event))
                _data.Events.Remove(_Event);
            CloseWindowCommand?.Execute(null);
        }

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
        public ObservableCollection<RepeatRuleViewModel> RepeatRules { private set; get; }

    }
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
    public class SpecialDaysEditorViewModel : EditorViewModel
    {
        public SpecialDaysEditorViewModel()
        {
            SpecialDays = new ObservableCollection<DateTime>();
        }
        public ObservableCollection<DateTime> SpecialDays { get; set; }
    }

}