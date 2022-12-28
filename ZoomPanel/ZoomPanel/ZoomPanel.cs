using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ServiceLocation;
using Microsoft.Xaml.Behaviors;
using ZoomPanel.Common;

namespace ZoomPanel
{
    [TemplatePart(Name = ZoomPanelControl.TP_ZOOM_CONTANER, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ZoomPanelControl.TP_SCROLLBAR_VERTICAL, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ZoomPanelControl.TP_SCROLLBAR_HORIZONTAL, Type = typeof(FrameworkElement))]
    public class ZoomPanelControl : Control
    {
        private const string TP_ZOOM_CONTANER = "xZoomContaner";
        private const string TP_SCROLLBAR_VERTICAL = "xScrollBarVertical";
        private const string TP_SCROLLBAR_HORIZONTAL = "xScrollBarHorizontal";
        private Canvas _zoomContaner = null;
        private ScrollBar _scrollBarVertical = null;
        private ScrollBar _scrollBarHorizontal = null;

        private Point _startMouseMove;

        private TranslateTransform _translateTransform = new TranslateTransform();
        private ScaleTransform _scaleTransform = new ScaleTransform();

        private Dictionary<object, Control> _ViewBinding = new Dictionary<Object, Control>(); 


        static ZoomPanelControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomPanelControl), new FrameworkPropertyMetadata(typeof(ZoomPanelControl)));
        }

        public ZoomPanelControl() : base()
        {
            Loaded += ZoomPanelControl_Loaded;
            Scale = new ScaleParams();
        }

        private void ZoomPanelControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        public override void OnApplyTemplate()
        {
            _scrollBarVertical = (ScrollBar)GetTemplateChild(TP_SCROLLBAR_VERTICAL);

            _scrollBarHorizontal = (ScrollBar)GetTemplateChild(TP_SCROLLBAR_HORIZONTAL);

            _zoomContaner = (Canvas)GetTemplateChild(TP_ZOOM_CONTANER);

            TransformGroup group = new TransformGroup();
            group.Children.Add(_scaleTransform);
            group.Children.Add(_translateTransform);

            _zoomContaner.RenderTransform = group;
            _zoomContaner.RenderTransformOrigin = new Point(0.0, 0.0);
        }

        public double ViewerWidthHalf { get => ViewWidth / 2; }
        public double ViewerHeightHalf { get => ViewHeight / 2; }
        public double ViewerActualWidthHalf { get => this.ActualWidth / 2; }
        public double ViewerActualHeightHalf { get => this.ActualHeight / 2; }
        public double OffsetXMin { get { return -ViewWidth + ViewerActualWidthHalf; } }
        public double OffsetYMin { get { return -ViewHeight + ViewerActualHeightHalf; } }
        public double OffsetXMax { get { return ViewerActualWidthHalf; } } 
        public double OffsetYMax { get { return ViewerActualHeightHalf; } }

        
        #region ServiceLocator
        public static readonly DependencyProperty ServiceLocatorProperty =
            DependencyProperty.Register(nameof(ServiceLocator), typeof(IServiceLocator), typeof(ZoomPanelControl), new PropertyMetadata(ServiceLocatorPropertyChanged));
        public static void ServiceLocatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue is IServiceLocator s)
                ((ZoomPanelControl)d).ServiceLocator = (s);
        }
        public IServiceLocator ServiceLocator
        {
            get { return (IServiceLocator)GetValue(ServiceLocatorProperty); }
            set  
            { 
                SetValue(ServiceLocatorProperty, value);
                UpdateControls();
            }
        }
        #endregion

        #region SelectedMapElement
        public static readonly DependencyProperty SelectedMapElementProperty = DependencyProperty.Register(
            nameof(SelectedMapElement), typeof(object), typeof(ZoomPanelControl), new PropertyMetadata(SelectedMapElementPropertyChanged));
        public static void SelectedMapElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).SelectedMapElement = (object)e.NewValue;
        }
        public object SelectedMapElement
        {
            get { return (object)GetValue(SelectedMapElementProperty); }
            set { SetValue(SelectedMapElementProperty, value); }
        }
        #endregion  
        
        #region ViewOffsetY
        public static readonly DependencyProperty ViewOffsetYProperty = DependencyProperty.Register(
            nameof(ViewOffsetY), typeof(double), typeof(ZoomPanelControl), new PropertyMetadata(ViewOffsetYPropertyChanged));
        public static void ViewOffsetYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).ViewOffsetY = (double)e.NewValue;
        }
        public double ViewOffsetY
        {
            get { return (double)GetValue(ViewOffsetYProperty); }
            set { SetValue(ViewOffsetYProperty, value); }
        }
        #endregion         
        #region ViewOffsetX
        public static readonly DependencyProperty ViewOffsetXProperty =
            DependencyProperty.Register(nameof(ViewOffsetX), typeof(double), typeof(ZoomPanelControl), new PropertyMetadata(ViewOffsetXPropertyChanged));
        public static void ViewOffsetXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).ViewOffsetX = (double)e.NewValue;
        }
        public double ViewOffsetX
        {
            get { return (double)GetValue(ViewOffsetXProperty); }
            set { SetValue(ViewOffsetXProperty, value); }
        }
        #endregion

        #region Scale
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register(nameof(Scale), typeof(ScaleParams), typeof(ZoomPanelControl), new PropertyMetadata(ScalePropertyChanged));
        public static void ScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).Scale = (ScaleParams)e.NewValue;
        }
        public ScaleParams Scale
        {
            get { return (ScaleParams)GetValue(ScaleProperty); }
            set
            {
                if ((value == null) || (Scale == value))
                    return;

                SetValue(ScaleProperty, value);

                if (IsLoaded)
                    UpdateTransform();
            }
        }
        #endregion

        #region PositionValidator
        public static readonly DependencyProperty PositionValidatorProperty =
            DependencyProperty.Register(nameof(PositionValidator), typeof(IPositionValidator), typeof(ZoomPanelControl), new PropertyMetadata(PositionValidatorPropertyChanged));
        public static void PositionValidatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).PositionValidator = (IPositionValidator)e.NewValue;
        }
        public IPositionValidator PositionValidator
        {
            get { return (IPositionValidator)GetValue(PositionValidatorProperty); }
            set
            {
                if ((value == null) || (PositionValidator == value))
                    return;

                SetValue(PositionValidatorProperty, value);

                if (IsLoaded)
                    UpdateTransform();
            }
        }
        #endregion
        
        #region ViewWidth
        public static readonly DependencyProperty ViewWidthProperty =
            DependencyProperty.Register(nameof(ViewWidth), typeof(double), typeof(ZoomPanelControl), new PropertyMetadata(ViewWidthPropertyChanged));
        public static void ViewWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).ViewWidth = (double)e.NewValue;
        }
        public double ViewWidth
        {
            get { return (double)GetValue(ViewWidthProperty); }
            set { }
        }
        #endregion
        #region ViewHeight
        public static readonly DependencyProperty ViewHeightProperty =
            DependencyProperty.Register(nameof(ViewHeight), typeof(double), typeof(ZoomPanelControl), new PropertyMetadata(ViewHeightPropertyChanged));
        public static void ViewHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).ViewHeight = (double)e.NewValue;
        }
        public double ViewHeight
        {
            get { return (double)GetValue(ViewHeightProperty); }
            set {  }
        }
        #endregion
        
        #region ContentHeight
        public static readonly DependencyProperty ContentHeightProperty =
            DependencyProperty.Register(nameof(ContentHeight), typeof(double), typeof(ZoomPanelControl), new PropertyMetadata(ContentHeightPropertyChanged));
        public static void ContentHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).ContentHeight = (double)e.NewValue;
        }
        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set
            {
                SetValue(ContentHeightProperty, value);
                // todo reinit control size
            }
        }
        #endregion
        #region ContentWidth
        public static readonly DependencyProperty ContentWidthProperty =
            DependencyProperty.Register(nameof(ContentWidth), typeof(double), typeof(ZoomPanelControl), new PropertyMetadata(ContentWidthPropertyChanged));
        public static void ContentWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).ContentWidth = (double)e.NewValue;
        }
        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set 
            {  
                SetValue(ContentWidthProperty, value); 
                // todo reinit control size
            }
        }
        #endregion
        #region Controls
        public static readonly DependencyProperty ControlsProperty =
           DependencyProperty.Register(nameof(Controls), typeof(ObservableCollection<Object>), typeof(ZoomPanelControl), new PropertyMetadata(OnControlsChanged));

        private static void OnControlsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomPanelControl)d).Controls = (ObservableCollection<Object>)e.NewValue;
        }
        public ObservableCollection<Object> Controls
        {
            get { return (ObservableCollection<Object>)GetValue(ControlsProperty); }
            set
            {
                if (Controls != null)
                {
                    Controls.CollectionChanged -= DoNotifyCollectionChangedEventHandler;
                    DeInit();
                }

                SetValue(ControlsProperty, value);

                if (Controls != null)
                {
                    Controls.CollectionChanged += DoNotifyCollectionChangedEventHandler;
                    if (IsLoaded)
                        Init();
                }
            }
        }
        private void DoNotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(var vm in e.NewItems)
                        AddElement(vm);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var vm in e.OldItems)
                    {
                        if(vm == SelectedMapElement)
                            SelectedMapElement = null;
                        if(_ViewBinding.ContainsKey(vm))
                            _zoomContaner?.Children.Remove(_ViewBinding[vm]);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    SelectedMapElement = null;
                    _zoomContaner?.Children.Clear();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    UpdateControls();
                    break;
            }
        }
        public void UpdateControls() 
        {
            SelectedMapElement = null;
            _zoomContaner?.Children.Clear();
            if (Controls != null)
            {
                foreach (var vm in Controls)
                    AddElement(vm);
            }
        }
        private void AddElement(Object c)
        {
            if (ServiceLocator == null)
                return;
            var v = ServiceLocator.GetInstance(c.GetType()) as Control;

            var b = new DragBehavior();
            b.OnSelected += DoControlSelected;
            Interaction.GetBehaviors(v).Add(b);

            _ViewBinding.Add(c, v);
            v.DataContext = c;
            _zoomContaner?.Children.Add(v);
        }

        private void DoControlSelected(Control c)
        {
            SelectedMapElement = c;
        }
        #endregion


        private double OffsetX
        { 
            get => _translateTransform.X; 
            set
            {
                if (value < OffsetXMin)
                    _translateTransform.X = OffsetXMin;
                else if (value > OffsetXMax)
                    _translateTransform.X = OffsetXMax;
                else
                    _translateTransform.X = value;
            } 
        }
        private double OffsetY
        { 
            get => _translateTransform.Y;
            set
            {
                if (value < OffsetYMin)
                    _translateTransform.Y = OffsetYMin;
                else if (value > OffsetYMax)
                    _translateTransform.Y = OffsetYMax;
                else
                    _translateTransform.Y = value;
             }
        }

        private void Init()
        {
            UpdateTransform();
            UpdateControls();

            MouseWheel += DoMouseWheel;
            MouseLeftButtonDown += DoMouseLeftButtonDown;
            MouseLeftButtonUp += DoMouseLeftButtonUp;
            MouseMove += DoMouseMove;
            SizeChanged += DoSizeChanged;
        }
        private void DeInit()
        {
            if (!IsInitialized)
                return;
            MouseWheel -= DoMouseWheel;
            MouseDown -= DoMouseLeftButtonDown;
            MouseUp -= DoMouseLeftButtonUp;
            MouseMove -= DoMouseMove;
            SizeChanged -= DoSizeChanged;

            _scrollBarVertical.ValueChanged -= DoScrollBarVerticalValueChanged;
            _scrollBarHorizontal.ValueChanged -= DoScrollBarVerticalValueChanged;

            SelectedMapElement = null;

            _zoomContaner?.Children.Clear();
        }       
        
        private void UpdateTransform()
        {
            DoTransform(ViewOffsetX + ViewerActualWidthHalf, ViewOffsetY + ViewerActualHeightHalf);
        }

        private void DoTransform(double x, double y)
        {
            _scaleTransform.ScaleX = Scale.Value;
            _scaleTransform.ScaleY = Scale.Value;

            SetValue(ViewWidthProperty, (object)(ContentWidth * Scale.Value));
            SetValue(ViewHeightProperty, (object)(ContentHeight * Scale.Value));

            OffsetX = x;
            OffsetY = y;

            ViewOffsetX = (OffsetX * Scale.Value) + ViewerActualWidthHalf;
            ViewOffsetY = (OffsetY * Scale.Value) + ViewerActualHeightHalf;

            ChangePositionOfScrollBar();
        }
        private void ChangePositionOfScrollBar()
        {
            _scrollBarHorizontal.Visibility = (this.ActualWidth < ViewWidth) ? Visibility.Visible : Visibility.Hidden;
            if (_scrollBarHorizontal.IsVisible) // todo relocate to resize control event
            {
                _scrollBarHorizontal.ValueChanged -= DoScrollBarHorizontalValueChanged;
                _scrollBarHorizontal.Minimum = OffsetXMin;
                _scrollBarHorizontal.Maximum = OffsetXMax;
                _scrollBarHorizontal.Value = OffsetX;
                _scrollBarHorizontal.ValueChanged += DoScrollBarHorizontalValueChanged;
             }

            _scrollBarVertical.Visibility = (this.ActualHeight < ViewHeight) ? Visibility.Visible : Visibility.Hidden;
            if (_scrollBarVertical.IsVisible) // todo relocate to resize control event
            {
                _scrollBarVertical.ValueChanged -= DoScrollBarVerticalValueChanged;
                _scrollBarVertical.Minimum = OffsetYMin;
                _scrollBarVertical.Maximum = OffsetYMax;
                _scrollBarVertical.Value = OffsetY;
                _scrollBarVertical.ValueChanged += DoScrollBarVerticalValueChanged;
            }
        }
        
        private void DoSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DoTransform(_translateTransform.X + ((e.NewSize.Width - e.PreviousSize.Width) / 2), 
                        _translateTransform.Y + ((e.NewSize.Height - e.PreviousSize.Height) / 2));
            ChangePositionOfScrollBar();
        }
        
        private void DoScrollBarVerticalValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoTransform(_translateTransform.X, e.NewValue);
        }
        private void DoScrollBarHorizontalValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoTransform(e.NewValue, _translateTransform.Y);
        }
        
        #region Child Events
        private void DoMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoom = e.Delta > 0 ? Scale.Step : -Scale.Step;
            var scaleNew = MathHelper.Clamp(Scale.Value + zoom, Scale.Min, Scale.Max);

            if (scaleNew == Scale.Value)
              return;

            var p = e.GetPosition(this);
            p.X = (((_translateTransform.X - p.X) / Scale.Value) * scaleNew) + p.X;
            p.Y = (((_translateTransform.Y - p.Y) / Scale.Value) * scaleNew) + p.Y;

            if(PositionValidator != null)
                p = PositionValidator.TryValidateAndGetPosition(p);

            Scale.Value = scaleNew;
            DoTransform(p.X, p.Y);
        }

        private void DoMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startMouseMove = e.GetPosition(this);
            SelectedMapElement = null;
            this.Cursor = Cursors.Hand;
            _zoomContaner?.CaptureMouse();
        }
        private void DoMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            _zoomContaner?.ReleaseMouseCapture();
        }
        private void DoMouseMove(object sender, MouseEventArgs e)
        {
            if(this.Cursor == Cursors.Hand)
            {
                Vector v = e.GetPosition(this) - _startMouseMove;
                _startMouseMove = e.GetPosition(this);
                DoTransform(_translateTransform.X + v.X, _translateTransform.Y + v.Y);
            }
        }
        #endregion
    }
}
