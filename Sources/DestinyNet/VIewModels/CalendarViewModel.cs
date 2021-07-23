﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace DestinyNet
{
    public class CalendarViewModel : BaseViewModel
    {
        private readonly ICalendarsEditor _calendarEditor;

        public ICommand DeleteCalendarCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public ICalendar SelectedCalendar { get => _calendarEditor.SelectedCalendar; }
        public CalendarViewModel(ICalendarsEditor calendarEditor, ICommand closeWindowCommand)
        {
            _calendarEditor = calendarEditor;
            DeleteCalendarCommand = new ActionCommand(DelateCalendar);
            CloseWindowCommand = closeWindowCommand;
        }
        private void DelateCalendar(object o)
        {
            if (_calendarEditor.SelectedCalendar == null)
                return;
            var c = _calendarEditor.SelectedCalendar;
            _calendarEditor.SelectedCalendar = null;
            _calendarEditor.Calendars.Remove(c);
            CloseWindowCommand?.Execute(null);
        }

    }

}
