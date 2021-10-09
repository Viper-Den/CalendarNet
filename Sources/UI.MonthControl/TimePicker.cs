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
using System.Windows.Shapes;

namespace UIMonthControl
{
    public class TimeElementControl
    {
        private Ellipse _elipse;
        private Label _label;
        private bool _isSelected;
        public TimeElementControl(Canvas canvas, string val)
        {
            _label = new Label();
            canvas.Children.Add(_label);
            Canvas.SetZIndex(_label, 2);
            Name = val;
            _label.Background = Brushes.Transparent;
            _label.PreviewMouseUp += DoMouseUp;

            _elipse = new Ellipse();
            canvas.Children.Add(_elipse);
            Canvas.SetZIndex(_elipse, 1);
            IsSelected = false;
        }
        public void SetPosition(double x, double y)
        {
            Canvas.SetLeft(_label, x - (_label.ActualWidth / 2));
            Canvas.SetTop(_label, y - (_label.ActualHeight / 2));
            _elipse.Width = _label.FontSize + 10;
            _elipse.Height = _elipse.Width;
            Canvas.SetLeft(_elipse, x - (_elipse.ActualWidth / 2));
            Canvas.SetTop(_elipse, y - (_elipse.ActualHeight / 2));
        }
        public Point GetPosition()
        {
            return new Point() { X = Canvas.GetLeft(_label) + (_label.ActualWidth / 2), Y = Canvas.GetTop(_label) + (_label.ActualHeight / 2) };
        }
        private void DoMouseUp(object sender, MouseButtonEventArgs e)
        {
            OnSelected?.Invoke(this);
        }
        public string Name { get => _label.Content.ToString(); set => _label.Content = value; }
        public bool IsSelected { 
            get => _isSelected;
            set {
                _isSelected = value;
                if (_isSelected)
                {
                    _label.Foreground = Brushes.White;
                    _elipse.Fill = Brushes.Gray;
                }
                else
                {
                    _label.Foreground = Brushes.Black;
                    _elipse.Fill = Brushes.Transparent;
                }
            } 
        }
        public Action<TimeElementControl> OnSelected { get; set; }
    }

    [TemplatePart(Name = TimePicker.TP_CANVAS, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_TITLE_PICKER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_POPUP, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_BUTTON, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TimePicker.TP_BUTTON_DONE, Type = typeof(FrameworkElement))]
    public class TimePicker : Control
    {
        private const int HOURS_STEP = 1;
        private const int HOURS_STEPS = 24 / HOURS_STEP;
        private const int MINUTES_STEP = 5;
        private const int MINUTES_STEPS = 60 / MINUTES_STEP;
        private Canvas _canvas;
        private TimeBox _TimePicker;
        private Popup _TimePickerPopup;
        private Border _TimePickerButton;
        private Line _lineHour;
        private Line _lineMinute;
        private Ellipse _point;
        private Button _buttonDone;
        private const string TP_CANVAS = "xCanvas";
        private const string TP_TITLE_PICKER = "xTimeBox";
        private const string TP_POPUP = "xPopup";
        private const string TP_BUTTON = "xButton";
        private const string TP_BUTTON_DONE = "xButtonDone";
        private Dictionary<int, TimeElementControl> _Hours;
        private Dictionary<int, TimeElementControl> _Minutes;
        private TimeElementControl _selectedHour;
        private TimeElementControl _selectedMinute;
        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
        }
        public TimePicker()
        {
            _Hours = new Dictionary<int, TimeElementControl>();
            _Minutes = new Dictionary<int, TimeElementControl>();
            DateTime time = new DateTime(2000, 1, 1, 0, 0, 0, 0);
        }
        ~TimePicker()
        {
            if (_TimePickerButton != null)
                _TimePickerButton.PreviewMouseDown -= DoClick;
            if (_TimePickerPopup != null)
                _TimePickerPopup.LostFocus -= DoLostFocus;
        }

