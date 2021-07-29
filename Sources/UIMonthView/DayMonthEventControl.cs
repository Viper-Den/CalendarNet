using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MonthEvent
{
    [TemplatePart(Name = DayMonthEventControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DayMonthEventControl.TP_CONTENT_PART, Type = typeof(FrameworkElement))]
    public class DayMonthEventControl : Control
    {

        private Label _Title;
        private StackPanel _Content;
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_CONTENT_PART = "xContent";
        static DayMonthEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayMonthEventControl), new FrameworkPropertyMetadata(typeof(DayMonthEventControl)));
        }

        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register("Events", typeof(ObservableCollection<IEvent>),
               typeof(DayMonthEventControl), new PropertyMetadata(OnEventsChanged));


        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DayMonthEventControl), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).Date = (DateTime)e.NewValue;
        }
        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).Events = (ObservableCollection<IEvent>)e.NewValue;
        }

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
                UpdateEvents();
            }
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
                if (Date == new DateTime(Date.Year, Date.Month, 1))
                {
                    _Title.Content = Date.ToString("MM.dd");
                }
                else
                {
                    _Title.Content = Date.ToString("dd");
                } 
            }
            if (Events == null) { return; }

            _Content.Children.Clear();
            foreach (var e in Events)
            {
                if (Date == e.Start)
                {
                    var l = new Label();
                    l.Height = 25;
                    l.Content = e.Caption;
                    l.Background = e.Color;
                    _Content.Children.Add(l);
                }
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _Content = (StackPanel)GetTemplateChild(TP_CONTENT_PART);
            _Content.MouseLeftButtonDown += OnAddEvent;
            _Title.MouseLeftButtonDown += OnAddEvent;
            UpdateEvents();
        }
        public Action<DateTime> AddAction { get; set; }

        private void OnAddEvent(object sender, MouseButtonEventArgs e)
        {
            AddAction?.Invoke(Date);
        }

    }
}

