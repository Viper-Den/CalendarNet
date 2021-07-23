using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UIMonthControl
{
    [TemplatePart(Name = DayControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    public class DayControl : Control
    {
        private Label _Title;
        private SolidColorBrush _BaseColor;
        private const string TP_TITLE_PART = "xTitle";
        static DayControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayControl), new FrameworkPropertyMetadata(typeof(DayControl)));
        }
        public DayControl()
        {
        }
        ~ DayControl()
        {
            MouseDown -= OnMouseDown;
            MouseEnter -= DoMouseEnter;
            MouseLeave -= DoMouseLeave;
            MouseUp -= OnMouseUp;
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DayControl), new PropertyMetadata(DatePropertyChanged));


        public static readonly DependencyProperty DateStartSelectedProperty =
            DependencyProperty.Register(" DateStartSelected", typeof(ICommand), typeof(DayControl), new PropertyMetadata(DateStartSelectedPropertyChanged));
        public static readonly DependencyProperty DateSelectedProperty =
            DependencyProperty.Register("DateSelected", typeof(ICommand), typeof(DayControl), new PropertyMetadata(DateSelectedChanged));
        public static readonly DependencyProperty DateFinishSelectedProperty =
            DependencyProperty.Register("DateFinishSelected", typeof(ICommand), typeof(DayControl), new PropertyMetadata(DateFinishSelectedPropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayControl)d).Date = (DateTime)e.NewValue;
        }
        public static void DateStartSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayControl)d).DateStartSelected = (ICommand)e.NewValue;
        }
        public static void DateSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayControl)d).DateSelected = (ICommand)e.NewValue;
        }
        public static void DateFinishSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayControl)d).DateFinishSelected = (ICommand)e.NewValue;
        }
        public ICommand DateStartSelected
        {
            get { return (ICommand)GetValue(DateStartSelectedProperty); }
            set { SetValue(DateStartSelectedProperty, value); }
        }
        public ICommand DateFinishSelected
        {
            get { return (ICommand)GetValue(DateFinishSelectedProperty); }
            set { SetValue(DateFinishSelectedProperty, value); }
        }
        public ICommand DateSelected
        {
            get { return (ICommand)GetValue(DateSelectedProperty); }
            set { SetValue(DateSelectedProperty, value); }
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
                UpdateElement();
            }
        }
        private void UpdateElement()
        {
            if (_Title == null) { return; }
            _Title.Content = Date.ToString("dd");
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _BaseColor = (SolidColorBrush)Background;
            MouseDown  += OnMouseDown;
            MouseEnter += DoMouseEnter;
            MouseLeave += DoMouseLeave;
            MouseUp    += OnMouseUp;
            UpdateElement();
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DateStartSelected?.Execute(Date);
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            DateFinishSelected?.Execute(Date);
        }
        private void DoMouseEnter(object sender, MouseEventArgs e)
        {
            DateSelected?.Execute(Date);
            _BaseColor = (SolidColorBrush)Background;
        }
        private void DoMouseLeave(object sender, MouseEventArgs e)
        {
            Background = _BaseColor;
        }

    }
}
