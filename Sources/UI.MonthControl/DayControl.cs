using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Destiny.Core;

namespace UIMonthControl
{

    [TemplatePart(Name = DayControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    public class DayControl : Control, IDayControl
    {
        private Label _Title;
        private const string TP_TITLE_PART = "xTitle";
        static DayControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayControl), new FrameworkPropertyMetadata(typeof(DayControl)));
        }
        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DayControl), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayControl)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
                Type = Pallete.GetDateType(value);
                UpdateElement();
            }
        }
        #endregion

        public DayType Type {private set; get;}
        public bool IsSelected { get; set; }
        private void UpdateElement()
        {
            if (_Title == null) 
                return; 
            _Title.Content = Date.ToString("dd");
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            UpdateElement();
        }

    }
}
