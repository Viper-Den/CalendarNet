using Destiny.Core;
using System;
using System.Windows.Controls;

namespace MonthEvent
{
    public class LabelTitleControl : Label, ITitleControl
    {
        public TitleControlType Type { get; set; }
        public string Text
        {
            get => (String)Content;
            set { Content = value; }
        }
    }
}
