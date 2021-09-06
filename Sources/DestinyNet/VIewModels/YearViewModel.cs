using System;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet
{
    public class YearViewModel : ViewModeDataBase
    {
        private DateTime _startDate;
        public YearViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
        }
        public void OnStartDate(object o)
        {
            if(o is DateTime)
                _startDate = (DateTime)o;
        }
        public void OnFinishDate(object o)
        {
            if (o is DateTime)
            {
                //DateRanges.Add(new DateRange() { Finish = (DateTime)o, Start = _startDate, Calendar = _data.Calendars[0] });
            }
        }
        public ICommand StartDateCommand { get => new ActionCommand(OnStartDate); }
        public ICommand FinishDateCommand { get => new ActionCommand(OnFinishDate); }

    }
}
