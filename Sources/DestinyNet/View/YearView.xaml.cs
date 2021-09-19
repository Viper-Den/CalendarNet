using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DestinyNet;

namespace DestinyNetViews
{
    /// <summary>
    /// Interaction logic for YearView.xaml
    /// </summary>
    public partial class YearView : UserControl
    {
        public YearView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
            window.KeyUp += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                if (this.DataContext is YearViewModel)
                    ((YearViewModel)this.DataContext).IsMultipleSelection = e.IsDown;
            }
        }
    }
}
