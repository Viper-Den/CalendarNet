using System;

namespace MapControls.MapPoint
{
    public class MapPoint: IMapPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Action<IMapPoint> OnPositionChanged { get; set; }
    }
}
