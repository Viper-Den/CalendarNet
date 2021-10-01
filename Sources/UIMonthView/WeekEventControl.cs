using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Destiny.Core;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;

namespace MonthEvent
{

    public class EventDayCol
    {
        private Canvas _canvas;
        private Dictionary<Event, Grid> _events;
        private ScrollViewer _scrollViewer;
        private ITimeCalculator _timeCalculator;
        public EventDayCol(DayMonthEventControl day, Canvas canvas, ScrollViewer scrollViewer, ITimeCalculator timeCalculator)
        {
            _timeCalculator = timeCalculator;
            _canvas = canvas;
            _scrollViewer = scrollViewer;
            _canvas.AllowDrop = true;
            _canvas.Drop += DoDrop;
            _canvas.MouseLeftButtonDown += DoAddEvent;
            Day = day;
            _events = new Dictionary<Event, Grid>();
        }
        private void DoDrop(object sender, DragEventArgs e)
        {
            int m = GetMin(e.GetPosition(_scrollViewer).Y);
            if (sender is Event)
            {
                var ev = ((Event)sender).Clone();
                ev.IsAllDay = false;
                ev.Rule.Start = new DateTime(Date.Year, Date.Month, Date.Day, (m / 60), (m % 60), 0);
                ev.Rule.Finish = ev.Rule.Start.AddHours(1);
            }
        }
        public Action<DateTime> OnAddEvent { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        private void DoAddEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                int m = GetMin(e.GetPosition(_scrollViewer).Y);
                OnAddEvent?.Invoke(new DateTime(Date.Year, Date.Month, Date.Day, (m / 60), 0, 0)); // (m % 60) 
            }
        }
        private int GetMin(double posY)
        {
            var Y = posY + _scrollViewer.VerticalOffset;
            int m = _timeCalculator.GetMin(Y);
            if (m > (24 * 60))
                return (24 * 60);
            else if (m < 0)
                return 0;
            else
                return m;
        }

