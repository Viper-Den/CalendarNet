using System;
using System.Collections.Generic;
using System.Windows;
using MapModels;

namespace ZoomPanel
{
    public class MapBindControl : IMapBindControl
    {
        private Rect _position;
        public List<IMapPoint> Points { get; } = new List<IMapPoint>();
        public MapBindControl()
        {
            Points.Add(new MapPointControl() { Type = MapPointType.Left });
            Points.Add(new MapPointControl() { Type = MapPointType.Top });
            Points.Add(new MapPointControl() { Type = MapPointType.Right });
            Points.Add(new MapPointControl() { Type = MapPointType.Bottom });
            foreach(var p in Points)
            {
                p.OnSelectedMapPoint += DoSelectedMapPoint;
                p.Radius = 10;
            }
        }
        private void DoSelectedMapPoint(IMapPoint p)
        {
            OnSelectedMapPoint?.Invoke(p);
        }
        public Action<IMapPoint> OnSelectedMapPoint { get; set; }
        public Rect Position
        {
            get => _position;
            set
            {
                _position = value;
                var w2 = (_position.Width / 2);
                var h2 = (_position.Height / 2);
                Points[0].Center = new Point(_position.Left, _position.Top + h2);
                Points[1].Center = new Point(_position.Left + w2, _position.Top);
                Points[2].Center = new Point(_position.Right, _position.Top + h2);
                Points[3].Center = new Point(_position.Left + w2, _position.Bottom);
            }
        }        
    }
}
