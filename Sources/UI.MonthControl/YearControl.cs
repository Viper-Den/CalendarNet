using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Destiny.Core;


namespace UIMonthControl
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
        private DateTime _startDate;
        private Boolean _selectMode;
        static YearControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(YearControl), new FrameworkPropertyMetadata(typeof(YearControl)));
        }
        public YearControl()
        {
            _Month = new List<MonthControl>();
            Pallete = new PalleteYear();
            MouseLeave += DoMouseLeave;
        }
        ~YearControl()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
            _Title.MouseLeftButtonDown -= OnNow;
        }

        #region PeriodStart
        public static readonly DependencyProperty PeriodStartProperty =
            DependencyProperty.Register("PeriodStart", typeof(ICommand), typeof(YearControl), new PropertyMetadata(PeriodStartChanged));
        public static void PeriodStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).PeriodStart = (ICommand)e.NewValue;
        }
        public ICommand PeriodStart
        {
            get { return (ICommand)GetValue(PeriodStartProperty); }
            set { SetValue(PeriodStartProperty, value); }
        }
        #endregion

        #region PeriodFinish
        public static readonly DependencyProperty PeriodFinishProperty =
            DependencyProperty.Register("PeriodFinish", typeof(ICommand), typeof(YearControl), new PropertyMetadata(PeriodFinishPropertyChanged));
        public static void PeriodFinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).PeriodFinish = (ICommand)e.NewValue;
        }
        public ICommand PeriodFinish
        {
            get { return (ICommand)GetValue(PeriodFinishProperty); }
            set { SetValue(PeriodFinishProperty, value); }
        }
        #endregion

        #region PeriodSelected
        public static readonly DependencyProperty PeriodSelectedProperty =
            DependencyProperty.Register(nameof(PeriodSelected), typeof(ICommand), typeof(YearControl), new PropertyMetadata(PeriodSelectedPropertyChanged));
        public static void PeriodSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).PeriodSelected = (ICommand)e.NewValue;
        }
        public ICommand PeriodSelected
        {
            get { return (ICommand)GetValue(PeriodSelectedProperty); }
            set { SetValue(PeriodSelectedProperty, value); }
        }
        #endregion

        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(YearControl), new PropertyMetadata(DatePropertyChanged));
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, new DateTime(value.Year, 1, 1));
                if (_Title != null)
                    UpdateElements();
            }
        }
        #endregion

        #region SelectedDates
        public static readonly DependencyProperty SelectedDatesProperty =
            DependencyProperty.Register(nameof(SelectedDates), typeof(ObservableCollection<DateTime>), typeof(YearControl), new PropertyMetadata(SelectedDatesChanged));
        public static void SelectedDatesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).SelectedDates = (ObservableCollection<DateTime>)e.NewValue;
        }
        public ObservableCollection<DateTime> SelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty); }
            set
            {
                SetValue(SelectedDatesProperty, value);
                UpdateSelectedDates();
            }
        }
        private void UpdateSelectedDates()
        {
            foreach (var m in _Month)
            {
                m.SelectedDates = SelectedDates;
            }
        }
        #endregion

        #region Pallete
        public static readonly DependencyProperty PalleteProperty =
            DependencyProperty.Register(nameof(Pallete), typeof(Pallete), typeof(YearControl), new PropertyMetadata(PalleteChanged));
        private static void PalleteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((YearControl)d).Pallete = (Pallete)e.NewValue;
        }
        public Pallete Pallete
        {
            get { return (Pallete)GetValue(PalleteProperty); }
            set
            {
                SetValue(PalleteProperty, value);
                UpdatePallete();
            }
        }

        private void UpdatePallete()
        {
            foreach (var m in _Month)
            {
                m.Pallete = Pallete;
            }
        }
        #endregion
        private void UpdateElements()
        {
            _Title.Content = Date.ToString("yyyy");
            var startDay = Date;
            foreach (var m in _Month)
            {
                m.Date = startDay;
                startDay = startDay.AddMonths(1);
            }
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
        private void OnNow(object sender, MouseButtonEventArgs e)
        {
            Date = DateTime.Now;
        }
        private void OnPeriodStart(DateTime date)
        {
            _startDate = date;
            _selectMode = true;
            PeriodStart?.Execute(date);
        }

        private void DoMouseLeave(object sender, MouseEventArgs e)
        {
            _selectMode = false;
        }
        private void DoDayEnter(DateTime date)
        {
            if (!_selectMode)
                return;

            var l = new List<DateTime>();
            DateTime startDate = _startDate;
            DateTime finishDate = date;
            if (_startDate.Date > date.Date)
            {
                finishDate = _startDate;
                startDate = date;
            }
            while (startDate.Date <= finishDate.Date)
            {
                l.Add(startDate);
                startDate = startDate.AddDays(1);
            }
            foreach (var m in _Month)
            {
                m.SelectDate(l);
            }
        }
        private void OnPeriodFinish(DateTime date)
        {
            _selectMode = false;
            var l = new List<DateTime>();
            DateTime finishDate = date;
            if (_startDate.Date > date.Date)
            {
                finishDate = _startDate;
                _startDate = date;
            }
            DateTime d = _startDate;
            while (d.Date <= finishDate.Date)
            {
                l.Add(d);
                d = d.AddDays(1);
            }
            PeriodFinish?.Execute(date);
            PeriodSelected?.Execute(l);
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
            _Title.MouseLeftButtonDown += OnNow;

            for (int y = 1; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    var m = new MonthControl()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(10, 10, 10, 10),
                        SelectedDates = SelectedDates,
                        Pallete = Pallete
                    };
                    Grid.SetColumn(m, x);
                    Grid.SetRow(m, y);
                    m.PeriodStart += OnPeriodStart;
                    m.PeriodFinish += OnPeriodFinish;
                    m.DayEnter += DoDayEnter;
                    _MainGrid.Children.Add(m);
                    _Month.Add(m);
                }
            }
            UpdateElements();
            UpdateSelectedDates();
            UpdatePallete();
        }
    }
}
