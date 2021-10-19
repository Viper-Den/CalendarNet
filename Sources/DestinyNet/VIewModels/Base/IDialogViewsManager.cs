using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet.ViewModels
{
    public interface IDialogViewsManager
    {
        public bool FreezeBackground { get;  }
        public void ShowDialogView(BaseViewModel dialogView, bool freezeBackground = false);
        public void ClosePopUpView(object o);
        public ICommand ClosePopUpViewCommand { get; }
    }
}
