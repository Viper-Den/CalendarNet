using System;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet.ViewModels
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
