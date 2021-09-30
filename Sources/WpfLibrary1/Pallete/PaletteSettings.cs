using System.Collections.ObjectModel;

namespace Destiny.Core
{
    public class PaletteSettings
    {
        public PaletteSettings() { 
            PaletteCollection = new ObservableCollection<Palette>();
            Default = new Palette();
        }
        public Palette Default { get; set; }
        public ObservableCollection<Palette> PaletteCollection { get; set; }
    }
}
