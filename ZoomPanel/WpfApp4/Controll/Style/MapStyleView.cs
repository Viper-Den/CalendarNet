using System.Windows;
using System.Windows.Media;
using MapControls.Core;


namespace MapControls.MapStyle
{
    public class MapStyleView : BaseViewModel
    {
        private FontFamily _fontFamily = new FontFamily("Century Gothic");
        private FontStyle _fontStyle = FontStyles.Normal;
        private FontWeight _fontWeight = FontWeights.Black;
        private FontStretch _fontStretch = FontStretches.Normal;
        private double _fontSize = 30;

        public FontFamily FontFamily
        {
            get => _fontFamily;
            set { SetField(ref _fontFamily, value, nameof(FontFamily)); }
        }
        public FontStyle FontStyle
        {
            get => _fontStyle;
            set { SetField(ref _fontStyle, value, nameof(FontStyle)); }
        }
        public FontWeight FontWeight
        {
            get => _fontWeight;
            set { SetField(ref _fontWeight, value, nameof(FontWeight)); }
        }
        public FontStretch FontStretch
        {
            get => _fontStretch;
            set { SetField(ref _fontStretch, value, nameof(FontStretch)); }
        }
        public double FontSize
        {
            get => _fontSize;
            set { SetField(ref _fontSize, value, nameof(FontSize)); }
        }
    }
}
