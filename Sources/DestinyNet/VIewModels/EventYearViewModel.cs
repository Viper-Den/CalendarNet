﻿using Destiny.Core;
using System.Windows.Media;

namespace DestinyNet
{
    public class EventYearViewModel: BaseViewModel
    {
        private Event _event;
        public EventYearViewModel(Event ev)
        {
            _event = ev;
        }
        public SolidColorBrush BackgroundColor
        {
            get => new SolidColorBrush(Color.FromArgb(200, _event.Calendar.Color.Color.R, _event.Calendar.Color.Color.G, _event.Calendar.Color.Color.B));
        }   
    }
}
