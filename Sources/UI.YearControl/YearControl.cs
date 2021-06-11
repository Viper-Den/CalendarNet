using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UIMonthControl;

namespace UIYearControl
{
    [TemplatePart(Name = YearControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = YearControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = YearControl.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = YearControl.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    public class YearControl : Control
    {
   
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext";
        private Grid _MainGrid;
        private Label _Title;
        private Label _Previous;
        private Label _Next;
        private List<MonthControl> _Month;

        public YearControl()
        {
            _Month = new List<MonthControl>();
        }
        static YearControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(YearControl), new FrameworkPropertyMetadata(typeof(YearControl)));
        }

        public static readonly DependencyProperty DateRangesProperty =
           DependencyProperty.Register("DateRanges", typeof(ObservableCollection<IDateRange>),
               typeof(YearControl), new PropertyMetadata(OnDateRangesChanged));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(YearControl), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).Date = (DateTime)e.NewValue;
        }
        private static void OnDateRangesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).DateRanges = (ObservableCollection<IDateRange>)e.NewValue;
        }

        public ObservableCollection<IDateRange> DateRanges
        {
            get { return (ObservableCollection<IDateRange>)GetValue(DateRangesProperty); }
            set
            {
                SetValue(DateRangesProperty, value);
                if (value != null)
                {
                    SelectRanges();
                }
            }
        }
        private void SelectRanges()
        {
            foreach (var m in _Month)
            {
                m.DateRanges = DateRanges;
            }
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, new DateTime(value.Year, 1, 1));
                if (_Title != null)
                {
                    UpdateElements();
                }
            }
        }
        private void UpdateElements()
        {
            _Title.Content = Date.ToString("yyyy");
            var startDay = Date;
            foreach (var m in _Month)
            {
                m.Date = startDay;
                startDay = startDay.AddMonths(1);
            }
            SelectRanges();
        }
        private void OnPrevious(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MinValue) { return; }
            Date = Date.AddYears(-1);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MaxValue) { return; }
            Date = Date.AddYears(1);
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
                    _MainGrid.Children.Add(m);
                    _Month.Add(m);
                }
            }
            UpdateElements();
        }

    }
}
