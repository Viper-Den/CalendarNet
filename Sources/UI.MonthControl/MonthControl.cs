using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace UIMonthControl
{
    //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.controls.itemscontrol?view=netcore-3.1
    [TemplatePart(Name = MonthControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class MonthControl : Control
    {
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext"; 
        private Grid _MainGrid;
        private TitleControl _Title;
        private TitleControl _Previous;
        private TitleControl _Next;
        private List<DayControl> _Days;
        private List<TitleControl> _TitleDays;


        public List<DayControl> Days { get => _Days; }
        ~MonthControl()
        {
            if(_Previous != null)
                _Previous.MouseLeftButtonDown -= OnPrevious;
            if (_Next != null)
                _Next.MouseLeftButtonDown -= OnNext;
            if (_Title != null)
                _Title.MouseLeftButtonDown -= OnNow;
        }
        public MonthControl()
        {
            _Days = new List<DayControl>();
            _TitleDays = new List<TitleControl>();
            ColorDayFinish = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorDayOff = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorDayOffFinish = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            ColorToDay = new SolidColorBrush(Color.FromRgb(17, 110, 190));
        }
        static MonthControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthControl), new FrameworkPropertyMetadata(typeof(MonthControl)));
        }

        #region DateProperty
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MonthControl),
                new PropertyMetadata(DatePropertyChanged));
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, new DateTime(value.Year, value.Month, 1));
                UpdateElements();
            }
        }
        #endregion
        #region DateRanges
        //public static readonly DependencyProperty DateRangesProperty =
        //   DependencyProperty.Register("DateRanges", typeof(ObservableCollection<IDateRange>), 
        //       typeof(MonthControl), new PropertyMetadata(OnDateRangesChanged));
        //private static void OnDateRangesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((MonthControl)d).DateRanges = (ObservableCollection<IDateRange>)e.NewValue;
        //}        public ObservableCollection<IDateRange> DateRanges
        //{
        //    get { return (ObservableCollection<IDateRange>)GetValue(DateRangesProperty); }
        //    set
        //    {
        //        SetValue(DateRangesProperty, value);
        //        if (value != null)
        //        {
        //            SelectRanges();
        //        }
        //    }
        //}
        #endregion
        #region ColorDayOff
        public static readonly DependencyProperty ColorDayOffProperty =
            DependencyProperty.Register("ColorDayOff", typeof(SolidColorBrush), typeof(MonthControl), new PropertyMetadata(ColorDayOffPropertyChanged));
        public static void ColorDayOffPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorDayOff = (SolidColorBrush)e.NewValue;
        }
        public SolidColorBrush ColorDayOff
        {
            get { return (SolidColorBrush)GetValue(ColorDayOffProperty); }
            set
            {
                SetValue(ColorDayOffProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region ColorDayFinish
        public static readonly DependencyProperty ColorDayFinishProperty =
            DependencyProperty.Register("ColorDayFinish", typeof(SolidColorBrush), typeof(MonthControl), new PropertyMetadata(ColorDayFinishPropertyChanged));
        private static void ColorDayFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorDayFinish = (SolidColorBrush)e.NewValue;
        }
        public SolidColorBrush ColorDayFinish
        {
            get { return (SolidColorBrush)GetValue(ColorDayFinishProperty); }
            set
            {
                SetValue(ColorDayFinishProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region ColorDayOffFinish
        public static readonly DependencyProperty ColorDayOffFinishProperty =
            DependencyProperty.Register("ColorDayOffFinish", typeof(SolidColorBrush), typeof(MonthControl), new PropertyMetadata(ColorDayOffFinishPropertyChanged));
        private static void ColorDayOffFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorDayOffFinish = (SolidColorBrush)e.NewValue;
        }
        public SolidColorBrush ColorDayOffFinish
        {
            get { return (SolidColorBrush)GetValue(ColorDayOffFinishProperty); }
            set
            {
                SetValue(ColorDayOffFinishProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region ColorToDay
        public static readonly DependencyProperty ColorToDayProperty =
            DependencyProperty.Register("ColorToDay", typeof(SolidColorBrush), typeof(MonthControl), new PropertyMetadata(ColorToDayPropertyChanged));
        private static void ColorToDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorToDay = (SolidColorBrush)e.NewValue;
        }
        public SolidColorBrush ColorToDay
        {
            get { return (SolidColorBrush)GetValue(ColorToDayProperty); }
            set
            {
                SetValue(ColorToDayProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region ViewBorderingMonths
        public static readonly DependencyProperty ViewBorderingMonthsProperty =
            DependencyProperty.Register("ViewBorderingMonths", typeof(Visibility), typeof(MonthControl), new PropertyMetadata(ViewBorderingMonthsPropertyChanged));
        public static void ViewBorderingMonthsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ViewBorderingMonths = (Visibility)e.NewValue;
        }
        public Visibility ViewBorderingMonths
        {
            get { return (Visibility)GetValue(ViewBorderingMonthsProperty); }
            set
            {
                SetValue(ViewBorderingMonthsProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region ViewButtons
        public static readonly DependencyProperty ViewButtonsProperty =
            DependencyProperty.Register("ViewButtons", typeof(Visibility), typeof(MonthControl), new PropertyMetadata(ViewButtonsPropertyChanged));
        public static void ViewButtonsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ViewButtons = (Visibility)e.NewValue;
        }
        public Visibility ViewButtons
        {
            get { return (Visibility)GetValue(ViewButtonsProperty); }
            set
            {
                SetValue(ViewButtonsProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region ViewSelectedDate
        public static readonly DependencyProperty ViewSelectedDateProperty =
            DependencyProperty.Register("ViewSelectedDate", typeof(bool), typeof(MonthControl), new PropertyMetadata(ViewSelectedDatePropertyChanged));
        public static void ViewSelectedDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ViewSelectedDate = (bool)e.NewValue;
        }
        public bool ViewSelectedDate
        {
            get { return (bool)GetValue(ViewSelectedDateProperty); }
            set
            {
                SetValue(ViewSelectedDateProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region ColorViewSelectedDate
        public static readonly DependencyProperty ColorViewSelectedDateProperty =
            DependencyProperty.Register("ColorViewSelectedDate", typeof(SolidColorBrush), typeof(MonthControl), new PropertyMetadata(ColorViewSelectedDatePropertyChanged));
        private static void ColorViewSelectedDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorViewSelectedDate = (SolidColorBrush)e.NewValue;
        }
        public SolidColorBrush ColorViewSelectedDate
        {
            get { return (SolidColorBrush)GetValue(ColorViewSelectedDateProperty); }
            set
            {
                SetValue(ColorViewSelectedDateProperty, value);
                UpdateElements();
            }
        }
        #endregion
        private void SelectRanges()
        {
            //foreach (var r in DateRanges)
            //{
            //    foreach (var d in _Days)
            //    {
            //        if ((d.Date >= r.Start) && (d.Date <= r.Finish))
            //        {
            //            d.Background = Brushes.Green;
            //        }
            //    }
            //}
        }
        private void UpdateElements()
        {
            if (_Title == null) 
                return; 
            var dayOfWeek = (int)Date.DayOfWeek;
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (dayOfWeek == 0) 
                dayOfWeek = 6;  // change to DateTimeFormat
            else 
                dayOfWeek--; 


            _Previous.Visibility = ViewButtons;
            _Next.Visibility = ViewButtons;
            if (ViewButtons == Visibility.Visible)
            {
                Grid.SetColumnSpan(_Title, 5);
                _Title.Text = Date.ToString("MMMM yyyy");
            }
            else
            {
                Grid.SetColumnSpan(_Title, 7);
                _Title.Text = Date.ToString("MMMM");
            }

            var startDay = Date.AddDays(-dayOfWeek);
            foreach (var d in _TitleDays)
            {
                d.Text = startDay.ToString("ddd");
                d.FontSize = 10;
                d.Visibility = Visibility.Visible;
                switch (startDay.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        d.Background = ColorDayOff;
                        break;
                    case DayOfWeek.Sunday:
                        d.Background = ColorDayOff;
                        break;
                    default:
                        d.Background = Background;
                        break;
                }
                startDay = startDay.AddDays(1);
            }
            startDay = Date.AddDays(-dayOfWeek);
            foreach (var d in _Days)
            {
                d.Date = startDay;
                d.ViewSelectedDate = ViewSelectedDate;
                d.ColorViewSelectedDate = ColorViewSelectedDate;
                if (d.Date.Month != Date.Month)
                    d.Visibility = ViewBorderingMonths;
                else
                    d.Visibility = Visibility.Visible;

                if (d.Date == DateTime.Today)
                {
                    d.Foreground = Brushes.White;
                    d.Background = ColorToDay;
                }
                else if (d.Date < DateTime.Today)
                {
                    d.Foreground = Brushes.Black;
                    switch (d.Date.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            d.Background = ColorDayOffFinish;
                            break;
                        case DayOfWeek.Sunday:
                            d.Background = ColorDayOffFinish;
                            break;
                        default:
                            d.Background = ColorDayFinish;
                            break;
                    }
                }
                else
                {
                    switch (d.Date.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            d.Background = ColorDayOff;
                            break;
                        case DayOfWeek.Sunday:
                            d.Background = ColorDayOff;
                            break;
                        default:
                            d.Background = Background;
                            break;
                    }
                }
                startDay = startDay.AddDays(1);
            }
        }
        private void OnPrevious(object sender, MouseButtonEventArgs e)
        {
            if(Date == DateTime.MinValue)
                return; 
            Date = Date.AddMonths(-1);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MaxValue) 
                return;  
            Date = Date.AddMonths(1);
        }
        private void OnNow(object sender, MouseButtonEventArgs e)
        {
            Date = DateTime.Now;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    var s = base.MeasureOverride(constraint);
        //    if (_Title != null)
        //    {
        //        _Title.FontSize = s.Height * 0.6;
        //        _Next.FontSize = _Title.FontSize;
        //        _Previous.FontSize = _Title.FontSize;
        //        }
        //    return s;
        //}
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var d = sender as DayControl;
            if (d != null)
                PeriodStart?.Invoke(d.Date);
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var d = sender as DayControl;
            if (d != null)
                PeriodFinish?.Invoke(d.Date);
        }
        public Action<DateTime> PeriodStart { get; set; }
        public Action<DateTime> PeriodFinish { get; set; }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _MainGrid = (Grid)GetTemplateChild(TP_MAIN_GRID_PART);
            _Title = (TitleControl)GetTemplateChild(TP_TITLE_PART);
            _Previous = (TitleControl)GetTemplateChild(TP_PREVIOUS_PART);
            _Next = (TitleControl)GetTemplateChild(TP_NEXT_PART);

            _Previous.Text = "<";
            _Next.Text = ">";
            _Previous.MouseLeftButtonDown += OnPrevious;
            _Next.MouseLeftButtonDown += OnNext;
            _Title.MouseLeftButtonDown += OnNow;

            for (int x = 0; x < 7; x++) // Second - for -day title
            {
                var d = new TitleControl()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };
                Grid.SetColumn(d, x);
                Grid.SetRow(d, 1);
                _MainGrid.Children.Add(d);
                _TitleDays.Add(d);
            }

            for (int y = 2; y < 8; y++) // first for - title; Second - for - day title
            {
                for (int x = 0; x < 7; x++)
                {
                    var d = new DayControl()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    Grid.SetColumn(d, x);
                    Grid.SetRow(d, y);
                    d.MouseDown += OnMouseDown;
                    d.MouseUp += OnMouseUp;
                    d.ViewSelectedDate = ViewSelectedDate;
                    d.ColorViewSelectedDate = ColorViewSelectedDate;
                    _MainGrid.Children.Add(d);
                    _Days.Add(d);
                }
            }
            UpdateElements();
        }

    }
}