        public void Remove(Event e)
        {
            if (e.Rule.IsDate(Date))
            {
                if (Day.Events.Contains(e))
                    Day.Events.Remove(e);
                if (_events.ContainsKey(e))
                {
                    var b = _events[e];
                    _canvas.Children.Remove(b);
                    _events.Remove(e);
                }
            }
        }
        public void AddEvent(Event e)
        {
            if (!e.Rule.IsDate(Date))
                return;
            if ((e.IsAllDay) && (!Day.Events.Contains(e)))
                Day.Events.Add(e);
            else if ((!e.IsAllDay) && (!_events.ContainsKey(e)))
            {
                var c = new Grid();
                //c.VerticalContentAlignment = VerticalAlignment.Stretch;
                //c.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                var ch = (Control)ItemTemplate.LoadContent();
                c.Children.Add(ch);
                ch.DataContext = e;

                c.DataContext = e;
                _events.Add(e, c);
                _canvas.Children.Add(c);
                Canvas.SetLeft(c, 0);
                Canvas.SetTop(c, _timeCalculator.GetPos(e.Rule.Start));
                c.Width = _canvas.ActualWidth - 10;
                c.Height = _timeCalculator.GetSize(e.Rule.Start, e.Rule.Finish);
            }
        }
        public void RemoveEvent(Event e)
        {
            if (!e.Rule.IsDate(Date))
                return;
            if (e.IsAllDay)
                Day.Events.Remove(e);
            else if (_events.ContainsKey(e))
            {
                var b = _events[e];
                _canvas.Children.Remove(b);
            }
        }
        public void Clear()
        {
            Day.Events.Clear();
            foreach (var e in _events.Keys)
            {
                var b = _events[e];
                _canvas.Children.Remove(b);
            }
            _events.Clear();
        }
        public void Update()
        {
            foreach (var ev in _events.Keys)
            {
                _events[ev].Width = _canvas.ActualWidth - 10;
                _events[ev].Height = _timeCalculator.GetSize(ev.Rule.Start, ev.Rule.Finish);
                Canvas.SetTop(_events[ev], _timeCalculator.GetPos(ev.Rule.Start));
            }
        }
        public DayMonthEventControl Day { get; }
        public DateTime Date
        {
            get
            {
                return Day.Date;
            }
            set
            {
                Day.Date = value;
                Clear();
            }
        }
    }

    public class HourRow
    {
        private RowDefinition _row;
        public HourRow(RowDefinition row)
        {
            _row = row ?? throw new ArgumentNullException(nameof(row));
        }
        public double GetSizeMin()
        {
            return (_row.Height.Value / DateHelper.MIN_IN_HOUR);
        }
        public double GetSizeHour()
        {
            return _row.Height.Value;
        }
        public void SetSize(int size)
        {
            _row.Height = new GridLength(size);
        }
    }

    public interface ITimeCalculator
    {
        int GetMin(double clickPos);
        double GetPos(DateTime time);
        double GetSize(DateTime start, DateTime finish);
    }


    //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.controls.itemscontrol?view=netcore-3.1
    [TemplatePart(Name = WeekEventControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CALL1, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CALL2, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CALL3, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CALL4, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CALL5, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CALL6, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CALL7, Type = typeof(FrameworkElement))]

    [TemplatePart(Name = WeekEventControl.TP_CANVAS1, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CANVAS2, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CANVAS3, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CANVAS4, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CANVAS5, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CANVAS6, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_CANVAS7, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_BUTTON_HIDE, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_BUTTON_HIDE_HOURS, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_SCROLLVIEWER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_ROW_EVENTS_VIEW, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = WeekEventControl.TP_TIME_GRID, Type = typeof(FrameworkElement))]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class WeekEventControl : BaseControl, ITimeCalculator
    {
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext";
        private const string TP_CALL1 = "xDayMonthEventControl1";
        private const string TP_CALL2 = "xDayMonthEventControl2";
        private const string TP_CALL3 = "xDayMonthEventControl3";
        private const string TP_CALL4 = "xDayMonthEventControl4";
        private const string TP_CALL5 = "xDayMonthEventControl5";
        private const string TP_CALL6 = "xDayMonthEventControl6";
        private const string TP_CALL7 = "xDayMonthEventControl7";
        private const string TP_CANVAS1 = "xCanvas1";
        private const string TP_CANVAS2 = "xCanvas2";
        private const string TP_CANVAS3 = "xCanvas3";
        private const string TP_CANVAS4 = "xCanvas4";
        private const string TP_CANVAS5 = "xCanvas5";
        private const string TP_CANVAS6 = "xCanvas6";
        private const string TP_CANVAS7 = "xCanvas7";
        private const string TP_SCROLLVIEWER = "xScrollViewer";
        private const string TP_BUTTON_HIDE = "xHide";
        private const string TP_BUTTON_HIDE_HOURS = "xHideHours";
        private const string TP_ROW_EVENTS_VIEW = "xRowEventsView";
        private const string TP_TIME_GRID = "xTimeGrid";
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private Grid _MainGrid;
        private Grid _TimeGrid;
        private Button _Hide;
        private Button _HideHours;
        private ScrollViewer _scrollViewer;
        private List<EventDayCol> _Days;
        private List<Label> _TitleDays;
        private RowDefinition _RowEventsView;
        private Dictionary<int, HourRow> _Rows;
        private bool _IsHoursHide;
        private bool _IsEventsHide;
        private double _heightDayMonth;

        ~WeekEventControl()
        {
            _Rows.Clear();
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
            _Title.MouseLeftButtonDown -= OnNow;
        }
        public WeekEventControl() : base()
        {
            _Rows = new Dictionary<int, HourRow>();
            _Days = new List<EventDayCol>();
            _TitleDays = new List<Label>();
            ColorDayFinish = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorDayOff = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorDayOffFinish = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            ColorToDay = new SolidColorBrush(Color.FromRgb(17, 110, 190));
        }
        static WeekEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WeekEventControl), new FrameworkPropertyMetadata(typeof(WeekEventControl)));
        }
        public bool IsHoursHide { get => _IsHoursHide; 
            set 
            {
                _IsHoursHide = value;
                UpdateRows();
                UpdateEventDayCol();
            } 
        }
        public bool IsEventsHide { get => _IsEventsHide;
            set
            {
                _IsEventsHide = value;
                UpdateEventsView();
            }
        }
        public double GetPos(DateTime time)
        {
            double h = 0;
            double m = 0;
            foreach (var i in _Rows.Keys)
            {
                h += _Rows[i].GetSizeHour();
                if (i == time.Hour)
                {
                    m = _Rows[i].GetSizeMin() * time.Minute;
                    break;
                }
            }
            return h + m;
        }
        public double GetSize(DateTime start, DateTime finish)
        {
            double h = 0;
            double m = 0;
            foreach (var i in _Rows.Keys)
            {
                if(i < start.Hour)
                {
                    continue;
                }

                if (i == start.Hour)
                {
                    m = _Rows[i].GetSizeMin() * start.Minute;
                }
                if (i == finish.Hour)
                {
                    m += _Rows[i].GetSizeMin() * finish.Minute;
                    break;
                }
                h += _Rows[i].GetSizeHour();
            }
            return h + m;
        }

        public int GetMin(double clickPos)
        {
            double p = 0;
            int i = 0;
            foreach(var r in _Rows.Values)
            {
                p += r.GetSizeHour();
                if (clickPos < p)
                {
                    p -= r.GetSizeHour();
                    break;
                }
                i++;
            }
            var d = (i * DateHelper.MIN_IN_HOUR) + ((clickPos - p) / _Rows[i].GetSizeMin());
            return Convert.ToInt32(d);
        }

        #region IgnoreHours
        public static readonly DependencyProperty IgnoreHoursProperty =
            DependencyProperty.Register(nameof(IgnoreHours), typeof(ObservableCollection<int>), typeof(WeekEventControl), new PropertyMetadata(IgnoreHoursPropertyChanged));
        public static void IgnoreHoursPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).IgnoreHours = (ObservableCollection<int>)e.NewValue;
        }
        public ObservableCollection<int> IgnoreHours
        {
            get { return (ObservableCollection<int>)GetValue(IgnoreHoursProperty); }
            set
            {
                SetValue(IgnoreHoursProperty, value);
                UpdateRows();
            }
        }
        #endregion
        #region AddEvent
        public static readonly DependencyProperty AddEventProperty =
            DependencyProperty.Register("AddEvent", typeof(ICommand), typeof(WeekEventControl), new PropertyMetadata(AddEventPropertyChanged));
        public static void AddEventPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).AddEvent = (ICommand)e.NewValue;
        }
        public ICommand AddEvent
        {
            get { return (ICommand)GetValue(AddEventProperty); }
            set { SetValue(AddEventProperty, value); }
        }
        #endregion
        #region ColorDayOffFinish
        public static readonly DependencyProperty ColorDayOffFinishProperty =
            DependencyProperty.Register("ColorDayOffFinish", typeof(SolidColorBrush), typeof(WeekEventControl), new PropertyMetadata(ColorDayOffFinishPropertyChanged));

        private static void ColorDayOffFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).ColorDayOffFinish = (SolidColorBrush)e.NewValue;
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
        #region Events
        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register("Events", typeof(ObservableCollection<Event>), typeof(WeekEventControl), new PropertyMetadata(OnEventsChanged));

        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).Events = (ObservableCollection<Event>)e.NewValue;
        }
        public ObservableCollection<Event> Events
        {
            get { return (ObservableCollection<Event>)GetValue(EventsProperty); }
            set
            {
                if (Events != null)
                    Events.CollectionChanged -= DoNotifyCollectionChangedEventHandler;

                SetValue(EventsProperty, value);
                UpdateEvents();
                if (Events != null)
                    Events.CollectionChanged += DoNotifyCollectionChangedEventHandler;
            }
        }
        #endregion
        #region ColorDayOff
        public static readonly DependencyProperty ColorDayOffProperty =
            DependencyProperty.Register("ColorDayOff", typeof(SolidColorBrush), typeof(WeekEventControl), new PropertyMetadata(ColorDayOffPropertyChanged));
        public static void ColorDayOffPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).ColorDayOff = (SolidColorBrush)e.NewValue;
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
        #region HourHeight
        public static readonly DependencyProperty HourHeightProperty =
            DependencyProperty.Register(nameof(HourHeight), typeof(int), typeof(WeekEventControl), new PropertyMetadata(HourHeightChanged));

        public static void HourHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).HourHeight = (int)e.NewValue;
        }
        public int HourHeight
        {
            get { return (int)GetValue(HourHeightProperty); }
            set
            {
                SetValue(HourHeightProperty, value);
                UpdateRows();
            }
        }
        #endregion
        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(WeekEventControl), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, new DateTime(value.Year, value.Month, value.Day)); // delete time
                UpdateElements();
            }
        }
        #endregion
        #region ColorDayFinish
        public static readonly DependencyProperty ColorDayFinishProperty =
            DependencyProperty.Register("ColorDayFinish", typeof(SolidColorBrush), typeof(WeekEventControl), new PropertyMetadata(ColorDayFinishPropertyChanged));

        private static void ColorDayFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).ColorDayFinish = (SolidColorBrush)e.NewValue;
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
        #region ColorToDay
        public static readonly DependencyProperty ColorToDayProperty =
            DependencyProperty.Register("ColorToDay", typeof(SolidColorBrush), typeof(WeekEventControl), new PropertyMetadata(ColorToDayPropertyChanged));
        private static void ColorToDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).ColorToDay = (SolidColorBrush)e.NewValue;
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
        #region WeekEventTemplate
        /// <summary>
        ///     The DependencyProperty for the ItemTemplate property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty WeekEventTemplateProperty =
                DependencyProperty.Register(nameof(WeekEventTemplate), typeof(DataTemplate), typeof(WeekEventControl),
                        new FrameworkPropertyMetadata((DataTemplate)null, new PropertyChangedCallback(OnWeekEventTemplateChanged)));

        /// <summary>
        ///     ItemTemplate is the template used to display each item.
        /// </summary>
        public DataTemplate WeekEventTemplate
        {
            get { return (DataTemplate)GetValue(WeekEventTemplateProperty); }
            set
            {
                SetValue(WeekEventTemplateProperty, value);
                UpdateTemplate();
            }
        }

        private void UpdateTemplate()
        {
            foreach (var d in _Days)
            {
                d.ItemTemplate = WeekEventTemplate;
            }
        }

        /// <summary>
        ///     Called when ItemTemplateProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnWeekEventTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).WeekEventTemplate = (DataTemplate)e.NewValue;
        }
        #endregion
        #region Wather
        public static readonly DependencyProperty DayWatherCollectionProperty =
            DependencyProperty.Register(nameof(DayWatherCollection), typeof(Dictionary<DateTime, IDayWather>), typeof(WeekEventControl), new PropertyMetadata(DayWatherCollectionPropertyChanged));

        public static void DayWatherCollectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).DayWatherCollection = (Dictionary<DateTime, IDayWather>)e.NewValue;
        }
        public Dictionary<DateTime, IDayWather> DayWatherCollection
        {
            get { return (Dictionary<DateTime, IDayWather>)GetValue(DayWatherCollectionProperty); }
            set
            {
                SetValue(DayWatherCollectionProperty, value);
                UpdateWather();
            }
        }
        public void UpdateWather()
        {
            if (_Days == null)
                return;
            foreach (var d in _Days)
            {
                d.Day.DayWatherCollection = DayWatherCollection;
            }
        }
        #endregion
        #region WeatherTemplate
        public static readonly DependencyProperty WeatherTemplateProperty =
            DependencyProperty.Register(nameof(WeatherTemplate), typeof(DataTemplate), typeof(WeekEventControl), new PropertyMetadata(WeatherTemplateChanged));
        public static void WeatherTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).WeatherTemplate = (DataTemplate)e.NewValue;
        }
        public DataTemplate WeatherTemplate
        {
            get { return (DataTemplate)GetValue(WeatherTemplateProperty); }
            set
            {
                SetValue(WeatherTemplateProperty, value);
                UpdateWeatherTemplate();
            }
        }
        public void UpdateWeatherTemplate()
        {
            if (_Days == null)
                return;
            foreach (var d in _Days)
            {
                d.Day.WeatherTemplate = WeatherTemplate;
            }
        }
        #endregion
        public void UpdateRows()
        {
            if (IgnoreHours == null)
                return;
            foreach (var i in _Rows.Keys)
            {
                if ((IgnoreHours.Contains(i))&&(_IsHoursHide))
                    _Rows[i].SetSize(0);
                else
                    _Rows[i].SetSize(HourHeight);
            }
        }
        private void UpdateEventsView()
        {
            if ((_RowEventsView == null) ||(_Days.Count == 0))
                return;

            if (IsEventsHide)
                _RowEventsView.Height = new GridLength(_Days[0].Day.TitleSize);
            else
                _RowEventsView.Height = new GridLength(_heightDayMonth); // 80 приблизительно
        }
        public void UpdateEvents()
        {
            if ((_Title != null) && (Events != null))
            {
                foreach (var d in _Days)
                {
                    d.Clear();
                    foreach (var e in Events)
                    {
                      d.AddEvent(e);
                    }
                }
            }
        }

        private void DoNotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var d in _Days)
                    {
                        foreach (var ev in e.NewItems)
                        {
                          d.AddEvent((Event)ev);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var d in _Days)
                    {
                        foreach (var ev in e.OldItems)
                        {
                            d.Remove((Event)ev);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    UpdateEvents();
                    break;
            }
        }
        private void UpdateElements()
        {
            if (_Title == null) { return; }
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
                    d.Day.Background = ColorToDay;
                }
                else if (d.Date.Month < Date.Month)
                {
                    switch (d.Date.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            d.Day.Background = ColorDayOffFinish;
                            break;
                        case DayOfWeek.Sunday:
                            d.Day.Background = ColorDayOffFinish;
                            break;
                        default:
                            d.Day.Background = ColorDayFinish;
                            break;
                    }
                }
                else
                {
                    switch (d.Date.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            d.Day.Background = ColorDayOff;
                            break;
                        case DayOfWeek.Sunday:
                            d.Day.Background = ColorDayOff;
                            break;
                        default:
                            d.Day.Background = Background;
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
            Date = Date.AddDays(-7);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MaxValue) { return; }
            Date = Date.AddDays(7);
        }
        private void OnNow(object sender, MouseButtonEventArgs e)
        {
            Date = DateTime.Now;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _MainGrid = (Grid)GetTemplateChild(TP_MAIN_GRID_PART);
            _TimeGrid = (Grid)GetTemplateChild(TP_TIME_GRID); 
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _Previous = (Label)GetTemplateChild(TP_PREVIOUS_PART);
            _Next = (Label)GetTemplateChild(TP_NEXT_PART);
            _scrollViewer = (ScrollViewer)GetTemplateChild(TP_SCROLLVIEWER);
            _Hide = (Button)GetTemplateChild(TP_BUTTON_HIDE);
            _HideHours = (Button)GetTemplateChild(TP_BUTTON_HIDE_HOURS);  
            _RowEventsView = (RowDefinition)GetTemplateChild(TP_ROW_EVENTS_VIEW);
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL1), (Canvas)GetTemplateChild(TP_CANVAS1), _scrollViewer, this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL2), (Canvas)GetTemplateChild(TP_CANVAS2), _scrollViewer, this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL3), (Canvas)GetTemplateChild(TP_CANVAS3), _scrollViewer, this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL4), (Canvas)GetTemplateChild(TP_CANVAS4), _scrollViewer, this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL5), (Canvas)GetTemplateChild(TP_CANVAS5), _scrollViewer, this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL6), (Canvas)GetTemplateChild(TP_CANVAS6), _scrollViewer, this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL7), (Canvas)GetTemplateChild(TP_CANVAS7), _scrollViewer, this));
            for (var i = 1; i < 25; i++)
                _Rows.Add(i, new HourRow((RowDefinition)GetTemplateChild($"R{i}")));

            foreach (var d in _Days)
            {
                d.Day.ItemTemplate = ItemTemplate;
                d.Day.AddAction += DoAddEvent;
                d.OnAddEvent += DoAddEvent;
            }
            _MainGrid.SizeChanged += MainGridSizeChanged;
            _Previous.Content = "<";
            _Next.Content = ">";
            _heightDayMonth = _RowEventsView.Height.Value;
            _Previous.MouseLeftButtonDown += OnPrevious;
            _Next.MouseLeftButtonDown += OnNext;
            _Title.MouseLeftButtonDown += OnNow;
            _Hide.Click += DoHideClick;
            _HideHours.Click += DoHideHoursClick;
            UpdateElements();
            UpdateTemplate();
            UpdateRows();
            UpdateEventsView();
            UpdateRows();
            UpdateWeatherTemplate();
            UpdateWather();
        }
        private void UpdateEventDayCol()
        {
            foreach (var d in _Days)
            {
                d.Update();
            }
        }

        private void DoHideHoursClick(object sender, RoutedEventArgs e)
        {
            IsHoursHide = !IsHoursHide;
        }

        private void DoHideClick(object sender, RoutedEventArgs e)
        {
            IsEventsHide = !IsEventsHide;
        }

        public void MainGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateEventDayCol();
        }
        private void DoAddEvent(DateTime date)
        {
            AddEvent?.Execute(date);
        }
    }
}
