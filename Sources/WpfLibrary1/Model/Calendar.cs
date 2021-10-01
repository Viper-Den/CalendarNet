using System;
using System.Windows.Media;

namespace Destiny.Core
{
    public class Calendar: BaseViewModel
    {
        private bool _enabled;
        public Calendar()
        {
            GUID = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public SolidColorBrush Background { get; set; }
        public SolidColorBrush Foreground { get; set; }
        public bool Enabled { get => _enabled; set { SetField(ref _enabled, value); } }
        public string GUID { get; set; }
    }
}
