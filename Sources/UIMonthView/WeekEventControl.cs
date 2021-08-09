using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Destiny.Core;

namespace MonthEvent
{
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
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class WeekEventControl : Control
    {
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext";
        private const string TP_CALL1 = "xColumn1";
        private const string TP_CALL2 = "xColumn2";
        private const string TP_CALL3 = "xColumn3";
        private const string TP_CALL4 = "xColumn4";
        private const string TP_CALL5 = "xColumn5";
        private const string TP_CALL6 = "xColumn6";
        private const string TP_CALL7 = "xColumn7";
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private List<DayMonthEventControl> _Days;
        private List<Label> _TitleDays;

        ~WeekEventControl()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
            _Title.MouseLeftButtonDown -= OnNow;
        }
        public WeekEventControl()
        {
            _Days = new List<DayMonthEventControl>();
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

        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register("Events", typeof(ObservableCollection<IEvent>),
               typeof(WeekEventControl), new PropertyMetadata(OnEventsChanged));

        public static readonly DependencyProperty ColorDayOffProperty =
            DependencyProperty.Register("ColorDayOff", typeof(SolidColorBrush), typeof(WeekEventControl),
                new PropertyMetadata(ColorDayOffPropertyChanged));

        public static readonly DependencyProperty ColorDayFinishProperty =
            DependencyProperty.Register("ColorDayFinish", typeof(SolidColorBrush), typeof(WeekEventControl),
                new PropertyMetadata(ColorDayFinishPropertyChanged));

        public static readonly DependencyProperty ColorDayOffFinishProperty =
            DependencyProperty.Register("ColorDayOffFinish", typeof(SolidColorBrush), typeof(WeekEventControl),
                new PropertyMetadata(ColorDayOffFinishPropertyChanged));

        public static readonly DependencyProperty ColorToDayProperty =
            DependencyProperty.Register("ColorToDay", typeof(SolidColorBrush), typeof(WeekEventControl),
                new PropertyMetadata(ColorToDayPropertyChanged));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(WeekEventControl),
                new PropertyMetadata(DatePropertyChanged));

        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).Events = (ObservableCollection<IEvent>)e.NewValue;
        }
        private static void ColorDayOffFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).ColorDayOffFinish = (SolidColorBrush)e.NewValue;
        }
        private static void ColorDayFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).ColorDayFinish = (SolidColorBrush)e.NewValue;
        }
        private static void ColorToDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).ColorToDay = (SolidColorBrush)e.NewValue;
        }
        public static void ColorDayOffPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).ColorDayOff = (SolidColorBrush)e.NewValue;
        }
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekEventControl)d).Date = (DateTime)e.NewValue;
        }
        public ObservableCollection<IEvent> Events
        {
            get { return (ObservableCollection<IEvent>)GetValue(EventsProperty); }
            set
                {
                    SetValue(EventsProperty, value);
                    UpdateEvents();
                }
        }
        public void UpdateEvents()
        {
            if (_Title != null)
            {
                //foreach (var d in _Days)
                //{
                //    d.Events = Events;
                //}
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
                SetValue(DateProperty, new DateTime(value.Year, value.Month, value.Day)); // delete time
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
            get { return (SolidColorBrush)GetValue(ColorToDayProperty); ; }
            set
            {
                SetValue(ColorToDayProperty, value);
                UpdateElements();
            }
        }
        private void UpdateElements()
        {
            if (_Title == null) { return;  }
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
                else if (d.Date.Month < Date.Month)
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
            //S_MainGrid = (Grid)GetTemplateChild(TP_MAIN_GRID_PART);
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _Previous = (Label)GetTemplateChild(TP_PREVIOUS_PART);
            _Next = (Label)GetTemplateChild(TP_NEXT_PART);
            _Days.Add((DayMonthEventControl)GetTemplateChild(TP_CALL1));
            _Days.Add((DayMonthEventControl)GetTemplateChild(TP_CALL2));
            _Days.Add((DayMonthEventControl)GetTemplateChild(TP_CALL3));
            _Days.Add((DayMonthEventControl)GetTemplateChild(TP_CALL4));
            _Days.Add((DayMonthEventControl)GetTemplateChild(TP_CALL5));
            _Days.Add((DayMonthEventControl)GetTemplateChild(TP_CALL6));
            _Days.Add((DayMonthEventControl)GetTemplateChild(TP_CALL7));
                                                                  
            _Previous.Content = "<";
            _Next.Content = ">";
            _Previous.MouseLeftButtonDown += OnPrevious;
            _Next.MouseLeftButtonDown += OnNext;
            _Title.MouseLeftButtonDown += OnNow;

            UpdateElements();
        }

    }
}
