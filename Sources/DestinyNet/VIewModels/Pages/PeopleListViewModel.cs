using Destiny.Core;
using DestinyNet.ViewModels.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace DestinyNet.ViewModels
{
    public class PeopleListViewModel: ViewModeDataBase
    {
        public PeopleListViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
        }
        public ObservableCollection<PersonViewModel> People { get => _data.People; }
        public ICommand EditPersonCommand { get => new ActionCommand(DoEditPersonCommand); }
        public ICommand AddPersonCommand { get => new ActionCommand(DoAddPersonCommand); }
        private void DoEditPersonCommand(object obj)
        {
            if(obj is PersonViewModel p)
                _dialogViewsManager.ShowDialogView(PersonRedactorViewModel.EditPerson(_dialogViewsManager.ClosePopUpViewCommand, p, People), true);
        }
        private void DoAddPersonCommand(object obj)
        {
            _dialogViewsManager.ShowDialogView(PersonRedactorViewModel.AddPerson(_dialogViewsManager.ClosePopUpViewCommand, People), true);
        }
    }
}
