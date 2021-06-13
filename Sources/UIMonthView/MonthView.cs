using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UIDayMonth;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace UIMonthView
{
    //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.controls.itemscontrol?view=netcore-3.1
    [TemplatePart(Name = MonthView.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthView.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthView.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthView.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class MonthView : Control
    {
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext";
        private Grid _MainGrid;
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private SolidColorBrush _ColorDayOff;
        private SolidColorBrush _ColorToDay;
        private SolidColorBrush _ColorDayFinish;
        private SolidColorBrush _ColorDayOffFinish;
        private List<DayMonthControl> _Days;
        private List<Label> _TitleDays;

        ~MonthView()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
        }
        public MonthView()
        {
            _Days = new List<DayMonthControl>();
            _TitleDays = new List<Label>();
            Date = DateTime.Now;
            ColorDayFinish = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorDayOff = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorDayOffFinish = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            ColorToDay = new SolidColorBrush(Color.FromRgb(17, 110, 190));
        }
        static MonthView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthView), new FrameworkPropertyMetadata(typeof(MonthView)));
        }
        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register("Events", typeof(ObservableCollection<IEvent>),
               typeof(MonthView), new PropertyMetadata(OnEventsChanged));

        public static readonly DependencyProperty ColorDayOffProperty =
            DependencyProperty.Register("ColorDayOff", typeof(SolidColorBrush), typeof(MonthView),
                new PropertyMetadata(ColorDayOffPropertyChanged));

        public static readonly DependencyProperty ColorDayFinishProperty =
            DependencyProperty.Register("ColorDayFinish", typeof(SolidColorBrush), typeof(MonthView),
                new PropertyMetadata(ColorDayFinishPropertyChanged));

        public static readonly DependencyProperty ColorDayOffFinishProperty =
            DependencyProperty.Register("ColorDayOffFinish", typeof(SolidColorBrush), typeof(MonthView),
                new PropertyMetadata(ColorDayOffFinishPropertyChanged));

        public static readonly DependencyProperty ColorToDayProperty =
            DependencyProperty.Register("ColorToDay", typeof(SolidColorBrush), typeof(MonthView),
                new PropertyMetadata(ColorToDayPropertyChanged));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MonthView),
                new PropertyMetadata(DatePropertyChanged));

        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthView)d).Events = (ObservableCollection<IEvent>)e.NewValue;
        }
        public ObservableCollection<IEvent> Events
        {
            get { return (ObservableCollection<IEvent>)GetValue(EventsProperty); }
            set
            {
                SetValue(EventsProperty, value);
                if (_Title != null)
                {
                    UpdateEvents();
                }
            }
        }
        public void UpdateEvents()
        {
            foreach (var d in _Days)
            {
                d.Events = Events;
            }
        }
            private static void ColorDayOffFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthView)d).ColorDayOffFinish = (SolidColorBrush)e.NewValue;
        }
        private static void ColorDayFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthView)d).ColorDayFinish = (SolidColorBrush)e.NewValue;
        }
        private static void ColorToDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthView)d).ColorToDay = (SolidColorBrush)e.NewValue;
        }
        public static void ColorDayOffPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthView)d).ColorDayOff = (SolidColorBrush)e.NewValue;
        }
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthView)d).Date = (DateTime)e.NewValue;
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
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, new DateTime(value.Year, value.Month, 1));
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
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (dayOfWeek == 0) { dayOfWeek = 6; } // change to DateTimeFormat
            else { dayOfWeek--; }


            _Title.Content = Date.ToString("yyyy MMMM");

            var startDay = Date.AddDays(-dayOfWeek);
            foreach (var d in _TitleDays)
            {
                d.Content = startDay.ToString("dddd");
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

                if (d.Date == DateTime.Today)
                {
                    d.Background = ColorToDay;
                }
                else if (d.Date.Month != Date.Month)
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
            UpdateEvents();
        }
        private void OnPrevious(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MinValue) { return; }
            Date = Date.AddMonths(-1);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MaxValue) { return; }
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
                    var d = new DayMonthControl()
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
