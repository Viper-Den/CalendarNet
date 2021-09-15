using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace UIMonthControl
{
    [TemplatePart(Name = YearControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = YearControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = YearControl.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = YearControl.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    public class YearControl : Control
    {
   
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext";
        private Grid _MainGrid;
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private List<MonthControl> _Month;
       // private Dictionary<DayControl, List<IDateRange>> _dictionaryDateRanges;

        static YearControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(YearControl), new FrameworkPropertyMetadata(typeof(YearControl)));
        }
        public YearControl()
        {
            //_dictionaryDateRanges = new Dictionary<DayControl, List<IDateRange>>(); 
            _Month = new List<MonthControl>();
        }
        ~YearControl()
        {
            //if (DateRanges != null)
            //    DateRanges.CollectionChanged -= NotifyCollectionChangedEventHandler;
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
            _Title.MouseLeftButtonDown -= OnNow;
        }

        #region PeriodStart
        public static readonly DependencyProperty PeriodStartProperty =
            DependencyProperty.Register("PeriodStart", typeof(ICommand), typeof(YearControl), new PropertyMetadata(PeriodStartChanged));
        public static void PeriodStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).PeriodStart = (ICommand)e.NewValue;
        }
        public ICommand PeriodStart
        {
            get { return (ICommand)GetValue(PeriodStartProperty); }
            set { SetValue(PeriodStartProperty, value); }
        }
        #endregion

        #region PeriodFinish
        public static readonly DependencyProperty PeriodFinishProperty =
            DependencyProperty.Register("PeriodFinish", typeof(ICommand), typeof(YearControl), new PropertyMetadata(PeriodFinishPropertyChanged));
        public static void PeriodFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).PeriodFinish = (ICommand)e.NewValue;
        }
        public ICommand PeriodFinish
        {
            get { return (ICommand)GetValue(PeriodFinishProperty); }
            set { SetValue(PeriodFinishProperty, value); }
        }
        #endregion

        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(YearControl), new PropertyMetadata(DatePropertyChanged));
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, new DateTime(value.Year, 1, 1));
                if (_Title != null)
                    UpdateElements();
            }
        }
        #endregion

        #region DateRanges
        //public static readonly DependencyProperty DateRangesProperty =
        //   DependencyProperty.Register("DateRanges", typeof(ObservableCollection<IDateRange>), typeof(YearControl), new PropertyMetadata(OnDateRangesChanged));
        //private static void OnDateRangesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((YearControl)d).DateRanges = (ObservableCollection<IDateRange>)e.NewValue;
        //}
        //public ObservableCollection<IDateRange> DateRanges
        //{
        //    get { return (ObservableCollection<IDateRange>)GetValue(DateRangesProperty); }
        //    set
        //    {
        //        if (DateRanges != null)
        //            DateRanges.CollectionChanged -= NotifyCollectionChangedEventHandler;


        //        SetValue(DateRangesProperty, value);
        //        if (value != null)
        //        {
        //            DateRanges.CollectionChanged += NotifyCollectionChangedEventHandler;
        //            SelectRanges();
        //        }
        //    }
        //}
        #endregion

        private void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    break;
                case NotifyCollectionChangedAction.Remove:

                    break;

            }

            //NotifyCollectionChangedAction.Remove
            //NotifyCollectionChangedAction.Replace
            //NotifyCollectionChangedAction.Move
            //NotifyCollectionChangedAction.Reset
        }
        //public bool IsRangeInRange()
        //{
        //    if()
        //    return (DateTime.Compare) || ();
        //}
        private void SelectRanges()
        {
            //foreach (var dr in DateRanges)
            //{
            //    foreach(var m in _Month)
            //    {
            //        if(m.Date)
            //    }
            //}
        }
        private void UpdateElements()
        {
            _Title.Content = Date.ToString("yyyy");
            var startDay = Date;
            foreach (var m in _Month)
            {
                m.Date = startDay;
                startDay = startDay.AddMonths(1);
            }
        }
        private void OnPrevious(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MinValue) { return; }
            Date = Date.AddYears(-1);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MaxValue) { return; }
            Date = Date.AddYears(1);
        }
        private void OnNow(object sender, MouseButtonEventArgs e)
        {
            Date = DateTime.Now;
        }
        private void OnPeriodStart(DateTime date)
        {
            PeriodStart?.Execute(date);
        }
        private void OnPeriodFinish(DateTime date)
        {
            PeriodFinish?.Execute(date);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _MainGrid = (Grid)GetTemplateChild(TP_MAIN_GRID_PART);
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _Previous = (Label)GetTemplateChild(TP_PREVIOUS_PART);
            _Next = (Label)GetTemplateChild(TP_NEXT_PART);

            _Previous.Content = "<";
            _Next.Content = ">";
            _Previous.MouseLeftButtonDown += OnPrevious;
            _Next.MouseLeftButtonDown += OnNext;
            _Title.MouseLeftButtonDown += OnNow;

            for (int y = 1; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    var m = new MonthControl()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    Grid.SetColumn(m, x);
                    Grid.SetRow(m, y);
                    m.Margin = new Thickness(10, 10, 10, 10);
                    m.ViewButtons = Visibility.Hidden;
                    m.ViewBorderingMonths = Visibility.Hidden;
                    m.PeriodStart += OnPeriodStart;
                    m.PeriodFinish += OnPeriodFinish;
                    _MainGrid.Children.Add(m);
                    _Month.Add(m);
                }
            }
            UpdateElements();
        }

    }
}
