using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UIMonthControl;

namespace UIYearControl
{
    [TemplatePart(Name = YearControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = YearControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    public class YearControl : Control
    {
   
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private Grid _MainGrid;
        private Label _Title;
        private DateTime _Date;
        private List<MonthControl> _Month;

        public YearControl()
        {
            _Month = new List<MonthControl>();
            _Date = DateTime.Now;
        }
        static YearControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(YearControl), new FrameworkPropertyMetadata(typeof(YearControl)));
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(YearControl), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).Date = new DateTime(((DateTime)e.NewValue).Year, ((DateTime)e.NewValue).Month, 1);
        }

        public DateTime Date
        {
            get { return _Date; }
            set
            {
                var d = new DateTime(value.Year, 1, 1);
                SetValue(DateProperty, d);
                if (_Title != null)
                {
                    _Date = d;
                    UpdateElements();
                }
            }
        }

        private void UpdateElements()
        {
            _Title.Content = _Date.ToString("yyyy");
            var startDay = _Date;
            foreach (var m in _Month)
            {
                m.Date = startDay;
                startDay = startDay.AddMonths(1);
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _MainGrid = (Grid)GetTemplateChild(TP_MAIN_GRID_PART);
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);

            for (int y = 1; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    var m = new MonthControl()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    Grid.SetColumn(m, x);
                    Grid.SetRow(m, y);
                    m.Margin = new Thickness(10, 10, 10, 10);
                    m.ViewButtons = Visibility.Hidden;
                    m.ViewBorderingMonths = Visibility.Hidden;
                    m.ColorDayFinish = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    m.ColorDayOff = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    m.ColorDayOffFinish = new SolidColorBrush(Color.FromRgb(210, 210, 210));
                    m.ColorToDay = new SolidColorBrush(Color.FromRgb(17, 110, 190));
                    _MainGrid.Children.Add(m);
                    _Month.Add(m);
                }
            }
            UpdateElements();
        }

    }
}
