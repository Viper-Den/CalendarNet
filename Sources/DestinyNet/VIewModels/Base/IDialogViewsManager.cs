
namespace DestinyNet
{
    public interface IDialogViewsManager
    {
        public bool FreezeBackground { get; set; }
        public void ShowDialogView(ViewModelBase dialogView, bool freezeBackground = false);
        public void ClosePopUpView();

    }
}
