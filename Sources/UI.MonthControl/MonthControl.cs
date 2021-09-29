using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Destiny.Core;

namespace UIMonthControl
{
    //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.controls.itemscontrol?view=netcore-3.1
    [TemplatePart(Name = MonthControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthControl.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class MonthControl : Control
    {
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext"; 
        private Grid _MainGrid;
        private TitleControl _Title;
        private TitleControl _Previous;
        private TitleControl _Next;
        private List<TitleControl> _TitleDays;
        private List<DateTime> _tempSelection;
        private Dictionary<DateTime, DayControl> _dictionaryDayControl;

        public List<DayControl> Days { get; private set; }
        ~MonthControl()
        {
            if(_Previous != null)
                _Previous.MouseLeftButtonDown -= OnPrevious;
            if (_Next != null)
                _Next.MouseLeftButtonDown -= OnNext;
            if (_Title != null)
                _Title.MouseLeftButtonDown -= OnNow;
        }
        public MonthControl()
        {
            Days = new List<DayControl>();
            Pallete = new Palette();
            _TitleDays = new List<TitleControl>();
            _tempSelection = new List<DateTime>();
            _dictionaryDayControl = new Dictionary<DateTime, DayControl>();

        }
        static MonthControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthControl), new FrameworkPropertyMetadata(typeof(MonthControl)));
        }
        
        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MonthControl),
                new PropertyMetadata(DatePropertyChanged));
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, new DateTime(value.Year, value.Month, 1));
                UpdateElements();
            }
        }
        #endregion
        #region SelectedDates
        public static readonly DependencyProperty SelectedDatesProperty =
            DependencyProperty.Register(nameof(SelectedDates), typeof(ObservableCollection<DateTime>), typeof(MonthControl), new PropertyMetadata(SelectedDatesChanged));
        public static void SelectedDatesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).SelectedDates = (ObservableCollection<DateTime>)e.NewValue;
        }
        public ObservableCollection<DateTime> SelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty); }
            set
            {
                if (SelectedDates != null)
                    SelectedDates.CollectionChanged -= DoSelectedDatesChanged;
                SetValue(SelectedDatesProperty, value);
                if (SelectedDates != null)
                    SelectedDates.CollectionChanged += DoSelectedDatesChanged;
                UpdateSelectedDates();
            }
        }

        private void DoSelectedDatesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case (NotifyCollectionChangedAction.Add):
                    foreach (var d in e.NewItems)
                    {
                        var dt = ((DateTime)d).Date;
                        if (_dictionaryDayControl.ContainsKey(dt))
                        {
                            _dictionaryDayControl[dt].Background = Pallete.Selected.Background;
                            _dictionaryDayControl[dt].Foreground = Pallete.Selected.Foreground;
                        }
                    }
                    break;
                case (NotifyCollectionChangedAction.Remove):
                    foreach (var d in e.OldItems)
                    {
                        var dt = ((DateTime)d).Date;
                        if (_dictionaryDayControl.ContainsKey(dt))
                        {
                            Pallete.PaintDay(_dictionaryDayControl[dt], Date);
                        }
                    }
                    break;
                case (NotifyCollectionChangedAction.Reset):
                    UpdateElements();
                    break;
                default:
                    break;
            }
        }
        private void UpdateSelectedDates()
        {
            if (SelectedDates == null)
                return;
            foreach(var dt in SelectedDates)
            {
                if (_dictionaryDayControl.ContainsKey(dt.Date))
                {
                    Pallete.PaintDay(_dictionaryDayControl[dt.Date], Date);
                }
            }
        }
        #endregion
        #region Pallete
        public static readonly DependencyProperty PalleteProperty =
            DependencyProperty.Register(nameof(Pallete), typeof(Palette), typeof(MonthControl), new PropertyMetadata(PalleteChanged));
        private static void PalleteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthControl)d).Pallete = (Palette)e.NewValue;
        }
        public Palette Pallete
        {
            get { return (Palette)GetValue(PalleteProperty); }
            set
            {
                if (value == null)
                    return;
                SetValue(PalleteProperty, value);
                UpdateElements();
            }
        }
        #endregion

        public void SelectDate(List<DateTime> dates)
        {
            foreach (var oldDate in _tempSelection)
            {
                if ((!dates.Contains(oldDate.Date)) && (_dictionaryDayControl.ContainsKey(oldDate.Date)))
                {
                    Pallete.PaintDay(_dictionaryDayControl[oldDate.Date], Date);
                }
            }
            foreach (var newDate in dates)
            {
                if ((_dictionaryDayControl.ContainsKey(newDate.Date)))
                {
                    _dictionaryDayControl[newDate.Date].Background = Pallete.Selected.Background;
                    _dictionaryDayControl[newDate.Date].Foreground = Pallete.Selected.Foreground;
                    _tempSelection.Add(newDate.Date);
                }
            }
        }
        private void UpdateElements()
        {
            if (_Title == null) 
                return; 
            var dayOfWeek = (int)Date.DayOfWeek;
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (dayOfWeek == 0) 
                dayOfWeek = 6;  // change to DateTimeFormat
            else 
                dayOfWeek--;

            Pallete.PaintTitle(_Title, Date);
            Pallete.PaintTitle(_Next, Date);
            Pallete.PaintTitle(_Previous, Date);


            if (_Previous.Visibility == Visibility.Hidden)
                Grid.SetColumnSpan(_Title, 7);
            else
                Grid.SetColumnSpan(_Title, 5);

            var startDay = Date.AddDays(-dayOfWeek);
            foreach (var t in _TitleDays)
            {
                if ((startDay.DayOfWeek == DayOfWeek.Saturday)||(startDay.DayOfWeek == DayOfWeek.Sunday))
                    t.Type = TitleControlType.WeekTitleDayOff;
                else
                    t.Type = TitleControlType.WeekTitle;
                Pallete.PaintTitle(t, startDay);
                startDay = startDay.AddDays(1);
            }

            startDay = Date.AddDays(-dayOfWeek);
            _dictionaryDayControl.Clear();
            foreach (var d in Days)
            {
                d.Date = startDay;
                Pallete.PaintDay(d, Date);
                _dictionaryDayControl.Add(d.Date.Date, d);
                startDay = startDay.AddDays(1);
            }
            UpdateSelectedDates();
        }
        private void OnPrevious(object sender, MouseButtonEventArgs e)
        {
            if(Date == DateTime.MinValue)
                return; 
            Date = Date.AddMonths(-1);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MaxValue) 
                return;  
            Date = Date.AddMonths(1);
        }
        private void OnNow(object sender, MouseButtonEventArgs e)
        {
            Date = DateTime.Now;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var d = sender as DayControl;
            if (d != null)
                PeriodStart?.Invoke(d.Date);
        }
        private void DoMouseEnter(object sender, MouseEventArgs e)
        {
            var d = sender as DayControl;
            if (d != null)
                DayEnter?.Invoke(d.Date);
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var d = sender as DayControl;
            _tempSelection.Clear();
            if (d != null)
                PeriodFinish?.Invoke(d.Date);
        }
        public Action<DateTime> DayEnter { get; set; }
        public Action<DateTime> PeriodStart { get; set; }
        public Action<DateTime> PeriodFinish { get; set; }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _MainGrid = (Grid)GetTemplateChild(TP_MAIN_GRID_PART);

            _Title = (TitleControl)GetTemplateChild(TP_TITLE_PART);
            _Title.Type = TitleControlType.Title;
            _Title.MouseLeftButtonDown += OnNow;

            _Previous = (TitleControl)GetTemplateChild(TP_PREVIOUS_PART);
            _Previous.Type = TitleControlType.Button;
            _Previous.Text = "<";
            _Previous.MouseLeftButtonDown += OnPrevious;

            _Next = (TitleControl)GetTemplateChild(TP_NEXT_PART);
            _Next.Type = TitleControlType.Button;
            _Next.Text = ">";
            _Next.MouseLeftButtonDown += OnNext;


            for (int x = 0; x < 7; x++) // Second - for -day title
            {
                var d = new TitleControl()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontSize = 10,
                    Visibility = Visibility.Visible,
                    Type = TitleControlType.WeekTitle
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
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                    };
                    Grid.SetColumn(d, x);
                    Grid.SetRow(d, y);
                    d.MouseDown += OnMouseDown;
                    d.MouseUp += OnMouseUp;
                    d.MouseEnter += DoMouseEnter;
                    _MainGrid.Children.Add(d);
                    Days.Add(d);
                }
            }
            UpdateElements();
            UpdateSelectedDates();
        }
    }
}
