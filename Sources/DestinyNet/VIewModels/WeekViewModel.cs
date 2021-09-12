using MonthEvent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Destiny.Core;
using System.Windows.Input;

namespace DestinyNet
{
    public class WeekViewModel : ViewModeDataBase
    {
        public WeekViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
        }
        private void OnAddEvent(object o)
        {
            if (o is DateTime)
            {
                _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelEditWeek(_dialogViewsManager.ClosePopUpViewCommand, _data, ((DateTime)o)), true);
            }
        }
        private void DoSelectedEvent(object o)
        {
            if (o is Event)
            {
                _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelEdit(_dialogViewsManager.ClosePopUpViewCommand, _data, (Event)o), true);
            }
        }
        public ObservableCollection<Event> Events { get => _data.Events; }
        public ICommand AddEventCommand { get => new ActionCommand(OnAddEvent); }
        public ICommand SelectedEvent { get => new ActionCommand(DoSelectedEvent); }
    }
}

