using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            _TitleContentControl.Content = (WeatherTemplate == null) ? null : WeatherTemplate.LoadContent();
        }
        #endregion
        #region DayWeatherCollection
        public static readonly DependencyProperty DayWeatherCollectionProperty =
            DependencyProperty.Register(nameof(DayWeatherCollection), typeof(Dictionary<DateTime, IDayWeather>), typeof(DayMonthEventControl), new PropertyMetadata(DayWeatherCollectionPropertyChanged));

        public static void DayWeatherCollectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DayMonthEventControl)d).DayWeatherCollection = (Dictionary<DateTime, IDayWeather>)e.NewValue;
        }
        public Dictionary<DateTime, IDayWeather> DayWeatherCollection
        {
            get { return (Dictionary<DateTime, IDayWeather>)GetValue(DayWeatherCollectionProperty); }
            set
            {
                if (value == null)
                    return;
                SetValue(DayWeatherCollectionProperty, value);
                UpdateWeather();
            }
        }
        public void UpdateWeather()
        {
            if ((DayWeatherCollection == null)||(_TitleContentControl.Content == null))
                return;
            if(DayWeatherCollection.ContainsKey(Date))
            {
                (_TitleContentControl.Content as FrameworkElement).Visibility = Visibility.Visible;
                (_TitleContentControl.Content as FrameworkElement).DataContext = DayWeatherCollection[Date];
            }
            else
                (_TitleContentControl.Content as FrameworkElement).Visibility = Visibility.Hidden;
        }
        #endregion
        public DayType Type { private set; get; }
        public Action<DateTime> AddAction { get; set; }
        public double TitleSize { get => _Title.ActualHeight; }
        public void UpdateEvents()
        {
            if (_Title == null)
                return;

            _Title.Content = (Date == new DateTime(Date.Year, Date.Month, 1)) ? Date.ToString("MM.dd") : Date.ToString("dd");
            UpdateWeather();
        }
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
            WeatherTemplateUpdate();
            UpdateEvents();
            UpdateWeather();
        }
        private void OnAddEvent(object sender, MouseButtonEventArgs e)
        {
            AddAction?.Invoke(Date);
        }

    }
}

