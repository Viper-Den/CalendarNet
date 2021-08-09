using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Destiny.Core;
using Converters;

namespace MonthEvent
{
    [TemplatePart(Name = DayMonthEventControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DayMonthEventControl.TP_CONTENT, Type = typeof(FrameworkElement))]
    public class DayMonthEventControl : Control
    {
        private Label _Title;
        private ListView _Content;
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_CONTENT = "xContent";
        static DayMonthEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayMonthEventControl), new FrameworkPropertyMetadata(typeof(DayMonthEventControl)));
        }
        public DayMonthEventControl()
        {
            //this.Resources.Add("BoolToVisibilityUsageConverter", new BoolToVisibilityUsageConverter());
            //Visibility="{Binding Path=Calendar.Enabled, Converter={StaticResource BoolToVisibilityUsageConverter}}"
            Events = new ObservableCollection<IEvent>();
        }

        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register("Events", typeof(ObservableCollection<IEvent>), typeof(DayMonthEventControl), new PropertyMetadata(OnEventsChanged));

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
            private set
            {
                SetValue(EventsProperty, value);
            }
        }
        public Action<DateTime> AddAction { get; set; }
        public void UpdateEvents()
        {
            if (_Title != null) 
            { 
                if (Date == new DateTime(Date.Year, Date.Month, 1))
                    _Title.Content = Date.ToString("MM.dd");
                else
                    _Title.Content = Date.ToString("dd");
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _Title = (Label)GetTemplateChild(TP_TITLE_PART); 
            _Content = (ListView)GetTemplateChild(TP_CONTENT);
            _Content.ItemsSource = Events;
            _Title.MouseLeftButtonDown += OnAddEvent;
            UpdateEvents();
        }
        private void OnAddEvent(object sender, MouseButtonEventArgs e)
        {
            AddAction?.Invoke(Date);
        }

    }
}

