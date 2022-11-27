using MapControls.Core;
using System;
using System.Windows.Media;
using System.Windows.Shapes;
using MapControls.MapPoint;

namespace MapControls.MapLine
{
    public class MapLinesViewModel : BaseViewModel
    {
        private IMapPoint _start;
        private IMapPoint _finish;
        private double _left;
        private double _top;
        private double _Width;
        private double _Height;
        public Path Path { get; set; } = new Path();
        public double Left { get => _left - 10; set { SetField(ref _left, value, nameof(Left)); }}
        public double Top { get => _top - 10; set { SetField(ref _top, value, nameof(Top)); } }
        public double Width { get => _Width + 10; set { SetField(ref _Width, value, nameof(Width)); } }
        public double Height { get => _Height + 10; set { SetField(ref _Height, value, nameof(Height)); } }
        public IMapPoint Start 
        { 
            get => _start; 
            set 
            {
                if (_start == value)
                    return;

                if ((value == null) && (_start != null))
                    _start.OnPositionChanged -= DoPositionChanged;

                _start = value;

                if (_start != null)
                    _start.OnPositionChanged += DoPositionChanged;

                OnUpdatedMapPoint(_start);
            }
        }
        public IMapPoint Finish
        { 
            get => _finish; 
            set 
            {
                if (_finish == value)
                    return;

                if ((value == null) && (_finish != null))
                    _finish.OnPositionChanged -= DoPositionChanged;

                _finish = value;

                if (_finish != null)
                    _finish.OnPositionChanged += DoPositionChanged;

                OnUpdatedMapPoint(_finish);
            }
        }

        private MapPointType GetPointType(IMapPoint p)
        {
            return MapPointType.Left;
        }
        
        private void DoPositionChanged(IMapPoint p)
        {
            OnUpdatedMapPoint(p);
        }

        private void OnUpdatedMapPoint(IMapPoint p)
        {
            if ((Start == null) || (Finish == null))
                return;

            Left = Math.Min(Start.X, Finish.X);
            Top = Math.Min(Start.Y, Finish.Y);
            Width = (Math.Max(Start.X, Finish.X) - Left);
            Height = (Math.Max(Start.Y, Finish.Y) - Top);
            var x = Math.Round(Start.X - Left);
            var y = Math.Round(Start.Y - Top);
            var x2 = Math.Round(Finish.X - Left);
            var y2 = Math.Round(Finish.Y - Top);

            if ((Double.IsNaN(x)) || (Double.IsNaN(y)) || (Double.IsNaN(x2)) || (Double.IsNaN(y2)))
                return;

            switch (GetPointType(Start))
            {
                case MapPointType.Left:
                    Path.Data = Geometry.Parse($"M{x},{y} L{ Math.Round(x - ((x - x2) / 2))},{y} L{ Math.Round(x - ((x - x2) / 2))},{y2} L{x2},{y2}");
                    break;
                case MapPointType.Top:
                    Path.Data = Geometry.Parse($"M{x},{y} L{x},{ Math.Round(y - ((y - y2) / 2))} L{x2},{ Math.Round(y - ((y - y2) / 2))} L{x2},{y2}");
                    break;
                case MapPointType.Right:
                    Path.Data = Geometry.Parse($"M{x},{y} L{ Math.Round(x + ((x2 - x) / 2))},{y} L{ Math.Round(x + ((x2 - x) / 2))},{y2} L{x2},{y2}");
                    break;
                case MapPointType.Bottom:
                    Path.Data = Geometry.Parse($"M{x},{y} L{x},{ Math.Round(y + ((y2 - y) / 2))} L{x2},{ Math.Round(y + ((y2 - y) / 2))} L{x2},{y2}");
                    break;
                default:
                    break;

            }
        }
    }
}
