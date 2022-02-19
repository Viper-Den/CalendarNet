using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ZoomPanel
{
    [TemplatePart(Name = ZoomPanel.TP_CONTENT_CONTROLL, Type = typeof(FrameworkElement))]
    public class ZoomPanel : Control
    {
        private const string TP_CONTENT_CONTROLL = "xContentControl";
        private const string TP_BACKGROUND_CONTROLL = "xBackgroundControl";
        private const string TP_SCROLL_HORIZONTAL = "xScrollHorizontal";
        private const string TP_SCROLL_VERTICAL = "xScrollVertical";
        private ContentControl _contentControl = null;
        private FrameworkElement _backgroundControl = null;
        private Point _currentImageOffset;
        private Point _startMouseMove;
        private TranslateTransform _translateTransform;
        private ScaleTransform _scaleTransform;
        private RowDefinition _scrollHorizontal;
        private ColumnDefinition _scrollVertical;
        
        static ZoomPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomPanel), new FrameworkPropertyMetadata(typeof(ZoomPanel)));
        }

        public double OffsetX { get => _translateTransform.X; }
        public double OffsetY { get => _translateTransform.Y; }

        #region ContentTemplate
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(ZoomPanel), new PropertyMetadata(ItemTemplateChanged));
        public static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanel)d).ContentTemplate = (DataTemplate)e.NewValue;
        }
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set
            {
                SetValue(ContentTemplateProperty, value);
                if(_contentControl != null)
                    _contentControl.Content = ContentTemplate.LoadContent();
            }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            _contentControl = (ContentControl)GetTemplateChild(TP_CONTENT_CONTROLL);
            _backgroundControl = (FrameworkElement)GetTemplateChild(TP_BACKGROUND_CONTROLL);
            _scrollHorizontal = (RowDefinition)GetTemplateChild(TP_SCROLL_HORIZONTAL);
            _scrollVertical = (ColumnDefinition)GetTemplateChild(TP_SCROLL_VERTICAL);
            _contentControl.HorizontalAlignment = HorizontalAlignment.Left;
            _contentControl.VerticalAlignment = VerticalAlignment.Top;

            _translateTransform = new TranslateTransform();
            _scaleTransform = new ScaleTransform();

            TransformGroup group = new TransformGroup();
            group.Children.Add(_scaleTransform);
            group.Children.Add(_translateTransform);

            _contentControl.RenderTransform = group;
            _contentControl.RenderTransformOrigin = new Point(0.0, 0.0);
            _contentControl.Content = ContentTemplate.LoadContent();

            _backgroundControl.MouseWheel += child_MouseWheel;
            _backgroundControl.MouseLeftButtonDown += child_MouseLeftButtonDown;
            _backgroundControl.MouseLeftButtonUp += child_MouseLeftButtonUp;
            _backgroundControl.MouseMove += child_MouseMove;
            _backgroundControl.PreviewMouseRightButtonDown += new MouseButtonEventHandler(child_PreviewMouseRightButtonDown);
            _backgroundControl.SizeChanged += DoSizeChanged;
        }

        private void DoSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var w2 = (e.NewSize.Width - e.PreviousSize.Width) / 2;
            var h2 = (e.NewSize.Height - e.PreviousSize.Height) / 2;
            SetTransform(OffsetX + w2, OffsetY + h2);
        }

        public void Reset()
        {
            _scaleTransform.ScaleX = 1.0;
            _scaleTransform.ScaleY = 1.0;
            SetTransform(0, 0);
        }

        private void SetTransform(double x, double y)
        {
            var newContentWidth = _contentControl.ActualWidth * _scaleTransform.ScaleX;
            var newContentHeight = _contentControl.ActualHeight * _scaleTransform.ScaleY;
            var offsetX = (ActualWidth - newContentWidth) / 2;
            var offsetY = (ActualHeight - newContentHeight) / 2;

            // если уменьшили до размера окна то скрываем скролы 
            //[+1] если не добавлять, то приведет мерцанию скролов  
            _scrollHorizontal.Height = (newContentHeight < ActualHeight+1) ? new GridLength(0) : new GridLength(16);
            _scrollVertical.Width = (newContentWidth < ActualWidth+1) ? new GridLength(0) : new GridLength(16);


            if (ActualWidth > (_contentControl.ActualWidth * _scaleTransform.ScaleX))
                _translateTransform.X = offsetX;
            else
            {
                var cw2 = (_contentControl.ActualWidth / 2) * _scaleTransform.ScaleX;
                var maxX = cw2 + offsetX;
                var minX = -cw2 + offsetX;
                if (x < minX)
                    _translateTransform.X = minX;
                else if (x > maxX)
                    _translateTransform.X = maxX;
                else
                    _translateTransform.X = x;
            }

            if (ActualHeight > (_contentControl.ActualHeight * _scaleTransform.ScaleY))
                _translateTransform.Y = offsetY;
            else
            {
                var ch2 = (_contentControl.ActualHeight / 2) * _scaleTransform.ScaleY;
                var maxY = ch2 + offsetY;
                var minY = -ch2 + offsetY;
                if (y < minY)
                    _translateTransform.Y = minY;
                else if (y > maxY)
                    _translateTransform.Y = maxY;
                else
                    _translateTransform.Y = y;
            }
        }

        #region Child Events

        private void child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoom = e.Delta > 0 ? .2 : -.2;
            if (!(e.Delta > 0) && (_scaleTransform.ScaleX < .4 || _scaleTransform.ScaleY < .4))
                return;

            Point relative = e.GetPosition(_contentControl);
            double absoluteX;
            double absoluteY;

            absoluteX = relative.X * _scaleTransform.ScaleX + _translateTransform.X;
            absoluteY = relative.Y * _scaleTransform.ScaleY + _translateTransform.Y;

            _scaleTransform.ScaleX += zoom;
            _scaleTransform.ScaleY += zoom;
            SetTransform(absoluteX - relative.X * _scaleTransform.ScaleX, absoluteY - relative.Y * _scaleTransform.ScaleY);
        }

        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startMouseMove = e.GetPosition(this);
            _currentImageOffset = new Point(_translateTransform.X, _translateTransform.Y);
            this.Cursor = Cursors.Hand;
            _contentControl.CaptureMouse();
        }

        private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _contentControl.ReleaseMouseCapture();
            this.Cursor = Cursors.Arrow;
        }

        void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Reset();
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (_contentControl.IsMouseCaptured)
            {
                Vector v = _startMouseMove - e.GetPosition(this);
                SetTransform(_currentImageOffset.X - v.X, _currentImageOffset.Y - v.Y);
            }
        }

        #endregion
    }
}
