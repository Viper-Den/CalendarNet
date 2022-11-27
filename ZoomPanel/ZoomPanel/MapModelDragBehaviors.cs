using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ZoomPanel.Common;

namespace ZoomPanel
{
    public partial class DragBehavior : Behavior<Control>
    {
        private bool _isDragging = false;
        private Point _mouseOffset;
        private Canvas _canvas;

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtomDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtomUp;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtomDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtomUp;
        }

        private void AssociatedObject_MouseLeftButtomDown(object sender, MouseButtonEventArgs e)
        {
            _canvas = VisualTreeHelper.GetParent(this.AssociatedObject) as Canvas;
            if (_canvas != null)
            {
                _isDragging = true;

                _mouseOffset = e.GetPosition(AssociatedObject);

                AssociatedObject.CaptureMouse();

                OnSelected?.Invoke(AssociatedObject);
            }
            else _isDragging = false;
            e.Handled = _isDragging;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                var v = e.GetPosition(_canvas) - _mouseOffset;
                var x = MathHelper.Clamp(v.X, 0, _canvas.Width - AssociatedObject.ActualWidth);
                var y = MathHelper.Clamp(v.Y, 0, _canvas.Height - AssociatedObject.ActualHeight);

                AssociatedObject.SetValue(Canvas.LeftProperty, x);
                AssociatedObject.SetValue(Canvas.TopProperty, y);
            }
        }

        private void AssociatedObject_MouseLeftButtomUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                AssociatedObject.ReleaseMouseCapture();
                _isDragging = false;
            }
        }

        public Action<Control> OnSelected { get; set; }
    }
}
