using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UIDayControl;
using System.Windows.Input;
using System.Windows.Media;

namespace UIMonthControl
{
    [TemplatePart(Name = MonthControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    public class MonthControl : Control
    {
   
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext"; 
        private Grid _MainGrid;
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private DateTime _Date;
        private Boolean _DisplayborderingMonths;
        private List<DayControl> _Days;
        private List<Label> _TitleDays;
        ~MonthControl()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
        }
        public MonthControl()
        {
            _Days = new List<DayControl>();
            _TitleDays = new List<Label>();
            _Date = DateTime.Now;
        }
        static MonthControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthControl), new FrameworkPropertyMetadata(typeof(MonthControl)));
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MonthControl), new PropertyMetadata(DatePropertyChanged));

        public static readonly DependencyProperty DisplayborderingMonthsProperty =
            DependencyProperty.Register("DisplayborderingMonths", typeof(Boolean), typeof(MonthControl), new PropertyMetadata(DisplayborderingMonthsPropertyChanged));
        
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           ((MonthControl)d).Date = (DateTime)e.NewValue;
        }
        public static void DisplayborderingMonthsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           ((MonthControl)d).DisplayborderingMonths = (Boolean)e.NewValue;
        }
        


        public DateTime Date
        {
            get { return _Date; }
            set
            {
                var d = new DateTime(value.Year, value.Month, 1);
                SetValue(DateProperty, d);
                if (_Title != null)
                {
                    _Date = d;
                    UpdateElements();
                }
            }
        }
        public Boolean DisplayborderingMonths
        {
            get { return _DisplayborderingMonths; }
            set
            {
                SetValue(DisplayborderingMonthsProperty, value);
                if (_Title != null)
                {
                    _DisplayborderingMonths = value;
                    UpdateElements();
                }
            }
        }

        private void UpdateElements() 
        {
            _Title.Content = Date.ToString("MMMM yyyy");
            var dayOfWeek = (int)Date.DayOfWeek;
            if(dayOfWeek == 0) { dayOfWeek = 6; } // to-do change to cultures selected mode
            else { dayOfWeek--; }


            var startDay = Date.AddDays(-dayOfWeek);
            foreach (var d in _TitleDays)
            {
                d.Content = startDay.ToString("ddd");
                startDay = startDay.AddDays(1);
            }
            startDay = Date.AddDays(-dayOfWeek);
            foreach (var d in _Days)
            {
                d.Date = startDay;
                if ((d.Date.Month != Date.Month) && (!_DisplayborderingMonths))
                {
                    d.Visibility = Visibility.Hidden;
                }
                else
                {
                    d.Visibility = Visibility.Visible;
                }
                startDay = startDay.AddDays(1);
            }
        }
        private void OnPrevious(object sender, MouseButtonEventArgs e)
        {
            Date = Date.AddMonths(-1);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            Date = Date.AddMonths(1);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _MainGrid = (Grid)GetTemplateChild(TP_MAIN_GRID_PART);
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _Previous = (Label)GetTemplateChild(TP_PREVIOUS_PART);
            _Next = (Label)GetTemplateChild(TP_NEXT_PART);

            _Previous.Content = "<";
            _Next.Content = ">";
            _Previous.MouseLeftButtonDown += OnPrevious;
            _Next.MouseLeftButtonDown += OnNext;

            if (!_DisplayborderingMonths)
            {
                _Previous.Visibility = Visibility.Hidden;
                _Next.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(_Title, 7);
            }
            else
            {
                _Previous.Visibility = Visibility.Visible;
                _Next.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(_Title, 5);
            }

            for (int x = 0; x < 7; x++) // Second - for -day title
            {
                var d = new Label()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };
                Grid.SetColumn(d, x);
                Grid.SetRow(d, 1);
                _MainGrid.Children.Add(d);
                _TitleDays.Add(d);
            }

            for (int y = 2; y < 8; y++) // first for - title; Second - for - day title
            {
                for (int x = 0; x < 7; x++)
                {
                    var d = new DayControl()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    Grid.SetColumn(d, x);
                    Grid.SetRow(d, y);
                    _MainGrid.Children.Add(d);
                    _Days.Add(d);
                    }
            }
            UpdateElements();
        }

    }
}
