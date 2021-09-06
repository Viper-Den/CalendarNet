using System;
using Destiny.Core;

namespace DestinyNet
{
    public class ViewModeDataBase : BaseViewModel
    {

        private DateTime _date;
        protected Data _data;
        protected IDialogViewsManager _dialogViewsManager;

        public ViewModeDataBase(Data data, IDialogViewsManager dialogViewsManager)
        {
            _dialogViewsManager = dialogViewsManager;
            _date = DateTime.Now;
            _data = data;
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
    }
}