        #region Time
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time), typeof(DateTime), typeof(TimePicker), new PropertyMetadata(DatePropertyChanged));

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
                {
                    _TimePicker.OnTimeChanged -= DoTimeChanged;
                    _TimePicker.Time = value;
                    _TimePicker.OnTimeChanged += DoTimeChanged;
                    SetTimeView(Time);
                }
            }
        }
        #endregion

        private void SetTimeView(DateTime time)
        {
            TimeElementControl selectedHour = _Hours[0];
            TimeElementControl selectedMinute = _Minutes[0];

            if (_Hours.ContainsKey(time.Hour))
                selectedHour = _Hours[time.Hour];

            int m = (time.Minute - (time.Minute % MINUTES_STEP));
            if (_Minutes.ContainsKey(m))
                selectedMinute = _Minutes[m];
            Select(selectedHour, selectedMinute);
        }

        private void DoSelectedHour(TimeElementControl element)
        {
            Select(element, _selectedMinute);
            var h = int.Parse(element.Name);
            Time = new DateTime(Time.Year, Time.Month, Time.Day, h, Time.Minute, 0);
        }
        private void DoSelectedMinute(TimeElementControl element)
        {
            Select(_selectedHour, element);
            var m = int.Parse(element.Name);
            Time = new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, m, 0);
        }
        private void Select(TimeElementControl selectedHour, TimeElementControl selectedMinute)
        {
            if(_selectedHour != null)
                _selectedHour.IsSelected = false;
            if (_selectedMinute != null)
                _selectedMinute.IsSelected = false;

            _selectedHour = selectedHour;
            _selectedMinute = selectedMinute;

            _selectedHour.IsSelected = true;
            _selectedMinute.IsSelected = true;

            var p = _selectedHour.GetPosition();
            _lineHour.X1 = (_canvas.ActualWidth / 2);
            _lineHour.Y1 = (_canvas.ActualHeight / 2);
            _lineHour.X2 = p.X;
            _lineHour.Y2 = p.Y;

            p = _selectedMinute.GetPosition();
            _lineMinute.X1 = (_canvas.ActualWidth / 2);
            _lineMinute.Y1 = (_canvas.ActualHeight / 2);
            _lineMinute.X2 = p.X;
            _lineMinute.Y2 = p.Y;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = (Canvas)GetTemplateChild(TP_CANVAS);
            _canvas.SizeChanged += DoSizeChanged;

            _TimePicker = (TimeBox)GetTemplateChild(TP_TITLE_PICKER);
            _TimePicker.OnTimeChanged += DoTimeChanged;

            _TimePickerPopup = (Popup)GetTemplateChild(TP_POPUP);

            _TimePickerButton = (Border)GetTemplateChild(TP_BUTTON);
            _TimePickerButton.PreviewMouseDown += DoClick;

            _buttonDone = (Button)GetTemplateChild(TP_BUTTON_DONE);
            _buttonDone.Click += DoDoneClick;

            for (var i = 0; i < HOURS_STEPS; i++)
            {
                var l = new TimeElementControl(_canvas, $"{i * HOURS_STEP}");
                _Hours.Add(i * HOURS_STEP, l);
                l.OnSelected += DoSelectedHour;
            }
            for (var i = 0; i < MINUTES_STEPS; i++)
            {
                var l = new TimeElementControl(_canvas, $"{i * MINUTES_STEP}");
                _Minutes.Add(i * MINUTES_STEP, l);
                l.OnSelected += DoSelectedMinute;
            }

            _lineHour = new Line();
            _lineHour.StrokeThickness = 3;
            _canvas.Children.Add(_lineHour);
            Canvas.SetZIndex(_lineHour, 0);
            _lineHour.Stroke = Brushes.Gray;

            _lineMinute = new Line();
            _canvas.Children.Add(_lineMinute);
            Canvas.SetZIndex(_lineMinute, 0);
            _lineMinute.Stroke = Brushes.Gray;


            _point = new Ellipse();
            _canvas.Children.Add(_point);
            Canvas.SetZIndex(_point, 0);
            _point.Fill = Brushes.Gray;
            _selectedHour = _Hours[0];
            _selectedMinute = _Minutes[0];
            Update();
            if (_TimePicker != null)
                _TimePicker.Time = Time;
        }

        private void DoTimeChanged(TimeBox obj)
        {
            SetValue(TimeProperty, obj.Time);
            SetTimeView(Time);
        }

        private void DoDoneClick(object sender, RoutedEventArgs e)
        {
            _TimePickerPopup.IsOpen = false;
        }

        private void DoSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            double angl = -180;
            var anglstep = 360 / HOURS_STEPS;
            var r = (_canvas.ActualWidth * 0.8) / 2; // 70%
            var x = _canvas.ActualWidth / 2;
            var y = _canvas.ActualHeight / 2;
            var rad = 180 / Math.PI;
            for (int i = 0; i < HOURS_STEPS; i++)
            {
                var xx = (r * Math.Sin((angl) / rad));
                var yy = (r * Math.Cos(angl / rad));
                _Hours[i * HOURS_STEP].SetPosition(x + xx, y + yy);
                angl -= anglstep;
            }

            angl = 180;
            anglstep = 360 / MINUTES_STEPS;
            r = (_canvas.ActualWidth * 0.45) / 2; // 70%
            x = _canvas.ActualWidth / 2;
            y = _canvas.ActualHeight / 2;
            rad = 180 / Math.PI;
            for (int i = 0; i < MINUTES_STEPS; i++)
            {
                var xx = (r * Math.Sin((angl) / rad));
                var yy = (r * Math.Cos(angl / rad));
                _Minutes[i * MINUTES_STEP].SetPosition(x + xx, y + yy);
                angl -= anglstep;
            }

            _point.Width = 10;
            _point.Height = 10;
            Canvas.SetLeft(_point, x - (_point.Width / 2));
            Canvas.SetTop(_point, y - (_point.Height / 2));
            Select(_selectedHour, _selectedMinute);
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
