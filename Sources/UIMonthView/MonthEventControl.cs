using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Destiny.Core;
using System.Collections.Specialized;

namespace MonthEvent
{
    //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.controls.itemscontrol?view=netcore-3.1
    [TemplatePart(Name = MonthEventControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthEventControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthEventControl.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthEventControl.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class MonthEventControl : Control
    {
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext";
        private Grid _MainGrid;
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private List<DayMonthEventControl> _Days;
        private List<Label> _TitleDays;
        ~MonthEventControl()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
            _Title.MouseLeftButtonDown -= OnNow;
        }
        public MonthEventControl()
        {
            _Days = new List<DayMonthEventControl>();
            _TitleDays = new List<Label>();
            ColorDayFinish = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorDayOff = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorDayOffFinish = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            ColorToDay = new SolidColorBrush(Color.FromRgb(17, 110, 190));
        }
        static MonthEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthEventControl), new FrameworkPropertyMetadata(typeof(MonthEventControl)));
        }


        private void DoSelectedEvent(object o) 
        {
            if(o is Event)
                CommandSelectedEvent?.Execute((Event)o);
        }

        #region AddEvent
        public static readonly DependencyProperty AddEventProperty =
            DependencyProperty.Register("AddEvent", typeof(ICommand), typeof(MonthEventControl), new PropertyMetadata(AddEventPropertyChanged));
        public static void AddEventPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).AddEvent = (ICommand)e.NewValue;
        }
        public ICommand AddEvent
        {
            get { return (ICommand)GetValue(AddEventProperty); }
            set { SetValue(AddEventProperty, value); }
        }
        #endregion

        #region CommandSelectedEvent
        public static readonly DependencyProperty CommandSelectedEventProperty =
            DependencyProperty.Register("CommandSelectedEvent", typeof(ICommand), typeof(MonthEventControl), new PropertyMetadata(CommandSelectedEventChanged));
        public static void CommandSelectedEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).CommandSelectedEvent = (ICommand)e.NewValue;
        }
        public ICommand CommandSelectedEvent
        {
            get { return (ICommand)GetValue(CommandSelectedEventProperty); }
            set { SetValue(CommandSelectedEventProperty, value); }
        }
        #endregion

        #region ItemTemplate
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(MonthEventControl), new PropertyMetadata(ItemTemplateChanged));
        public static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).ItemTemplate = (DataTemplate)e.NewValue;
        }
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set
            {
                SetValue(ItemTemplateProperty, value);
                UpdateElements();
            }
        }
        #endregion

        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register("Events", typeof(ObservableCollection<Event>), typeof(MonthEventControl), new PropertyMetadata(OnEventsChanged));

        public static readonly DependencyProperty ColorDayOffProperty =
            DependencyProperty.Register("ColorDayOff", typeof(SolidColorBrush), typeof(MonthEventControl), new PropertyMetadata(ColorDayOffPropertyChanged));

        public static readonly DependencyProperty ColorDayFinishProperty =
            DependencyProperty.Register("ColorDayFinish", typeof(SolidColorBrush), typeof(MonthEventControl), new PropertyMetadata(ColorDayFinishPropertyChanged));

        public static readonly DependencyProperty ColorDayOffFinishProperty =
            DependencyProperty.Register("ColorDayOffFinish", typeof(SolidColorBrush), typeof(MonthEventControl), new PropertyMetadata(ColorDayOffFinishPropertyChanged));

        public static readonly DependencyProperty ColorToDayProperty =
            DependencyProperty.Register("ColorToDay", typeof(SolidColorBrush), typeof(MonthEventControl), new PropertyMetadata(ColorToDayPropertyChanged));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MonthEventControl), new PropertyMetadata(DatePropertyChanged));

        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).Events = (ObservableCollection<Event>)e.NewValue;
        }
         private static void ColorDayOffFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).ColorDayOffFinish = (SolidColorBrush)e.NewValue;
        }
        private static void ColorDayFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).ColorDayFinish = (SolidColorBrush)e.NewValue;
        }
        private static void ColorToDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).ColorToDay = (SolidColorBrush)e.NewValue;
        }
        public static void ColorDayOffPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).ColorDayOff = (SolidColorBrush)e.NewValue;
        }
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).Date = (DateTime)e.NewValue;
        }
        public void UpdateEvents()
        {
            if ((_Title != null)&&(Events != null))
            {
                foreach (var d in _Days)
                {
                    foreach (var e in Events)
                    {
                        if (e.Rule.IsDate(d.Date))
                        {
                            if (!d.Events.Contains(e))
                                d.Events.Add(e);
                        }
                        else 
                        { 
                            if (d.Events.Contains(e))
                                d.Events.Remove(e);
                        }
                    }
                }
            }
        }
        public ObservableCollection<Event> Events
        {
            get { return (ObservableCollection<Event>) GetValue(EventsProperty);}
            set
            {
                if (Events != null)
                    Events.CollectionChanged -= DoNotifyCollectionChangedEventHandler;

                SetValue(EventsProperty, value);
                if(Events != null)
                  Events.CollectionChanged += DoNotifyCollectionChangedEventHandler;
                UpdateEvents();
            }
        }

        private void DoNotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
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

        public SolidColorBrush ColorDayOffFinish
        {
            get { return (SolidColorBrush)GetValue(ColorDayOffFinishProperty); }
            set
            {
                SetValue(ColorDayOffFinishProperty, value);
                UpdateElements();
            }
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
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, new DateTime(value.Year, value.Month, 1));
                UpdateElements();
            }
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
        public SolidColorBrush ColorToDay
        {
            get { return (SolidColorBrush)GetValue(ColorToDayProperty); }
            set
            {
                SetValue(ColorToDayProperty, value);
                UpdateElements();
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
                    d.Foreground = Brushes.White;
                    d.Background = ColorToDay;
                }
                else if (d.Date.Month != Date.Month)
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
        private void OnAddEvent(DateTime date)
        {
            AddEvent?.Execute(date);
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
                    var d = new DayMonthEventControl()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    Grid.SetColumn(d, x);
                    Grid.SetRow(d, y);
                    _MainGrid.Children.Add(d);
                    _Days.Add(d);
                    d.OnSelectedEvent += DoSelectedEvent;
                    d.AddAction += OnAddEvent;
                }
            }
            UpdateElements();
        }

    }
}
