namespace DestinyNet
{

        public class DialogViewsManager : ViewModelBase, IDialogViewsManager
    {
            private ViewModelBase _currentDialogView;
            private bool _freezeBackGround;

            //public ICommand ClosePopUpViewCommand => new ActionCommand(ClosePopUpView);

            public ViewModelBase CurrentDialogView
            {
                get => _currentDialogView;
                private set => SetField(ref _currentDialogView, value);
            }

            public bool FreezeBackground
            {
                get => _freezeBackGround;
                set => SetField(ref _freezeBackGround, value);
            }

            public void ShowDialogView(ViewModelBase dialogView, bool freezeBackground = false)
            {
                CurrentDialogView = dialogView;
                FreezeBackground = freezeBackground;
            }

            public void ClosePopUpView()
            {
                CurrentDialogView = null;
                FreezeBackground = false;
            }
        }
}
