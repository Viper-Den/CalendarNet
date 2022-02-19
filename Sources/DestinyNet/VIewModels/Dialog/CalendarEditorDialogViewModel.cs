using System;
using System.Windows.Input;
using System.Windows.Media;
using Destiny.Core;

namespace DestinyNet.ViewModels
{
    public class CalendarEditorDialogViewModel : BaseViewModel
    {
        private readonly ICalendarsEditor _calendarEditor;
        private readonly ICommand _closeWindowCommand;
        private string _name;
        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        public string Name { 
            get => _name; 
            set { SetField(ref _name, value, nameof(Name)); } 
        }
        public SolidColorBrush Background { 
            get => _background; 
            set { SetField(ref _background, value, nameof(Background)); } 
        }
        public SolidColorBrush Foreground { 
            get => _foreground; 
            set { SetField(ref _foreground, value, nameof(Foreground)); } 
        }
        public ICommand DeleteCalendarCommand { get; }
        public ICommand CancelCalendarChangesCommand { get => _closeWindowCommand; }
        public ICommand SaveCalendarCommand { get; }
        public CalendarEditorDialogViewModel(ICalendarsEditor calendarEditor, ICommand closeWindowCommand)
        {
            if (calendarEditor.SelectedCalendar == null)
                throw new Exception("you should select calendar");
            _calendarEditor = calendarEditor;
            _closeWindowCommand = closeWindowCommand;
            SaveCalendarCommand = new ActionCommand(DoSaveCalendar); ;
            DeleteCalendarCommand = new ActionCommand(DoDelateCalendar);

            Name = _calendarEditor.SelectedCalendar.Name;
            Background = _calendarEditor.SelectedCalendar.Background;
            Foreground = _calendarEditor.SelectedCalendar.Foreground;

            if (Background == null)
                Background = new SolidColorBrush();
            if (Foreground == null)
                Foreground = new SolidColorBrush();
        }
        private void DoDelateCalendar(object o)
        {
            if (_calendarEditor.SelectedCalendar == null)
                return;
            var c = _calendarEditor.SelectedCalendar;
            _calendarEditor.SelectedCalendar = null;
            _calendarEditor.Calendars.Remove(c);
            _closeWindowCommand?.Execute(null);
        }
        private void DoSaveCalendar(object o)
        {
            if (_calendarEditor.SelectedCalendar == null)
                return;
            _calendarEditor.SelectedCalendar.Name = Name;
            _calendarEditor.SelectedCalendar.Background = Background;
            _calendarEditor.SelectedCalendar.Foreground = Foreground;
            _closeWindowCommand?.Execute(null);
        }

    }

}
