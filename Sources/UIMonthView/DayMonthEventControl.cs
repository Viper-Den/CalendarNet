using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Converters;
using Destiny.Core;

namespace MonthEvent
{
    [TemplatePart(Name = DayMonthEventControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DayMonthEventControl.TP_CONTENT, Type = typeof(FrameworkElement))]
    public class DayMonthEventControl : Selector
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
            Events = new ObservableCollectionWithItemNotify<Event>();
        }
        #region ItemTemplate
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DayMonthEventControl), new PropertyMetadata(ItemTemplateChanged));
        public static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).ItemTemplate = (DataTemplate)e.NewValue;
        }
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set {  SetValue(ItemTemplateProperty, value); }
        }
        #endregion
        #region EventsProperty
        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register("Events", typeof(ObservableCollectionWithItemNotify<Event>), typeof(DayMonthEventControl), new PropertyMetadata(OnEventsChanged));
        public ObservableCollectionWithItemNotify<Event> Events
        {
            get { return (ObservableCollectionWithItemNotify<Event>)GetValue(EventsProperty); }
            private set  { SetValue(EventsProperty, value); }
        }
        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).Events = (ObservableCollectionWithItemNotify<Event>)e.NewValue;
        }
        #endregion
        #region DateProperty
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DayMonthEventControl), new PropertyMetadata(DatePropertyChanged));
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
                UpdateEvents();
            }
        }
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).Date = (DateTime)e.NewValue;
        }
        #endregion
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
            _Content.ItemTemplate = ItemTemplate;
            _Title.MouseLeftButtonDown += OnAddEvent;
            UpdateEvents();
        }
        private void OnAddEvent(object sender, MouseButtonEventArgs e)
        {
            AddAction?.Invoke(Date);
        }

    }
}

