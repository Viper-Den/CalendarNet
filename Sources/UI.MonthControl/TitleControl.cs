using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UIMonthControl
{
    [TemplatePart(Name = TitleControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    public class TitleControl : Control
    {
        private Label _Title;
        private const string TP_TITLE_PART = "xTitle";
        static TitleControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleControl), new FrameworkPropertyMetadata(typeof(TitleControl)));
        }

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string), typeof(TitleControl), new PropertyMetadata(TextPropertyChanged));

        public static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TitleControl)d).Text = (string)e.NewValue;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set 
            {
                SetValue(TextProperty, value);
                UpdateControl();
            }
        }
        public void UpdateControl()
        {
            if (_Title != null)
            {
                _Title.Content = Text;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateControl();
        }
        protected override Size MeasureOverride(Size constraint)
        {
            var s = base.MeasureOverride(constraint);
            if (_Title != null)
            {
                _Title.FontSize = s.Height * 0.5;
            }
            return s;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _Title = (Label)GetTemplateChild(TP_TITLE_PART);
        }

    }
}
