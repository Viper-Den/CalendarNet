using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;


namespace UIMonthControl
{
    [TemplatePart(Name = CalendarItem.TP_GRID, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CalendarItem.TP_SETTINGS, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CalendarItem.TP_CHANGE_POSITION_BUTTON, Type = typeof(FrameworkElement))]
    public class CalendarItem : Control
    {
        private const string TP_GRID = "xGrid";
        private const string TP_SETTINGS = "xSettings";
        private const string TP_CHANGE_POSITION_BUTTON = "xChangePositionButton";
        private Grid _grid;
        private Image _settings;
        private Image _сhangePosition;

        //#region PropertyCalendar
        //public static readonly DependencyProperty CalendarProperty = DependencyProperty.Register("Calendar", 
        //        typeof(ICalendar), typeof(CalendarItem), new PropertyMetadata(CalendarPropertyChanged));

        //private static void CalendarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((CalendarItem)d).Calendar = (ICalendar)e.NewValue;
        //}
        //public ICalendar Calendar
        //{
        //    get { return (ICalendar)GetValue(CalendarProperty); }
        //    set { SetValue(CalendarProperty, value); }
        //}
        //#endregion

        #region ButtonsVisibilityProperty
        public static readonly DependencyProperty ButtonsVisibilityProperty = DependencyProperty.Register("ButtonsVisibility", 
                typeof(Visibility), typeof(CalendarItem), new PropertyMetadata(ButtonsVisibilityPropertyChanged));

        private static void ButtonsVisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CalendarItem)d).ButtonsVisibility = (Visibility)e.NewValue;
        }
        public Visibility ButtonsVisibility
        {
            get { return (Visibility)GetValue(ButtonsVisibilityProperty); }
            set { SetValue(ButtonsVisibilityProperty, value); }
        }
        #endregion

        #region SettingsCommand
        public static readonly DependencyProperty SettingsCommandProperty =
            DependencyProperty.Register("SettingsCommand", typeof(ICommand), typeof(CalendarItem), new PropertyMetadata(SettingsCommandPropertyChanged));

        private static void SettingsCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CalendarItem)d).SettingsCommand = (ICommand)e.NewValue;
        }
        public ICommand SettingsCommand
        {
            get { return (ICommand)GetValue(SettingsCommandProperty); }
            set { SetValue(SettingsCommandProperty, value); }
        }
        #endregion

        static CalendarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(typeof(CalendarItem)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ButtonsVisibility = Visibility.Hidden;
            _grid = (Grid)GetTemplateChild(TP_GRID);
            _settings = (Image)GetTemplateChild(TP_SETTINGS);
            _сhangePosition = (Image)GetTemplateChild(TP_SETTINGS);
            _grid.MouseLeave += DoMouseLeave;
            _grid.MouseEnter += DoMouseEnter;
            _settings.MouseDown += DoMouseDownSettings;
        }
        private void DoMouseDownSettings(object sender, MouseButtonEventArgs e)
        {
            SettingsCommand?.Execute(DataContext);
        }
        private void DoMouseLeave(object sender, MouseEventArgs e)
        {
            ButtonsVisibility = Visibility.Hidden;
        }
        private void DoMouseEnter(object sender, MouseEventArgs e)
        {
            ButtonsVisibility = Visibility.Visible;
        }

    }
}
