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
            AddEventCommand = new ActionCommand(OnAddEvent);
        }
        private void OnAddEvent(object o)
        {
            if (o is DateTime)
            {
                _dialogViewsManager.ShowDialogView(new EventEditorViewModel(_dialogViewsManager.ClosePopUpViewCommand, _data), true);
            }
        }
        public ObservableCollection<IEvent> Events {get => _data.Events; }
        public ICommand AddEventCommand { get; }
    }
}
