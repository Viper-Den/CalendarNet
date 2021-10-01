﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Destiny.Core;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;

namespace MonthEvent
{

    //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.controls.itemscontrol?view=netcore-3.1
    [TemplatePart(Name = MonthEventControl.TP_MAIN_GRID_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthEventControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthEventControl.TP_PREVIOUS_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MonthEventControl.TP_NEXT_PART, Type = typeof(FrameworkElement))]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class MonthEventControl : Selector
    {
        private const string TP_MAIN_GRID_PART = "MainGrid";
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_PREVIOUS_PART = "xPrevious";
        private const string TP_NEXT_PART = "xNext";
        private Grid _MainGrid;
        private LabelTitleControl _Title;
        private LabelTitleControl _Previous;
        private LabelTitleControl _Next;
        private List<DayMonthEventControl> _Days;
        private List<LabelTitleControl> _TitleDays;
        ~MonthEventControl()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
            _Title.MouseLeftButtonDown -= OnNow;
        }
        public MonthEventControl()
        {
            _Days = new List<DayMonthEventControl>();
            _TitleDays = new List<LabelTitleControl>();
            Palette = new PaletteMounthEvent();
        }
        static MonthEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthEventControl), new FrameworkPropertyMetadata(typeof(MonthEventControl)));
        }

        #region AddEvent
        public static readonly DependencyProperty AddEventProperty =
            DependencyProperty.Register(nameof(AddEvent), typeof(ICommand), typeof(MonthEventControl), new PropertyMetadata(AddEventPropertyChanged));
        public static void AddEventPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).AddEvent = (ICommand)e.NewValue;
        }
        public ICommand AddEvent
        {
            get { return (ICommand)GetValue(AddEventProperty); }
            set { SetValue(AddEventProperty, value); }
        }
        #endregion
        #region EventTemplate
        public static readonly DependencyProperty EventTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(MonthEventControl), new PropertyMetadata(ItemTemplateChanged));
        public static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).EventTemplate = (DataTemplate)e.NewValue;
        }
        public DataTemplate EventTemplate
        {
            get { return (DataTemplate)GetValue(EventTemplateProperty); }
            set
            {
                SetValue(EventTemplateProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region Events
        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register(nameof(Events), typeof(ObservableCollection<Event>), typeof(MonthEventControl), new PropertyMetadata(OnEventsChanged));

        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).Events = (ObservableCollection<Event>)e.NewValue;
        }
        public ObservableCollection<Event> Events
        {
            get { return (ObservableCollection<Event>) GetValue(EventsProperty);}
            set
            {
                if (Events != null)
                    Events.CollectionChanged -= DoNotifyCollectionChangedEventHandler;

                SetValue(EventsProperty, value);
                if(Events != null)
                  Events.CollectionChanged += DoNotifyCollectionChangedEventHandler;
                UpdateEvents();
            }
        }
        #endregion
        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(MonthEventControl), new PropertyMetadata(DatePropertyChanged));

        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).Date = (DateTime)e.NewValue;
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
        #region Palette
        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register(nameof(Palette), typeof(Palette), typeof(MonthEventControl), new PropertyMetadata(PaletteProperty));
        private static void PalettePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).Palette = (Palette)e.NewValue;
        }
        public Palette Palette
        {
            get { return (Palette)GetValue(PaletteProperty); }
            set
            {
                SetValue(PaletteProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region Wather
        public static readonly DependencyProperty DayWatherCollectionProperty =
            DependencyProperty.Register(nameof(DayWatherCollection), typeof(Dictionary<DateTime, IDayWather>), typeof(MonthEventControl), new PropertyMetadata(DayWatherCollectionPropertyChanged));

        public static void DayWatherCollectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).DayWatherCollection = (Dictionary<DateTime, IDayWather>)e.NewValue;
        }
        public Dictionary<DateTime, IDayWather> DayWatherCollection
        {
            get { return (Dictionary<DateTime, IDayWather>)GetValue(DayWatherCollectionProperty); }
            set
            {
                SetValue(DayWatherCollectionProperty, value);
                UpdateWather();
            }
        }
        public void UpdateWather()
        {
            if (_Days == null)
                return;
            foreach (var d in _Days)
                d.DayWatherCollection = DayWatherCollection;
        }
        #endregion
        #region WeatherTemplate
        public static readonly DependencyProperty WeatherTemplateProperty =
            DependencyProperty.Register(nameof(WeatherTemplate), typeof(DataTemplate), typeof(MonthEventControl), new PropertyMetadata(WeatherTemplateChanged));
        public static void WeatherTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).WeatherTemplate = (DataTemplate)e.NewValue;
        }
        public DataTemplate WeatherTemplate
        {
            get { return (DataTemplate)GetValue(WeatherTemplateProperty); }
            set
            {
                SetValue(WeatherTemplateProperty, value);
                UpdateWeatherTemplate();
            }
        }
        public void UpdateWeatherTemplate()
        {
            if (_Days == null)
                return;
            foreach (var d in _Days)
                d.WeatherTemplate = WeatherTemplate;
        }
        #endregion
        private void UpdateElements()
        {
            if (_Title == null)
                return;

            Palette.PaintTitle(_Title, Date);
            Palette.PaintTitle(_Next, Date);
            Palette.PaintTitle(_Previous, Date);


            if (_Previous.Visibility == Visibility.Hidden)
                Grid.SetColumnSpan(_Title, 7);
            else
                Grid.SetColumnSpan(_Title, 5);

            var startDay = DateHelper.GetWeekStartDate(Date);
            foreach (var t in _TitleDays)
            {
                if ((startDay.DayOfWeek == DayOfWeek.Saturday) || (startDay.DayOfWeek == DayOfWeek.Sunday))
                    t.Type = TitleControlType.WeekTitleDayOff;
                else
                    t.Type = TitleControlType.WeekTitle;
                Palette.PaintTitle(t, startDay);
                startDay = startDay.AddDays(1);
            }

            startDay = DateHelper.GetWeekStartDate(Date);
            foreach (var d in _Days)
            {
                d.Date = startDay;
                Palette.PaintDay(d, Date);
                startDay = startDay.AddDays(1);
            }
            UpdateEvents();
        }
        public void UpdateEvents()
        {
            if ((_Title == null) || (Events == null))
                return;
            foreach (var d in _Days)
            {
                foreach (var e in Events)
                {
                    if (e.Rule.IsDate(d.Date))
                    {
                        if (!d.Events.Contains(e))
                            d.Events.Add(e);
                    }
                    else
                    {
                        if (d.Events.Contains(e))
                            d.Events.Remove(e);
                    }
                }
            }
        }
        private void OnPrevious(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MinValue) 
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
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _MainGrid = (Grid)GetTemplateChild(TP_MAIN_GRID_PART);

            _Title = (LabelTitleControl)GetTemplateChild(TP_TITLE_PART);
            _Title.Type = TitleControlType.Title;
            _Title.MouseLeftButtonDown += OnNow;

            _Previous = (LabelTitleControl)GetTemplateChild(TP_PREVIOUS_PART);
            _Previous.Type = TitleControlType.Button;
            _Previous.MouseLeftButtonDown += OnPrevious;

            _Next = (LabelTitleControl)GetTemplateChild(TP_NEXT_PART);
            _Next.Type = TitleControlType.Button;
            _Next.MouseLeftButtonDown += OnNext;

            for (int x = 0; x < 7; x++) // Second - for -day title
            {
                var d = new LabelTitleControl()
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
                d.Type = TitleControlType.Title;
            }

            for (int y = 2; y < 8; y++) // first for - title; Second - for - day title
            {
                for (int x = 0; x < 7; x++)
                {
                    var d = new DayMonthEventControl()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    Grid.SetColumn(d, x);
                    Grid.SetRow(d, y);
                    _MainGrid.Children.Add(d);
                    _Days.Add(d);
                    d.ItemTemplate = ItemTemplate;
                    d.AddAction += OnAddEvent;
                }
            }
            UpdateWeatherTemplate();
            UpdateElements();
            UpdateWather();
        }
        private void DoNotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    UpdateEvents();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UpdateEvents();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    UpdateEvents();
                    break;
            }
        }
        private void OnAddEvent(DateTime date)
        {
            AddEvent?.Execute(date);
        }

    }
}
