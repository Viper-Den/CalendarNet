using System;
using System.Windows;
using System.Windows.Media;

namespace Destiny.Core
{
    public interface IDayControl
    {
        Visibility Visibility { get; set; }
        Brush Foreground { get; set; }
        Brush Background { get; set; }
        DateTime Date { get; set; }
        DayType Type { get; }
    }
}
