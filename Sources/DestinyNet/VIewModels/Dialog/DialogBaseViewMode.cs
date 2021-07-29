using System;
using System.Windows.Input;

namespace DestinyNet
{
    public class DialogBaseViewMode : BaseViewModel
    {
        public ICommand CloseWindowCommand { get; }
        public DialogBaseViewMode(ICommand closeWindowCommand)
        {
            CloseWindowCommand = closeWindowCommand;
        }
    }
}
