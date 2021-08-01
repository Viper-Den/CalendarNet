using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace UIMonthControl
{
    [TemplatePart(Name = TimePicker.TP_LIST_VIEW, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER_POPUP, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER_BUTTON, Type = typeof(FrameworkElement))]
    public class TimePicker : Control
    {
        private ListView _ListView;
        private TextBox _TimePicker;
        private Popup _TimePickerPopup;
        private Button _TimePickerButton;
        private const string TP_LIST_VIEW = "xListView";
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
            if (_TimePickerPopup != null)
                _TimePickerPopup.LostFocus -= DoLostFocus;
            if (_TimePicker != null)
                _TimePicker.PreviewTextInput -= OnPreviewTextInput;
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
            _ListView = (ListView)GetTemplateChild(TP_LIST_VIEW);
            _TimePicker = (TextBox)GetTemplateChild(TP_TITLE_PICKER);
            _TimePickerPopup = (Popup)GetTemplateChild(TP_TITLE_PICKER_POPUP);
            _TimePickerButton = (Button)GetTemplateChild(TP_TITLE_PICKER_BUTTON);
            _TimePickerButton.Click += DoClick;
            _ListView.LostFocus += DoLostFocus;
            _TimePicker.PreviewTextInput += OnPreviewTextInput;
        }
        private void DoClick(object sender, RoutedEventArgs e)
        {
            _TimePickerPopup.IsOpen = !_TimePickerPopup.IsOpen;
            if (_TimePickerPopup.IsOpen)
                _ListView.Focus();
        }
        private void DoLostFocus(object sender, RoutedEventArgs e) 
        {
            _TimePickerPopup.IsOpen = false;
        }
        private static readonly Regex _regex = new Regex(@"\d{2,2}:\d{2,2}"); //regex that matches disallowed text
        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            //var fullText = _TimePicker.Text.Insert(_TimePicker.SelectionStart, e.Text);
            //e.Handled = _regex.IsMatch(fullText);
        }
    }
}
