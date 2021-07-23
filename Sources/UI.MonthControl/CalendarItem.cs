using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace UIMonthControl
{
    [TemplatePart(Name = CalendarItem.TP_GRID, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CalendarItem.TP_LABEL, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CalendarItem.TP_BORDER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CalendarItem.TP_SETTINGS, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CalendarItem.TP_IMAGE_IS_CHECKED, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CalendarItem.TP_CHANGE_POSITION_BUTTON, Type = typeof(FrameworkElement))]
    public class CalendarItem : Control
    {
        private const string TP_LABEL = "xLabel";
        private const string TP_GRID = "xGrid";
        private const string TP_BORDER = "xBorder";
        private const string TP_SETTINGS = "xSettings";
        private const string TP_IMAGE_IS_CHECKED = "xImageIsChecked";
        private const string TP_CHANGE_POSITION_BUTTON = "xChangePositionButton";

        private Grid _grid;
        private Label _Label;
        private Image _settings;
        private Image _сhangePosition;
        private Image _ImageIsChecked;
        private Border _BorderIsChecked;

        private const int DEF_MAX_VALUE = 150;

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

        #region IsChecked
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(CalendarItem), new PropertyMetadata(IsCheckedPropertyChanged));

        private static void IsCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CalendarItem)d).IsChecked = (bool)e.NewValue;
        }
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set 
            { 
                SetValue(IsCheckedProperty, value);
                SetIsCheck(value);
            }
        }
        #endregion

        static CalendarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(typeof(CalendarItem)));
        }
        ~CalendarItem()
        {
            _BorderIsChecked.MouseUp -= DoMouseUp;
            _Label.MouseUp -= DoMouseUp;
            MouseLeave -= DoMouseLeave;
            MouseEnter -= DoMouseEnter;
            _settings.MouseDown -= DoMouseDownSettings;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ButtonsVisibility = Visibility.Hidden;
            _grid = (Grid)GetTemplateChild(TP_GRID);
            _settings = (Image)GetTemplateChild(TP_SETTINGS);
            _сhangePosition = (Image)GetTemplateChild(TP_SETTINGS);
            _ImageIsChecked = (Image)GetTemplateChild(TP_IMAGE_IS_CHECKED);
            _BorderIsChecked = (Border)GetTemplateChild(TP_BORDER);
            _Label = (Label)GetTemplateChild(TP_LABEL);
            _BorderIsChecked.MouseUp += DoMouseUp;
            _Label.MouseUp += DoMouseUp;
            MouseLeave += DoMouseLeave;
            MouseEnter += DoMouseEnter;
            _settings.MouseDown += DoMouseDownSettings;
            SetIsCheck(IsChecked);
        }
        private void DoMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsChecked = !IsChecked;
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

        private void SetIsCheck(bool value)
        {
            if((_BorderIsChecked == null) || (_ImageIsChecked == null))
              return;
            
            if (value)
            {
                //if (_BorderIsChecked.BorderBrush is SolidColorBrush colorBrush)
                //    if ((colorBrush.Color.R > DEF_MAX_VALUE) || (colorBrush.Color.G > DEF_MAX_VALUE) || (colorBrush.Color.B > DEF_MAX_VALUE))
                //    _ImageIsChecked.Source = new BitmapImage(new Uri("../Images/3.png", UriKind.Relative));
                //    else
                //        _ImageIsChecked.Source = new BitmapImage(new Uri("../Images/2.png", UriKind.Relative));
                _ImageIsChecked.Source = new BitmapImage(new Uri("../Images/2.png", UriKind.Relative));
                _BorderIsChecked.Background = _BorderIsChecked.BorderBrush;
            }
            else
            {
                _ImageIsChecked.Source = new BitmapImage(new Uri("../Images/1.png", UriKind.Relative));
                _BorderIsChecked.Background = Brushes.Transparent;
            }
        }
    }
}
