using Destiny.Core;
using System;
using System.Windows;

namespace DestinyNet.ViewModels
{
    public class GeneralSettingsViewModel : BaseViewModel
    {
        private WindowSettings _settings;

        public GeneralSettingsViewModel(WindowSettings windowSettings)
        {
            _settings = windowSettings ?? throw new ArgumentNullException(nameof(windowSettings));
        }
        public string GUIDStyle
        {
            get => _settings.GUIDStyle;
            set
            {
                _settings.GUIDStyle = value;
                OnPropertyChanged(GUIDStyle);
            }
        }
        public int Left
        {
            get => _settings.Left;
            set
            {
                _settings.Left = value;
                OnPropertyChanged(Left);
            }
        }
        public int Top
        {
            get => _settings.Top;
            set
            {
                _settings.Top = value;
                OnPropertyChanged(Top);
            }
        }
        public WindowState WindowState { get; set; }
    }
    }
