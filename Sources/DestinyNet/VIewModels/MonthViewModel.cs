using MonthEvent;
using System.Collections.ObjectModel;

namespace DestinyNet
{
    public class MonthViewModel : ViewModeDataBase
    {
        public MonthViewModel(Data data) : base(data)
        {
        }
        public ObservableCollection<Event> Events {get => _data.Events; }
    }
}
