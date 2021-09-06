using MonthEvent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Destiny.Core;

namespace DestinyNet
{
    public class WeekViewModel : ViewModeDataBase
    {
        public WeekViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager) { }
        public ObservableCollection<Event> Events { get => _data.Events; }
    }

}
