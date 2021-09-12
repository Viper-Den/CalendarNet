using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UIMonthControl
{
    public class TimeItem
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }


    [TemplatePart(Name = TimePicker.TP_CANVAS, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_POPUP, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_BUTTON, Type = typeof(FrameworkElement))]
    public class TimePicker : Control
    {
        private const int HOURS_STEP = 1;
        private const int HOURS_STEPS = 24 / HOURS_STEP;
        private const int MINUTES_STEP = 5;
        private const int MINUTES_STEPS = 60 / MINUTES_STEP;
        private Canvas _canvas;
        private TimeBox _TimePicker;
        private Popup _TimePickerPopup;
        private Button _TimePickerButton;
        private Line _line;
        private Ellipse _point;
        private Label _SelectedHour;
        private Label _SelectedMinute;
        private Ellipse _SelectedHourElipse;
        private Ellipse _SelectedMinuteElipse;
        private const string TP_CANVAS = "xCanvas";
        private const string TP_TITLE_PICKER = "xTimeBox";
        private const string TP_POPUP = "xPopup";
        private const string TP_BUTTON = "xButton";
        private List<Label> _Hours;
        private List<Label> _Minutes;
        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
        }
        public TimePicker()
        {
            _Hours = new List<Label>();
            _Minutes = new List<Label>();
            DateTime time = new DateTime(2000, 1, 1, 0, 0, 0, 0);
        }
        ~TimePicker()
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
            _canvas = (Canvas)GetTemplateChild(TP_CANVAS);
            _TimePicker = (TimeBox)GetTemplateChild(TP_TITLE_PICKER);
            _TimePickerPopup = (Popup)GetTemplateChild(TP_POPUP);
            _TimePickerButton = (Button)GetTemplateChild(TP_BUTTON);
            _TimePickerButton.Click += DoClick;
            _canvas.SizeChanged += DoSizeChanged;
            for (var i = 0; i < HOURS_STEPS; i++)
            {
                var l = new Label();
                _canvas.Children.Add(l);
                Canvas.SetZIndex(l, 2);
                l.Content = $"{i * HOURS_STEP}";
                l.MouseUp += DoMouseUp;
                l.Background = Brushes.Transparent;
                _Hours.Add(l);
            }
            for (var i = 0; i < MINUTES_STEPS; i++)
            {
                var l = new Label();
                _canvas.Children.Add(l);
                Canvas.SetZIndex(l, 2);
                l.Content = $"{i * MINUTES_STEP}";
                l.Background = Brushes.Transparent;
                l.MouseUp += DoMouseUp;
                _Minutes.Add(l);
            }

            _SelectedHour = _Hours[0];
            _SelectedMinute = _Minutes[0];

            _line = new Line();
            _canvas.Children.Add(_line);
            Canvas.SetZIndex(_line, 0);
            _point = new Ellipse();
            _canvas.Children.Add(_point);
            Canvas.SetZIndex(_point, 0);
            _line.Stroke = Brushes.AliceBlue;
            _point.Fill = Brushes.Gray;


            _SelectedHourElipse = new Ellipse();
            _SelectedHourElipse.Fill = Brushes.Gray;
            _canvas.Children.Add(_SelectedHourElipse);
            Canvas.SetZIndex(_SelectedHourElipse, 1);
            _SelectedMinuteElipse = new Ellipse();
            _SelectedMinuteElipse.Fill = Brushes.Gray;
            _canvas.Children.Add(_SelectedMinuteElipse);
            Canvas.SetZIndex(_SelectedMinuteElipse, 1);
            SetPositionHours();
            SetPositionMinutes();
            if (_TimePicker != null)
                _TimePicker.Time = Time;
        }
        private void DoMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Label))
                return;

            if (_Hours.Contains(sender as Label))
            {
                var h = int.Parse((sender as Label).Content.ToString());
                Time = new DateTime(Time.Year, Time.Month, Time.Day, h, Time.Minute, 0);
            }
            if (_Minutes.Contains(sender as Label))
            {
                var m = int.Parse((sender as Label).Content.ToString());
                Time = new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, m, 0);
            }

        }
        private void DoSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetPositionHours();
            SetPositionMinutes();
        }
        private void SetPositionHours()
        {
            double angl = -180;
            var anglstep = 360 / HOURS_STEPS;
            var r = (_canvas.ActualWidth * 0.6) / 2; // 70%
            var x = _canvas.ActualWidth / 2;
            var y = _canvas.ActualHeight / 2;
            var rad = 180 / Math.PI;
            for (int i = 0; i < HOURS_STEPS; i++)
            {
                var l = _Hours[i];
                var xx = (r * Math.Sin((angl) / rad));
                var yy = (r * Math.Cos(angl / rad));
                Canvas.SetLeft(l, x + xx - (l.ActualWidth / 2));
                Canvas.SetTop(l, y + yy - (l.ActualHeight / 2));
                angl -= anglstep;
            }
            _point.Width = 10;
            _point.Height = 10;
            Canvas.SetLeft(_point, x - (_point.Width / 2));
            Canvas.SetTop(_point, y - (_point.Height / 2));
            _SelectedHour.Foreground = Brushes.White;
            _SelectedHourElipse.Width = _SelectedHour.ActualWidth;
            _SelectedHourElipse.Height = _SelectedHour.ActualWidth;
            Canvas.SetLeft(_SelectedHourElipse, Canvas.GetLeft(_SelectedHour));
            Canvas.SetTop(_SelectedHourElipse, Canvas.GetTop(_SelectedHour) - ((_SelectedHourElipse.Height - _SelectedHour.ActualHeight) / 2));
        }
        private void SetPositionMinutes()
        {
            double angl = 180;
            var anglstep = 360 / MINUTES_STEPS;
            var r = (_canvas.ActualWidth * 0.3) / 2; // 70%
            var x = _canvas.ActualWidth / 2;
            var y = _canvas.ActualHeight / 2;
            var rad = 180 / Math.PI;
            for (int i = 0; i < MINUTES_STEPS; i++)
            {
                var l = _Minutes[i];
                var xx = (r * Math.Sin((angl) / rad));
                var yy = (r * Math.Cos(angl / rad));
                Canvas.SetLeft(l, x + xx - (l.ActualWidth / 2));
                Canvas.SetTop(l, y + yy - (l.ActualHeight / 2));
                angl -= anglstep;
            }
            _point.Width = 10;
            _point.Height = 10;
            Canvas.SetLeft(_point, x - (_point.Width / 2));
            Canvas.SetTop(_point, y - (_point.Height / 2));

            _SelectedMinute.Foreground = Brushes.White;
            _SelectedMinuteElipse.Width = _SelectedMinute.ActualWidth;
            _SelectedMinuteElipse.Height = _SelectedMinute.ActualWidth;
            Canvas.SetLeft(_SelectedMinuteElipse, Canvas.GetLeft(_SelectedMinute));
            Canvas.SetTop(_SelectedMinuteElipse, Canvas.GetTop(_SelectedMinute) - ((_SelectedMinuteElipse.Height - _SelectedMinute.ActualHeight) / 2));
        }

        private void DoClick(object sender, RoutedEventArgs e)
        {
            _TimePickerPopup.IsOpen = !_TimePickerPopup.IsOpen;
        }
        private void DoLostFocus(object sender, RoutedEventArgs e)
        {
            _TimePickerPopup.IsOpen = false;
        }
    }
}
