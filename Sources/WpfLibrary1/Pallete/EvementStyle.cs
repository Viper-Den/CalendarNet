using System.Windows.Media;

namespace Destiny.Core
{
    public class EvementStyle: BaseViewModel
    {
        private EvementStyleType _type;
        public EvementStyle(SolidColorBrush background, SolidColorBrush foreground, EvementStyleType type)
        {
            _type = type;
            Background = background;
            Foreground = foreground;
        }

        public void SetEvementStyle(EvementStyle evementStyle)
        {
            Background = evementStyle.Background;
            Foreground = evementStyle.Foreground;
        }
        public string Name { get => _type.ToString(); }
        public SolidColorBrush Background { get; set; }
        public SolidColorBrush Foreground { get; set; }
    }
}
