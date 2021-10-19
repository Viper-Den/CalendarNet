using Destiny.Core;
using System.Windows.Input;

namespace DestinyNet.ViewModels
{
        public class DialogViewsManager : BaseViewModel, IDialogViewsManager
        {
            private BaseViewModel _currentDialogView;
            private bool _freezeBackGround;

            public ICommand ClosePopUpViewCommand {  get  => new ActionCommand(ClosePopUpView); }
            public BaseViewModel CurrentDialogView
            {
                get => _currentDialogView;
                private set => SetField(ref _currentDialogView, value);
            }

            public bool FreezeBackground
            {
                get => _freezeBackGround;
                set{ SetField(ref _freezeBackGround, value); }
            }

            public void ShowDialogView(BaseViewModel dialogView, bool freezeBackground = false)
            {
                CurrentDialogView = dialogView;
                FreezeBackground = freezeBackground;
            }

            public void ClosePopUpView(object o)
            {
                CurrentDialogView = null;
                FreezeBackground = false;
            }
        }
}
