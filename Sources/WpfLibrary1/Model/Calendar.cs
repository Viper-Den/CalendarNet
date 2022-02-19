using System;
using System.Windows.Media;

namespace Destiny.Core
{
    public class Calendar: BaseViewModel
    {
        private bool _enabled;
        private string _gUID;
        private string _name;
        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        public Calendar()
        {
            GUID = Guid.NewGuid().ToString();
            _background = new SolidColorBrush();
            _foreground = new SolidColorBrush();
        }
        public string Name
        {
            get => _name;
            set { SetField(ref _name, value); }
        }
        public SolidColorBrush Background
        {
            get => _background;
            set { SetField(ref _background, value); }
        }
        public SolidColorBrush Foreground
        {
            get => _foreground;
            set { SetField(ref _foreground, value); }
        }
        public bool Enabled 
        { 
            get => _enabled; 
            set { SetField(ref _enabled, value); } 
        }
        public string GUID
        {
            get => _gUID;
            set { SetField(ref _gUID, value); }
        }
    }
}
