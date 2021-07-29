using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DestinyNet
{
    public class EventEditorViewModel : DialogBaseViewMode
    {
        private readonly Data _data;
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
            RepeatType = RepeatTypes.Days;
        }
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
        public Calendar SelectedCalendar { get; set; }
        public IViewModel SelectedRepeatViewModel { get => _viewModelsDictionary[RepeatType]; }
        public RepeatTypes RepeatType { get; set; }

    }

    public class DaysEditorViewModel : BaseViewModel
    {
    }
    public class WeekEditorViewModel : BaseViewModel
    {
    }
    public class MounthEditorViewModel : BaseViewModel
    {
    }
    public class YearEditorViewModel : BaseViewModel
    {
    }

}