using System;
using System.Windows;

namespace Destiny.Core
{
    public interface IPalette
    {
        string Name { get; }
        EvementStyle DayNoMonth { get; }
        EvementStyle DayOff { get; }
        EvementStyle DayFinish { get; }
        EvementStyle DayOffFinish { get; }
        EvementStyle Selected { get; }
        EvementStyle SelectedDefault { get; }
        EvementStyle ToDay { get; }
        EvementStyle Day { get; }
        Visibility ViewBorderingMonths { get; }
        void PaintTitle(ITitleControl control, DateTime date);
        void PaintDay(IDayControl control, DateTime date);
    }
}
