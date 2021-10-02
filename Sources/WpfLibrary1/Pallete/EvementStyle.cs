using System.Windows.Media;

namespace Destiny.Core
{
    public class EvementStyle: BaseViewModel
    {
        public EvementStyle(Brush background, Brush foreground)
        {
            Background = background;
            Foreground = foreground;
        }

        public void SetEvementStyle(EvementStyle evementStyle)
        {
            Background = evementStyle.Background;
            Foreground = evementStyle.Foreground;
        }
        public Brush Background { get; set; }
        public Brush Foreground { get; set; }
    }
}
