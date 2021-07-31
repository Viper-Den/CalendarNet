using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DestinyNet
{
    public class RepeatRule
    {
        public RepeatRule(RepeatTypes repeatType, string name)
        {
            RepeatType = repeatType;
            Name = name;
        }
        public RepeatTypes RepeatType { get; set; }
        public string Name { get; set; }
    }

    public class EventEditorViewModel : DialogBaseViewMode
    {
        private readonly Data _data;
        private RepeatRule _selectedRepeatRules;
        private Dictionary<RepeatTypes, IViewModel> _viewModelsDictionary;

        public EventEditorViewModel(ICommand closeWindowCommand, Data data) : base(closeWindowCommand)
        {
            _data = data;
            _viewModelsDictionary = new Dictionary<RepeatTypes, IViewModel>();
            _viewModelsDictionary.Add(RepeatTypes.None, null);
            _viewModelsDictionary.Add(RepeatTypes.Days, new DaysEditorViewModel());
            _viewModelsDictionary.Add(RepeatTypes.Week, new WeekEditorViewModel());
            _viewModelsDictionary.Add(RepeatTypes.Mounth, new MounthEditorViewModel());
            _viewModelsDictionary.Add(RepeatTypes.Year, new YearEditorViewModel());

            RepeatRules = new ObservableCollection<RepeatRule>();
            RepeatRules.Add(new RepeatRule(RepeatTypes.None, ""));
            RepeatRules.Add(new RepeatRule(RepeatTypes.Days, "Повторять по дням"));
            RepeatRules.Add(new RepeatRule(RepeatTypes.Week, "Повторять по неделям"));
            RepeatRules.Add(new RepeatRule(RepeatTypes.Mounth, "Повторять по месяцам"));
            RepeatRules.Add(new RepeatRule(RepeatTypes.Year, "Повторять по годам"));
            SelectedRepeatRules = RepeatRules[0];
        }
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
        public Calendar SelectedCalendar { get; set; }

        public IViewModel SelectedRepeatViewModel { get => _viewModelsDictionary[SelectedRepeatRules.RepeatType]; }

        public RepeatRule SelectedRepeatRules
        {
            get => _selectedRepeatRules;
            set
            {
                SetField(ref _selectedRepeatRules, value);
                OnPropertyChanged(nameof(SelectedRepeatViewModel));
            }
        }
        public ObservableCollection<RepeatRule> RepeatRules { get; set; }

    }

    public class DaysEditorViewModel : BaseViewModel
    {
        public int Step { get; set; }
        public bool IsDayStep { get; set; }
        public bool IsWorkDayStep { get; set; }
        public int RepeatCount { get; set; }
    }
    public class WeekEditorViewModel : BaseViewModel
    {
        public int Step { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public bool IsSunday { get; set; }
        public int RepeatCount { get; set; }
    }
    public class MounthEditorViewModel : BaseViewModel
    {
        public int Step { get; set; }
        public int RepeatCount { get; set; }
    }
    public class YearEditorViewModel : BaseViewModel
    {
        public int Step { get; set; }
        public int RepeatCount { get; set; }
    }

}