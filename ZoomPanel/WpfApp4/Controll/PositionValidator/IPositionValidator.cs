using System.Windows;

namespace MapControls.MapPositionValidator
{
    public interface IPositionValidator
    {
        Point TryValidateAndGetPosition(Point p);
    }
}
