using System.Windows.Media;

namespace Destiny.Core
{
    public class ControlStyle
    {
        public ControlStyle(Brush background, Brush foreground)
        {
            Background = background;
            Foreground = foreground;
        }
        public Brush Foreground { get; set; }
        public Brush Background { get; set; }
    }
}
