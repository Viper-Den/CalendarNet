using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using UIMonthControl;

namespace DestinyNet
{
    public class YearViewModel : ViewModeDateBase
    {
        private DateTime _startDate;
        public YearViewModel(Data data) : base(data)
        {
            DateRanges = new ObservableCollection<IDateRange>();
            StartDate = new ActionCommand(o => { _startDate = (DateTime)o; });
            FinishDate = new ActionCommand(o => { DateRanges.Add(new DateRange() { Start = _startDate, Finish = (DateTime)o }); }) ;
            Update();
        }
        public void Update()
        {
            DateRanges.Clear();
            foreach (var r in _data.DateRanges)
            {
                DateRanges.Add(r);
            }
            OnPropertyChanged(nameof(DateRanges));
        }
        public ObservableCollection<IDateRange> DateRanges { get; }
        public ICommand StartDate { get; }
        public ICommand FinishDate { get; }

    }
}
