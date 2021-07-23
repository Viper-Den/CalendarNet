using System;

namespace DestinyNet
{
    public class ViewModeDataBase : BaseViewModel
    {

        private DateTime _date;
        protected Data _data;

        public ViewModeDataBase(Data data)
        {
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
