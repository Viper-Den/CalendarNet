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
    public class TimeItem
    {
        public string Name { get; set; }
    }
    [TemplatePart(Name = TimePicker.TP_LIST_VIEW, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_POPUP, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_BUTTON, Type = typeof(FrameworkElement))]
    public class TimePicker : Control
    {
        private ListView _ListView;
        private TimeBox _TimePicker;
        private Popup _TimePickerPopup;
        private Button _TimePickerButton;
        private const string TP_LIST_VIEW = "xListView";
        private const string TP_TITLE_PICKER = "xTimeBox";
        private const string TP_POPUP = "xPopup"; 
        private const string TP_BUTTON = "xButton";
        public ObservableCollection<TimeItem> TimeItems { get; set; }
        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
        }
        public TimePicker()
        {
            TimeItems = new ObservableCollection<TimeItem>();
            DateTime time = new DateTime(2000, 1, 1, 0, 0, 0, 0);
            var finishDayte = time.AddDays(1);
            while (time < finishDayte)
            {
                TimeItems.Add(new TimeItem() { Name = time.ToString("HH:mm") });
                time = time.AddMinutes(15);
            }
        }
        ~ TimePicker()
        {
            if (_TimePickerButton != null)
                _TimePickerButton.Click -= DoClick;
            if (_TimePickerPopup != null)
                _TimePickerPopup.LostFocus -= DoLostFocus;
        }

        #region Time
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(TimePicker), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TimePicker)d).Time = (DateTime)e.NewValue;
        }
        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set
            {
                SetValue(TimeProperty, value);
                if (_TimePicker != null)
                    _TimePicker.Time = value;
            }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _ListView = (ListView)GetTemplateChild(TP_LIST_VIEW);
            _TimePicker = (TimeBox)GetTemplateChild(TP_TITLE_PICKER);
            _TimePickerPopup = (Popup)GetTemplateChild(TP_POPUP);
            _TimePickerButton = (Button)GetTemplateChild(TP_BUTTON);
            _TimePickerButton.Click += DoClick;
            _ListView.LostFocus += DoLostFocus;
            if (_ListView != null)
            {
                _ListView.ItemsSource = TimeItems;
                _ListView.SelectionChanged += SelectionChangedEventHandler;
            }
        }
        public void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if ((listView != null) &&(listView.SelectedItem != null))
                _TimePicker.Text = ((TimeItem)listView.SelectedItem).Name;
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
    }
}
