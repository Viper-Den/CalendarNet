using System.Windows;

namespace ZoomPanel
{
    public interface IPositionValidator
    {
        Point TryValidateAndGetPosition(Point p);
    }
}
