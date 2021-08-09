using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Destiny.Core;

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

        public EventEditorViewModel(ICommand closeWindowCommand, Data data) : base(closeWindowCommand)
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
            AddEventCommand = new ActionCommand(OnAddEvent);
            Caption = "New Event";
            Start = DateTime.Now;
            Finish = DateTime.Now;
            StartTime = DateTime.Now;
            FinishTime = DateTime.Now;
        }
        private void OnAddEvent(object o)
        {
            var e = new Event();
            
            e.Calendar = SelectedCalendar;
            e.Caption = Caption;
            e.RuleType = SelectedRepeatRules.RuleType;
            e.Rule.Start = new DateTime(Start.Year, Start.Month, Start.Day, StartTime.Hour, StartTime.Minute, 0);
            e.Rule.Finish = new DateTime(Finish.Year, Finish.Month, Finish.Day, FinishTime.Hour, FinishTime.Minute, 0);
            if(SelectedRepeatViewModel != null) 
            { 
                e.Rule.Step = SelectedRepeatViewModel.Step;
                e.Rule.RepeatCount = SelectedRepeatViewModel.RepeatCount;
                if (e.RuleType == RuleRepeatTypes.Days)
                {
                    (e.Rule as RuleRepeatDay).IsDayStep = (SelectedRepeatViewModel as DaysEditorViewModel).IsDayStep;
                    (e.Rule as RuleRepeatDay).IsWorkDayStep = (SelectedRepeatViewModel as DaysEditorViewModel).IsWorkDayStep;
                    (e.Rule as RuleRepeatDay).IsRepeatDay = (SelectedRepeatViewModel as DaysEditorViewModel).IsRepeatDay;
                    (e.Rule as RuleRepeatDay).IsMonday = (SelectedRepeatViewModel as DaysEditorViewModel).IsMonday;
                    (e.Rule as RuleRepeatDay).IsTuesday = (SelectedRepeatViewModel as DaysEditorViewModel).IsTuesday;
                    (e.Rule as RuleRepeatDay).IsWednesday = (SelectedRepeatViewModel as DaysEditorViewModel).IsWednesday;
                    (e.Rule as RuleRepeatDay).IsThursday = (SelectedRepeatViewModel as DaysEditorViewModel).IsThursday;
                    (e.Rule as RuleRepeatDay).IsFriday = (SelectedRepeatViewModel as DaysEditorViewModel).IsFriday;
                    (e.Rule as RuleRepeatDay).IsSaturday = (SelectedRepeatViewModel as DaysEditorViewModel).IsSaturday;
                    (e.Rule as RuleRepeatDay).IsSunday = (SelectedRepeatViewModel as DaysEditorViewModel).IsSunday;
                }
            }
            _data.Events.Add(e);
            CloseWindowCommand?.Execute(o);
        }

        public string Caption { get; set; }

        private DateTime _startDate;
        public DateTime Start 
        { 
            get => _startDate; 
            set {  _startDate = value; } 
        }
        public DateTime Finish { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
        public Calendar SelectedCalendar { get; set; }

        public ICommand AddEventCommand { get; }
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
        public int Step { get; set; }
        public int RepeatCount { get; set; }
        public virtual void Init(RuleRepeat rule)
        {
            var r = rule as RuleRepeat;
            if (r != null)
            {
                Step = r.Step;
                RepeatCount = r.RepeatCount;
            }
        }
    }
    public class DaysEditorViewModel : EditorViewModel
    {
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
                RepeatCount = r.RepeatCount;
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