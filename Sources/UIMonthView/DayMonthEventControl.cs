using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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


        private void UpdateElement()
        {
            if (Date == new DateTime(Date.Year, Date.Month, 1))
            {
                _Title.Content = Date.ToString("MM.dd");
            }
            else
            {
                _Title.Content = Date.ToString("dd");
            }
            UpdateEvents();
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
                if (_Title != null)
                {
                    UpdateElement();
                }
            }
        }
        public void UpdateEvents()
        {
            if(Events == null) { return; }

            _Content.Children.Clear();
            foreach (var e in Events)
            {
                if (Date == e.Date)
                {
                    var l = new Label();
                    l.Height = 25;
                    l.Content = e.Caption;
                    l.Background = e.Color;
                    _Content.Children.Add(l);

                }
            }
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
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _Content = (StackPanel)GetTemplateChild(TP_CONTENT_PART);
            UpdateElement();
        }

    }
}

