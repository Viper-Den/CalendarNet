using System;
using System.Windows;
using System.Windows.Controls;

namespace UIMonthControl
{
    [TemplatePart(Name = DayControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    public class DayControl : Control
    {
        private Label _Title;
        private const string TP_TITLE_PART = "xTitle";
        static DayControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayControl), new FrameworkPropertyMetadata(typeof(DayControl)));
        }

        public static readonly DependencyProperty DateProperty = 
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DayControl), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayControl)d).Date = (DateTime)e.NewValue;
        }

        private void UpdateElement()
        {
           _Title.Content = Date.ToString("dd");
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
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            UpdateElement();
        }

    }
}
