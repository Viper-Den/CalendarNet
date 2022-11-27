using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp4
{
    public class ZoomBorder : Border
    {
        private FrameworkElement _child = null;
        private Point _currentImageOffset;
        private Point _startMouseMove;
        private TranslateTransform _translateTransform;
        private ScaleTransform _scaleTransform;
        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != this.Child)
                    this.Initialize(value);
                base.Child = value;
            }
        }
        public double OffsetX { get => _translateTransform.X; }
        public double OffsetY { get => _translateTransform.Y; }

        public void Initialize(UIElement element)
        {
            if (element is FrameworkElement c)
            {
                _child = c;
                _child.HorizontalAlignment = HorizontalAlignment.Left;
                _child.VerticalAlignment = VerticalAlignment.Top;
                _translateTransform = new TranslateTransform();
                _scaleTransform = new ScaleTransform();
                TransformGroup group = new TransformGroup();
                group.Children.Add(_scaleTransform);
                group.Children.Add(_translateTransform);

                _child.RenderTransform = group;
                _child.RenderTransformOrigin = new Point(0.0, 0.0);

                this.MouseWheel += child_MouseWheel;
                this.MouseLeftButtonDown += child_MouseLeftButtonDown;
                this.MouseLeftButtonUp += child_MouseLeftButtonUp;
                this.MouseMove += child_MouseMove;
                this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(child_PreviewMouseRightButtonDown);
                this.SizeChanged += DoSizeChanged;
                this.Width = _child.Width;
                this.Height = _child.Height;
            }
        }

        private void DoSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_child != null)
            {
                var w2 = (e.NewSize.Width - e.PreviousSize.Width) / 2;
                var h2 = (e.NewSize.Height - e.PreviousSize.Height) / 2;
                SetTransform(OffsetX + w2, OffsetY + h2); 
            }
        }

        public void Reset()
        {
            if (_child != null)
            {
                _scaleTransform.ScaleX = 1.0;
                _scaleTransform.ScaleY = 1.0;
                SetTransform(0, 0);
            }
        }
       
        private void SetTransform(double x, double y)
        {
            var offsetX = ((ActualWidth - (_child.ActualWidth * _scaleTransform.ScaleX)) / 2);
            var offsetY = ((ActualHeight - (_child.ActualHeight * _scaleTransform.ScaleY)) / 2);


            if (ActualWidth > (_child.ActualWidth * _scaleTransform.ScaleX))
                _translateTransform.X = offsetX;
            else
            {
                var cw2 = (_child.ActualWidth / 2) * _scaleTransform.ScaleX;
                var maxX = cw2 + offsetX;
                var minX = -cw2 + offsetX;
                if (x < minX)
                    _translateTransform.X = minX;
                else if (x > maxX)
                    _translateTransform.X = maxX;
                else
                    _translateTransform.X = x;
            }

            if (ActualHeight > (_child.ActualHeight * _scaleTransform.ScaleY))
                _translateTransform.Y = offsetY;
            else
            {
                var ch2 = (_child.ActualHeight / 2) * _scaleTransform.ScaleY;
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
            if (_child != null)
            {

                double zoom = e.Delta > 0 ? .2 : -.2;
                if (!(e.Delta > 0) && (_scaleTransform.ScaleX < .4 || _scaleTransform.ScaleY < .4))
                    return;

                Point relative = e.GetPosition(_child);
                double absoluteX;
                double absoluteY;

                absoluteX = relative.X * _scaleTransform.ScaleX + _translateTransform.X;
                absoluteY = relative.Y * _scaleTransform.ScaleY + _translateTransform.Y;

                _scaleTransform.ScaleX += zoom;
                _scaleTransform.ScaleY += zoom;
                SetTransform(absoluteX - relative.X * _scaleTransform.ScaleX, absoluteY - relative.Y * _scaleTransform.ScaleY);
            }
        }

        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_child != null)
            {
                _startMouseMove = e.GetPosition(this);
                _currentImageOffset = new Point(_translateTransform.X, _translateTransform.Y);
                this.Cursor = Cursors.Hand;
                _child.CaptureMouse();
            }
        }

        private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_child != null)
            {
                _child.ReleaseMouseCapture();
                this.Cursor = Cursors.Arrow;
            }
        }

        void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Reset();
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (_child != null)
            {
                if (_child.IsMouseCaptured)
                {
                    Vector v = _startMouseMove - e.GetPosition(this);
                    SetTransform(_currentImageOffset.X - v.X, _currentImageOffset.Y - v.Y);
                }
            }
        }

        #endregion
    }
}