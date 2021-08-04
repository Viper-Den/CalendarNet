using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace UIMonthControl
{
    [TemplatePart(Name = DatePicker.TP_DATE_PICKER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DatePicker.TP_POPUP, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DatePicker.TP_BUTTON, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DatePicker.TP_MONTH_CONTROL, Type = typeof(FrameworkElement))]
    public class DatePicker : Control
    {
        private DateBox _DateBox;
        private Popup _Popup;
        private Button _Button;
        private MonthControl _MonthControl; 
        private const string TP_DATE_PICKER = "xDatePicker";
        private const string TP_POPUP = "xPopup";
        private const string TP_BUTTON = "xButton";
        private const string TP_MONTH_CONTROL = "xMonthControl"; 
        static DatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePicker), new FrameworkPropertyMetadata(typeof(DatePicker)));
        }
        ~DatePicker()
        {
            if (_Button != null)
                _Button.Click -= DoClick;
            if (_Popup != null)
                _Popup.LostFocus -= DoLostFocus;
            if (_MonthControl != null)
                _MonthControl.PeriodStart -= DoPeriodStart;
        }

        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DatePicker), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DatePicker)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
                if (_DateBox != null)
                    _DateBox.Date = value;
            }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _DateBox = (DateBox)GetTemplateChild(TP_DATE_PICKER);
            _Popup = (Popup)GetTemplateChild(TP_POPUP);
            _Button = (Button)GetTemplateChild(TP_BUTTON);
            _MonthControl = (MonthControl)GetTemplateChild(TP_MONTH_CONTROL);
            _MonthControl.PeriodStart += DoPeriodStart;
            _Button.Click += DoClick;
        }
        public void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if ((listView != null) &&(listView.SelectedItem != null))
                _DateBox.Text = ((TimeItem)listView.SelectedItem).Name;
        }
        private void DoClick(object sender, RoutedEventArgs e)
        {
            _Popup.IsOpen = !_Popup.IsOpen;
        }
        private void DoLostFocus(object sender, RoutedEventArgs e)
        {
            _Popup.IsOpen = false;
        }
        private void DoPeriodStart(DateTime date)
        {
            _DateBox.Date = date;
            _Popup.IsOpen = false;
        }
    }
}
