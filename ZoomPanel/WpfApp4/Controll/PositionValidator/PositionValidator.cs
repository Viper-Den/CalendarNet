using System.Windows;
using ZoomPanel;

namespace MapControls.MapPositionValidator
{
    public class PositionValidator: IPositionValidator
    {
        private readonly int STEP_MOVE = 20;
        public Point TryValidateAndGetPosition(Point p)
        {
            p.X = ((int)(p.X / STEP_MOVE)) * STEP_MOVE;
            p.Y = ((int)(p.Y / STEP_MOVE)) * STEP_MOVE;
            return p;
        }
    }
}
