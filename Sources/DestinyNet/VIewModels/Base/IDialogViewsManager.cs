using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet
{
    public interface IDialogViewsManager
    {
        public bool FreezeBackground { get; set; }
        public void ShowDialogView(BaseViewModel dialogView, bool freezeBackground = false);
        public void ClosePopUpView(object o);
        public ICommand ClosePopUpViewCommand { get; }
    }
}
