using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Converters;
using Destiny.Core;

namespace MonthEvent
{
    [TemplatePart(Name = DayMonthEventControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DayMonthEventControl.TP_CONTENT, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DayMonthEventControl.TP_CONTENT_GRID, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DayMonthEventControl.TP_CONTENT_CONTENTCONTROL, Type = typeof(FrameworkElement))] 
    public class DayMonthEventControl : BaseControl, IDayControl
    {
        private Label _Title;
        private ListView _Content;
        private Grid _TitleGrid;
        private ContentControl _TitleContentControl;
        private const string TP_TITLE_PART = "xTitle";
        private const string TP_CONTENT = "xContent";
        private const string TP_CONTENT_GRID = "xTitleGrid";
        private const string TP_CONTENT_CONTENTCONTROL = "xTitleContentControl"; 
        static DayMonthEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayMonthEventControl), new FrameworkPropertyMetadata(typeof(DayMonthEventControl)));
        }
        public DayMonthEventControl()
        {
            Events = new ObservableCollectionWithItemNotify<Event>();
        }
        #region EventsProperty
        public static readonly DependencyProperty EventsProperty =
           DependencyProperty.Register(nameof(Events), typeof(ObservableCollectionWithItemNotify<Event>), typeof(DayMonthEventControl), new PropertyMetadata(OnEventsChanged));
        public ObservableCollectionWithItemNotify<Event> Events
        {
            get { return (ObservableCollectionWithItemNotify<Event>)GetValue(EventsProperty); }
            private set  { SetValue(EventsProperty, value); }
        }
        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).Events = (ObservableCollectionWithItemNotify<Event>)e.NewValue;
        }
        #endregion
        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(DayMonthEventControl), new PropertyMetadata(DatePropertyChanged));
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
                Type = Palette.GetDateType(value);
                UpdateEvents();
            }
        }
        public static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).Date = (DateTime)e.NewValue;
        }
        #endregion        
        #region WeatherTemplate
        public static readonly DependencyProperty WeatherTemplateProperty =
            DependencyProperty.Register(nameof(WeatherTemplate), typeof(DataTemplate), typeof(DayMonthEventControl), new PropertyMetadata(WeatherTemplateChanged));
        public static void WeatherTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).WeatherTemplate = (DataTemplate)e.NewValue;
        }
        public DataTemplate WeatherTemplate
        {
            get { return (DataTemplate)GetValue(WeatherTemplateProperty); }
            set { 
                SetValue(WeatherTemplateProperty, value);
                WeatherTemplateUpdate();
            }
        }
        public void WeatherTemplateUpdate()
        {
            if (_TitleContentControl == null)
                return;

            if (WeatherTemplate == null)
                _TitleContentControl.Content = null;
            else
                _TitleContentControl.Content = WeatherTemplate.LoadContent();
        }
        #endregion
        #region Wather
        public static readonly DependencyProperty DayWatherCollectionProperty =
            DependencyProperty.Register(nameof(DayWatherCollection), typeof(ObservableCollection<IDayWather>), typeof(DayMonthEventControl), new PropertyMetadata(DayWatherCollectionPropertyChanged));

        public static void DayWatherCollectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).DayWatherCollection = (ObservableCollection<IDayWather>)e.NewValue;
        }
        public ObservableCollection<IDayWather> DayWatherCollection
        {
            get { return (ObservableCollection<IDayWather>)GetValue(DayWatherCollectionProperty); }
            set
            {
                SetValue(DayWatherCollectionProperty, value);
                UpdateWather();
            }
        }
        public void UpdateWather()
        {
            if ((DayWatherCollection == null)||(_TitleContentControl.Content == null))
                return;
            foreach (var w in DayWatherCollection)
            {
                if (w.IsDate(Date))
                {
                    (_TitleContentControl.Content as FrameworkElement).Visibility = Visibility.Visible;
                    (_TitleContentControl.Content as FrameworkElement).DataContext = w;
                }
                //else
                //    (_TitleContentControl.Content as FrameworkElement).Visibility = Visibility.Hidden;
            }
        }
        #endregion
        public DayType Type { private set; get; }
        public Action<DateTime> AddAction { get; set; }
        public void UpdateEvents()
        {
            if (_Title != null) 
            { 
                if (Date == new DateTime(Date.Year, Date.Month, 1))
                    _Title.Content = Date.ToString("MM.dd");
                else
                    _Title.Content = Date.ToString("dd");
            }
        }
        public double TitleSize { get => _Title.ActualHeight; }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
            _Content = (ListView)GetTemplateChild(TP_CONTENT);
            _TitleGrid = (Grid)GetTemplateChild(TP_CONTENT_GRID);
            _TitleContentControl = (ContentControl)GetTemplateChild(TP_CONTENT_CONTENTCONTROL);
            _Content.ItemsSource = Events;
            _Content.ItemTemplate = ItemTemplate;
            _TitleGrid.PreviewMouseUp += OnAddEvent;
            UpdateEvents();
            WeatherTemplateUpdate();
            UpdateWather();
        }
        private void OnAddEvent(object sender, MouseButtonEventArgs e)
        {
            AddAction?.Invoke(Date);
        }

    }
}

