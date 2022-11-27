
using System;

namespace MapControls.MapPoint
{
    public interface IMapPoint
    {
        double X { get; }
        double Y { get; }

        Action<IMapPoint> OnPositionChanged { get; set; }
    }
}
