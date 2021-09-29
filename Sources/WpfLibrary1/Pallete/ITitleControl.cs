using System.Windows;
using System.Windows.Media;

namespace Destiny.Core
{
    public interface ITitleControl
    {
        Brush Background { get; set; }
        Brush Foreground { get; set; }
        string Text { get; set; }
        TitleControlType Type { get; set; }
        Visibility Visibility { get; set; }

    }
}
