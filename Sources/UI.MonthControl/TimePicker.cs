using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace UIMonthControl
{
    [TemplatePart(Name = TimePicker.TP_GRID, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER_POPUP, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER_BUTTON, Type = typeof(FrameworkElement))]
    public class TimePicker : Control
    {
        private Grid _grid;
        private TextBox _TimePicker;
        private Popup _TimePickerPopup;
        private Button _TimePickerButton;
        private const string TP_GRID = "xGrid";
        private const string TP_TITLE_PICKER = "xTimePicker";
        private const string TP_TITLE_PICKER_POPUP = "xTimePickerPopup"; 
        private const string TP_TITLE_PICKER_BUTTON = "xTimePickerButton";
        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
        }
        ~ TimePicker()
        {
            if (_TimePickerButton != null)
                _TimePickerButton.Click -= DoClick;
            if (_TimePicker != null)
                _TimePicker.KeyDown -= DoKeyDown;
            if (_TimePickerPopup != null)
                _TimePickerPopup.LostFocus -= DoLostFocus;
        }

        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(TimePicker), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TimePicker)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
            }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _grid = (Grid)GetTemplateChild(TP_GRID);
            _TimePicker = (TextBox)GetTemplateChild(TP_TITLE_PICKER);
            _TimePickerPopup = (Popup)GetTemplateChild(TP_TITLE_PICKER_POPUP);
            _TimePickerButton = (Button)GetTemplateChild(TP_TITLE_PICKER_BUTTON);
            _TimePickerButton.Click += DoClick;
            _TimePicker.KeyDown += DoKeyDown;
            _TimePickerPopup.LostFocus += DoLostFocus;
        }
        private void DoClick(object sender, RoutedEventArgs e)
        {
            _TimePickerPopup.IsOpen = !_TimePickerPopup.IsOpen;
            if (_TimePickerPopup.IsOpen)
                FocusManager.SetFocusedElement(_TimePickerPopup, _TimePickerPopup);
        }

        private void DoLostFocus(object sender, RoutedEventArgs e) 
        {
            _TimePickerPopup.IsOpen = false;
        }
        public void DoKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
