using Destiny.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DestinyNet.ViewModels
{
    public class PeopleViewModel: ViewModeDataBase
    {
        public PeopleViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
        }
        public ObservableCollection<Person> People { get => _data.People; }
    }
}
