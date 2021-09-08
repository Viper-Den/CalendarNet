using MonthEvent;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet
{
    public class MonthViewModel : ViewModeDataBase
    {
        public MonthViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
        }
        private void OnAddEvent(object o)
        {
            if (o is DateTime)
            {
                _dialogViewsManager.ShowDialogView(new EventEditorViewModel(_dialogViewsManager.ClosePopUpViewCommand, _data, ((DateTime)o)), true);
            }
        }
        private void DoSelectedEvent(object o)
        {
            if (o is Event)
            {
                _dialogViewsManager.ShowDialogView(new EventEditorViewModel(_dialogViewsManager.ClosePopUpViewCommand, _data, DateTime.Now, (Event)o), true);
            }
        }
        public ObservableCollection<Event> Events {get => _data.Events; }
        public ICommand AddEventCommand { get => new ActionCommand(OnAddEvent); }
        public ICommand SelectedEvent { get => new ActionCommand(DoSelectedEvent); }
    }
}
