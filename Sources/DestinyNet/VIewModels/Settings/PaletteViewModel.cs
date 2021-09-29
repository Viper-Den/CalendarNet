using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Destiny.Core;

namespace DestinyNet
{
    public class PaletteViewModel : BaseViewModel
    {
        public PaletteViewModel(Palette palette)
        {
            Palette = palette ?? throw new NullReferenceException(nameof(palette));
        }
        public Palette Palette{ get; }
    }
}
