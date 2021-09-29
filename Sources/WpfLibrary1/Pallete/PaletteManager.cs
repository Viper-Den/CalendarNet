using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Destiny.Core
{
    public class PaletteManager: BaseViewModel
    {
        private ObservableCollection<Palette> _paletteCollection;
        private Palette _selectedPalette;

        public PaletteManager(Palette defaultSelectedPalette)
        {
            DefaultSelectedPalette = defaultSelectedPalette ?? throw new NullReferenceException(nameof(defaultSelectedPalette));
            _paletteCollection = new ObservableCollection<Palette>();
            _selectedPalette = DefaultSelectedPalette;
        }
        public Palette SelectedPalette
        {
            get => _selectedPalette;
            private set { SetField(ref _selectedPalette, value); }
        }
        public Palette DefaultSelectedPalette { get; private set; }
    }
}
