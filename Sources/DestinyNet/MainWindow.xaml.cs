using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Interop;
using System.Windows.Forms;

namespace DestinyNet
{
    
    public partial class MainWindow : Window
    {

        // https://www.codeproject.com/Articles/36468/WPF-NotifyIcon-2
        private bool _cancelClosing = true; 
        public MainWindow()
        {
            InitializeComponent();
            Closing += DoClosing;
        }
        ~MainWindow()
        {
            Closing -= DoClosing;
        }

        private void DoClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = _cancelClosing;
            Hide();
        }

        private void TaskbarIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _cancelClosing = false;
            Close();
        }
    }
}
