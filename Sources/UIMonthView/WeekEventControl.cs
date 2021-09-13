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
        private Dictionary<Event, Label> _events;
        private Control _control;
        public EventDayCol(DayMonthEventControl day, Canvas canvas, Control control)
        {
            _canvas = canvas;
            _control = control;
            _canvas.MouseLeftButtonDown += DoAddEvent;
            Day = day;
            _events = new Dictionary<Event, Label>();
        }
        public Action<object> OnSelectedEvent { get; set; }
        public Action<DateTime> OnAddEvent { get; set; }
        private void DoSelectEvent(object sender, MouseButtonEventArgs e)
        {
            if ((sender is Label) && (_events.ContainsValue(sender as Label)))
                foreach (var ev in _events.Keys)
                {
                    if (_events[ev] == (sender as Label))
                    {
                        OnSelectedEvent?.Invoke((object)ev);
                        return;
                    }
                }
        }
        private void DoAddEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Point p = e.GetPosition(_control);
                int m = (int)((_canvas.ActualHeight / (24 * 60)) * p.Y);
                OnAddEvent?.Invoke(new DateTime(Date.Year, Date.Month, Date.Day, (m / 60), (m % 60), 0));
            }
        }

        public void AddEvent(Event e)
        {
            if (!e.Rule.IsDate(Date))
            {
                if (Day.Events.Contains(e))
                    Day.Events.Remove(e);
                if (_events.ContainsKey(e))
                {
                    var b = _events[e];
                    b.MouseUp -= DoSelectEvent;
                    _canvas.Children.Remove(b);
                    _events.Remove(e);
                }
                return;
            }
            if ((e.IsAllDay) && (!Day.Events.Contains(e)))
                Day.Events.Add(e);
            else if (!_events.ContainsKey(e))
            {
                var b = new Label();
                b.Background = e.Color;
                b.Content = e.Caption;
                b.MouseUp += DoSelectEvent;
                _events.Add(e, b);
                _canvas.Children.Add(b);
                Canvas.SetLeft(b, 0);
                Canvas.SetTop(b, GetSize(e.Rule.Start));
                b.Width = _canvas.ActualWidth - 10;
                var t = e.Rule.Finish - e.Rule.Start;
                b.Height = GetSize(new DateTime(e.Rule.Finish.Year, e.Rule.Finish.Month, e.Rule.Finish.Day, t.Hours, t.Minutes, 0));
            }
        }

        private double GetSize(DateTime d)
        {
            var r = ((d.Hour * 60) + d.Minute) * (_canvas.ActualHeight / (24 * 60));
            return r;
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
        private void Clear()
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
            foreach (var l in _events.Values)
            {
                l.Width = _canvas.ActualWidth - 10;
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
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class WeekEventControl : Selector
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
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private Grid _MainGrid;
        private List<EventDayCol> _Days;
        private List<Label> _TitleDays;

        ~WeekEventControl()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
            _Title.MouseLeftButtonDown -= OnNow;
        }
        public WeekEventControl() : base()
        {
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
        #region CommandSelectedEvent
        public static readonly DependencyProperty CommandSelectedEventProperty =
            DependencyProperty.Register("CommandSelectedEvent", typeof(ICommand), typeof(WeekEventControl), new PropertyMetadata(CommandSelectedEventChanged));
        public static void CommandSelectedEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).CommandSelectedEvent = (ICommand)e.NewValue;
        }
        public ICommand CommandSelectedEvent
        {
            get { return (ICommand)GetValue(CommandSelectedEventProperty); }
            set { SetValue(CommandSelectedEventProperty, value); }
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
                if (Events != null)
                    Events.CollectionChanged += DoNotifyCollectionChangedEventHandler;
                UpdateEvents();
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

        public void UpdateEvents()
        {
            if ((_Title != null) && (Events != null))
            {
                foreach (var d in _Days)
                {
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
                    UpdateEvents();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UpdateEvents();
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
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _Previous = (Label)GetTemplateChild(TP_PREVIOUS_PART);
            _Next = (Label)GetTemplateChild(TP_NEXT_PART);
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL1), (Canvas)GetTemplateChild(TP_CANVAS1), this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL2), (Canvas)GetTemplateChild(TP_CANVAS2), this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL3), (Canvas)GetTemplateChild(TP_CANVAS3), this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL4), (Canvas)GetTemplateChild(TP_CANVAS4), this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL5), (Canvas)GetTemplateChild(TP_CANVAS5), this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL6), (Canvas)GetTemplateChild(TP_CANVAS6), this));
            _Days.Add(new EventDayCol((DayMonthEventControl)GetTemplateChild(TP_CALL7), (Canvas)GetTemplateChild(TP_CANVAS7), this));
            foreach (var d in _Days)
            {
                d.Day.ItemTemplate = ItemTemplate;
                d.Day.AddAction += DoAddEvent;
                d.OnAddEvent += DoAddEvent;
                d.OnSelectedEvent += DoSelectedEvent;
            }
            _MainGrid.SizeChanged += MainGridSizeChanged;
            _Previous.Content = "<";
            _Next.Content = ">";
            _Previous.MouseLeftButtonDown += OnPrevious;
            _Next.MouseLeftButtonDown += OnNext;
            _Title.MouseLeftButtonDown += OnNow;
            UpdateElements();
        }
        public void MainGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var d in _Days)
            {
                d.Update();
            }
        }
        private void DoSelectedEvent(object o)
        {
            if (o is Event)
                CommandSelectedEvent?.Execute((Event)o);
        }
        private void DoAddEvent(DateTime date)
        {
            AddEvent?.Execute(date);
        }
    }
}
