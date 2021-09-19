using System;
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

    public class LabelTitle: Label, ITitleControl
    {
        public TitleControlType Type { get; set; }
        public string Text 
        {
            get => (String)Content;
            set { Content = value; }
        }
    }

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
        private LabelTitle _Title;
        private LabelTitle _Previous;
        private LabelTitle _Next;
        private List<DayMonthEventControl> _Days;
        private List<LabelTitle> _TitleDays;
        ~MonthEventControl()
        {
            _Previous.MouseLeftButtonDown -= OnPrevious;
            _Next.MouseLeftButtonDown -= OnNext;
            _Title.MouseLeftButtonDown -= OnNow;
        }
        public MonthEventControl()
        {
            _Days = new List<DayMonthEventControl>();
            _TitleDays = new List<LabelTitle>();
            Pallete = new PalleteMounthEvent();
        }
        static MonthEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthEventControl), new FrameworkPropertyMetadata(typeof(MonthEventControl)));
        }

        #region AddEvent
        public static readonly DependencyProperty AddEventProperty =
            DependencyProperty.Register("AddEvent", typeof(ICommand), typeof(MonthEventControl), new PropertyMetadata(AddEventPropertyChanged));
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
        #region ItemTemplate
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(MonthEventControl), new PropertyMetadata(ItemTemplateChanged));
        public static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).ItemTemplate = (DataTemplate)e.NewValue;
        }
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set
            {
                SetValue(ItemTemplateProperty, value);
                UpdateElements();
            }
        }
        #endregion
        #region Events
        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register("Events", typeof(ObservableCollection<Event>), typeof(MonthEventControl), new PropertyMetadata(OnEventsChanged));

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
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MonthEventControl), new PropertyMetadata(DatePropertyChanged));

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
        #region Pallete
        public static readonly DependencyProperty PalleteProperty =
            DependencyProperty.Register(nameof(Pallete), typeof(Pallete), typeof(MonthEventControl), new PropertyMetadata(PalleteChanged));
        private static void PalleteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MonthEventControl)d).Pallete = (Pallete)e.NewValue;
        }
        public Pallete Pallete
        {
            get { return (Pallete)GetValue(PalleteProperty); }
            set
            {
                SetValue(PalleteProperty, value);
                UpdateElements();
            }
        }
        #endregion
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
            _Title.Type = TitleControlType.Title;
            Pallete.PaintTitle(_Next, Date);
            _Title.Type = TitleControlType.Button;
            Pallete.PaintTitle(_Previous, Date);
            _Title.Type = TitleControlType.Button;


            if (_Previous.Visibility == Visibility.Hidden)
                Grid.SetColumnSpan(_Title, 7);
            else
                Grid.SetColumnSpan(_Title, 5);

            var startDay = Date.AddDays(-dayOfWeek);
            foreach (var t in _TitleDays)
            {
                if ((startDay.DayOfWeek == DayOfWeek.Saturday) || (startDay.DayOfWeek == DayOfWeek.Sunday))
                    t.Type = TitleControlType.WeekTitleDayOff;
                else
                    t.Type = TitleControlType.WeekTitle;
                Pallete.PaintTitle(t, startDay);
                startDay = startDay.AddDays(1);
            }

            startDay = Date.AddDays(-dayOfWeek);
            foreach (var d in _Days)
            {
                d.Date = startDay;
                Pallete.PaintDay(d, Date);
                startDay = startDay.AddDays(1);
            }
            UpdateEvents();
        }

        public void UpdateEvents()
        {
            if ((_Title != null) && (Events != null))
            {
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
        }
        private void OnPrevious(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MinValue) { return; }
            Date = Date.AddMonths(-1);
        }
        private void OnNext(object sender, MouseButtonEventArgs e)
        {
            if (Date == DateTime.MaxValue) { return; }
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

            _Title = (LabelTitle)GetTemplateChild(TP_TITLE_PART);
            _Title.Type = TitleControlType.Title;
            _Title.MouseLeftButtonDown += OnNow;

            _Previous = (LabelTitle)GetTemplateChild(TP_PREVIOUS_PART);
            _Previous.Type = TitleControlType.Button;
            _Previous.Text = "<";
            _Previous.MouseLeftButtonDown += OnPrevious;

            _Next = (LabelTitle)GetTemplateChild(TP_NEXT_PART);
            _Next.Type = TitleControlType.Button;
            _Next.Text = ">";
            _Next.MouseLeftButtonDown += OnNext;

            for (int x = 0; x < 7; x++) // Second - for -day title
            {
                var d = new LabelTitle()
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
            UpdateElements();
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
