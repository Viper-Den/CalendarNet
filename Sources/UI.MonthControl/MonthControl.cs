using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UIDayControl;
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
    [System.Windows.Markup.ContentProperty("Items")]
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(FrameworkElement))]
    public class MonthControl : Control
    {
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext"; 
        private Grid _MainGrid;
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private DateTime _Date;
        private SolidColorBrush _ColorDayOff;
        private SolidColorBrush _ColorToDay;
        private SolidColorBrush _ColorDayFinish;
        private SolidColorBrush _ColorDayOffFinish; 
        private Visibility _ViewBorderingMonths;
        private Visibility _ViewButtons;
        private List<DayControl> _Days;
        private List<Label> _TitleDays;
        private ObservableCollection<IDateRange> _RangesCollection;

        ~MonthControl()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
        }
        public MonthControl()
        {
            _Days = new List<DayControl>();
            _TitleDays = new List<Label>();
            _Date = DateTime.Now;
            _RangesCollection = new ObservableCollection<IDateRange>();
        }
        static MonthControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthControl), new FrameworkPropertyMetadata(typeof(MonthControl)));
        }
        public static readonly DependencyProperty DateRangesProperty =
           DependencyProperty.Register("DateRanges", typeof(ObservableCollection<IDateRange>), 
               typeof(MonthControl), new PropertyMetadata(OnDateRangesChanged));

        public static readonly DependencyProperty ColorDayOffProperty =
            DependencyProperty.Register("ColorDayOff", typeof(SolidColorBrush), typeof(MonthControl), 
                new PropertyMetadata(ColorDayOffPropertyChanged));

        public static readonly DependencyProperty ColorDayFinishProperty =
            DependencyProperty.Register("ColorDayFinish", typeof(SolidColorBrush), typeof(MonthControl), 
                new PropertyMetadata(ColorDayFinishPropertyChanged));

        public static readonly DependencyProperty ColorDayOffFinishProperty =
            DependencyProperty.Register("ColorDayOffFinish", typeof(SolidColorBrush), typeof(MonthControl), 
                new PropertyMetadata(ColorDayOffFinishPropertyChanged));

        public static readonly DependencyProperty ColorToDayProperty =
            DependencyProperty.Register("ColorToDay", typeof(SolidColorBrush), typeof(MonthControl), 
                new PropertyMetadata(ColorToDayPropertyChanged));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MonthControl), 
                new PropertyMetadata(DatePropertyChanged));

        public static readonly DependencyProperty ViewBorderingMonthsProperty =
            DependencyProperty.Register("ViewBorderingMonths", typeof(Visibility), typeof(MonthControl), 
                new PropertyMetadata(ViewBorderingMonthsPropertyChanged));

        public static readonly DependencyProperty ViewButtonsProperty =
            DependencyProperty.Register("ViewButtons", typeof(Visibility), typeof(MonthControl),
                new PropertyMetadata(ViewButtonsPropertyChanged));

        private static void OnDateRangesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).DateRanges = (ObservableCollection<IDateRange>)e.NewValue;
        }

        
        public ObservableCollection<IDateRange> DateRanges
        {
            get { return (ObservableCollection<IDateRange>)GetValue(DateRangesProperty); }
            set
            {
                foreach(var r in value)
                {
                    foreach(var d in _Days)
                    {
                        if((d.Date >= r.Start) && (d.Date <= r.Finish)) 
                        {
                            d.Background = Brushes.Green;
                        }
                    }
                }
            }
        }
        private static void ColorDayOffFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorDayOffFinish = (SolidColorBrush)e.NewValue;
        }
        private static void ColorDayFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorDayFinish = (SolidColorBrush)e.NewValue;
        }
        private static void ColorToDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorToDay = (SolidColorBrush)e.NewValue;
        }
        public static void ColorDayOffPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ColorDayOff = (SolidColorBrush)e.NewValue;
        }
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           ((MonthControl)d).Date = (DateTime)e.NewValue;
        }
        public static void ViewBorderingMonthsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ViewBorderingMonths = (Visibility)e.NewValue;
        }
        public static void ViewButtonsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).ViewButtons = (Visibility)e.NewValue;
        }
        public SolidColorBrush ColorDayOffFinish
        {
            get { return _ColorDayOffFinish; }
            set
            {
                SetValue(ColorDayOffFinishProperty, value);
                _ColorDayOffFinish = value;
                if (_Title != null)
                {
                    UpdateElements();
                }
            }
        }
        public SolidColorBrush ColorDayOff
        {
            get { return _ColorDayOff; }
            set
            {
                SetValue(ColorDayOffProperty, value);
                _ColorDayOff = value;
                if (_Title != null)
                {
                    UpdateElements();
                }
            }
        }
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                var d = new DateTime(value.Year, value.Month, 1);
                SetValue(DateProperty, d);
                _Date = d;
                if (_Title != null)
                {
                    UpdateElements();
                }
            }
        }
        public Visibility ViewBorderingMonths
        {
            get { return _ViewBorderingMonths; }
            set
            {
                SetValue(ViewBorderingMonthsProperty, value);
                _ViewBorderingMonths = value;
                if (_Title != null)
                {
                    UpdateElements();
                }
            }
        }
        public Visibility ViewButtons
        {
            get { return _ViewButtons; }
            set
            {
                SetValue(ViewButtonsProperty, value);
                _ViewButtons = value;
                if (_Title != null)
                {
                    UpdateElements();
                }
            }
        }
        public SolidColorBrush ColorDayFinish
        {
            get { return _ColorDayFinish; }
            set
            {
                SetValue(ColorDayFinishProperty, value);
                _ColorDayFinish = value;
                if (_Title != null)
                {
                    UpdateElements();
                }
            }
        }
        public SolidColorBrush ColorToDay
        {
            get { return _ColorToDay; }
            set
            {
                SetValue(ColorToDayProperty, value);
                _ColorToDay = value;
                if (_Title != null)
                {
                    UpdateElements();
                }
            }
        }
        private void UpdateElements() 
        {
            var dayOfWeek = (int)Date.DayOfWeek;
            if(dayOfWeek == 0) { dayOfWeek = 6; } // to-do change to cultures selected mode
            else { dayOfWeek--; }


            _Previous.Visibility = ViewButtons;
            _Next.Visibility = ViewButtons;
            if (ViewButtons == Visibility.Visible)
            {
                Grid.SetColumnSpan(_Title, 5);
                _Title.Content = Date.ToString("MMMM yyyy");
            }
            else
            {
                Grid.SetColumnSpan(_Title, 7);
                _Title.Content = Date.ToString("MMMM");
            }

            var startDay = Date.AddDays(-dayOfWeek);
            foreach (var d in _TitleDays)
            {
                d.Content = startDay.ToString("ddd");
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
                if (d.Date.Month != Date.Month)
                {
                    d.Visibility = ViewBorderingMonths;
                }
                else
                {
                    d.Visibility = Visibility.Visible;
                }

                if (d.Date == DateTime.Today)
                {
                    d.Background = ColorToDay;
                }
                else if (d.Date < DateTime.Today)
                {
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
            Date = Date.AddMonths(-1);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            Date = Date.AddMonths(1);
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
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

            for (int x = 0; x < 7; x++) // Second - for -day title
            {
                var d = new Label()
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
                    _MainGrid.Children.Add(d);
                    _Days.Add(d);
                }
            }
            UpdateElements();
        }

    }
}
