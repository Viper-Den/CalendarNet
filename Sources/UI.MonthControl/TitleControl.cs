using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Destiny.Core;

namespace UIMonthControl
{

    [TemplatePart(Name = TitleControl.TP_TITLE_PART, Type = typeof(FrameworkElement))]
    public class TitleControl : Control, ITitleControl
    {
        private Label _Title;
        private const string TP_TITLE_PART = "xTitle";
        static TitleControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleControl), new FrameworkPropertyMetadata(typeof(TitleControl)));
        }
        public TitleControl()
        {
        }
        ~TitleControl()
        {
            _Title.MouseDown -= DoMouseDown;
            _Title.MouseEnter -= DoMouseEnter;
            _Title.MouseLeave -= DoMouseLeave;
            _Title.MouseUp -= DoMouseUp;
        }
        public TitleControlType Type { get; set; }

        private void DoMouseUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseUp(e);
        }

        private void DoMouseLeave(object sender, MouseEventArgs e)
        {
            OnMouseLeave(e);
        }

        private void DoMouseEnter(object sender, MouseEventArgs e)
        {
            OnMouseEnter(e);
        }

        private void DoMouseDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseDown(e);
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
            _Title.MouseDown += DoMouseDown;
            _Title.MouseEnter += DoMouseEnter;
            _Title.MouseLeave += DoMouseLeave;
            _Title.MouseUp += DoMouseUp;
        }

    }
}
