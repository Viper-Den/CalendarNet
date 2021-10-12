using Destiny.UI.Controls;
using System;
using System.Collections.Generic;
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
        private Border _Button;
        
        private MonthControl _MonthControl;
        private ObservableCollection<DateTime> _selectedDates;
        private const string TP_DATE_PICKER = "xDatePicker";
        private const string TP_POPUP = "xPopup";
        private const string TP_BUTTON = "xButton";
        private const string TP_MONTH_CONTROL = "xMonthControl"; 
        static DatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePicker), new FrameworkPropertyMetadata(typeof(DatePicker)));
        }
        public DatePicker()
        {
            _selectedDates = new ObservableCollection<DateTime>();
        }

        private void DatePicker_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _Popup.IsOpen = false;
        }

        ~DatePicker()
        {
            if (_Button != null)
                _Button.PreviewMouseDown -= DoClick;
            //if (_Popup != null)
            //    _Popup.LostFocus -= DoLostFocus;
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
                {
                    _DateBox.OnDateChanged -= DoDateChanged;
                    _DateBox.Date = value;
                    SetDateView(Date);
                    _DateBox.OnDateChanged += DoDateChanged;
                }
            }
        }

        private void SetDateView(DateTime time)
        {
            if (_MonthControl == null)
                return;
            _MonthControl.Date = Date;
            _selectedDates.Clear();
            _selectedDates.Add(Date);
        }
        private void DoDateChanged(DateBox obj)
        {
            SetValue(DateProperty, obj.Date);
            SetDateView(Date);
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _DateBox = (DateBox)GetTemplateChild(TP_DATE_PICKER);

            _Popup = (Popup)GetTemplateChild(TP_POPUP);

            _MonthControl = (MonthControl)GetTemplateChild(TP_MONTH_CONTROL);
            _MonthControl.SelectedDates = _selectedDates;
            _MonthControl.PeriodStart += DoPeriodStart;

            _Button = (Border)GetTemplateChild(TP_BUTTON);
            _Button.PreviewMouseUp += DoClick;
            _Button.Focusable = true;
            _Button.FocusableChanged += DatePicker_FocusableChanged;
            SetDateView(Date);
        }
        private void DoClick(object sender, RoutedEventArgs e)
        {
            _Popup.IsOpen = !_Popup.IsOpen;
            if (_Popup.IsOpen)
                _Popup.Focus();
        }
        //private void DoLostFocus(object sender, RoutedEventArgs e)
        //{
        //    _Popup.IsOpen = false;
        //}
        private void DoPeriodStart(DateTime date)
        {
            Date = date;
            _Popup.IsOpen = false;
        }
    }
}
