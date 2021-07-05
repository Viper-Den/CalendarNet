using MonthEvent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DestinyNet
{
    public class WeekViewModel : ViewModeDateBase
    {
        public WeekViewModel(Data data) : base(data)
        {
            Events = new ObservableCollection<IEvent>();
            Update();
        }
        public ObservableCollection<IEvent> Events { get; }
        public void Update()
        {
            Events.Clear();
            foreach (var e in _data.Events)
            {
                Events.Add(e);
            }
            OnPropertyChanged(nameof(Events));
        }
    }

}
