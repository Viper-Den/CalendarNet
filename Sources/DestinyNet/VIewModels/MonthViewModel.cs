using MonthEvent;
using System.Collections.ObjectModel;

namespace DestinyNet
{
    public class MonthViewModel : ViewModeDateBase
    {
        public MonthViewModel(Data data) : base(data)
        {
            Events = new ObservableCollection<IEvent>();
            Update();
        }
        public ObservableCollection<IEvent> Events {get; }
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
