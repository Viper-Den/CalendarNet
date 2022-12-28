using System.Windows;
using System.Windows.Controls;

namespace MapControls.MapText
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MapTextView : UserControl
    {
        public MapTextView()
        {
            InitializeComponent();
            Loaded += MapTextView_Loaded;
        }

        private void MapTextView_Loaded(object sender, RoutedEventArgs e)
        {
            Focus();
        }
    }
}
