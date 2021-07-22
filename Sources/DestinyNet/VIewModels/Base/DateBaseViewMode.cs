using System;

namespace DestinyNet
{
    public class ViewModeDateBase : BaseViewModel
    {

        private DateTime _date;
        protected Data _data;

        public ViewModeDateBase(Data data)
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
