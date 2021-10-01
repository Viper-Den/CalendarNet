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
    
    //public class HotkeysRegistrator
    //{
    //    [DllImport("User32.dll")]
    //    public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    //    [DllImport("User32.dll")]
    //    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    //    [DllImport("kernel32.dll")]
    //    public static extern Int16 GlobalAddAtom(string name);
    //    [DllImport("kernel32.dll")]
    //    public static extern Int16 GlobalDeleteAtom(Int16 nAtom);

    //    private IntPtr _windowHandle;
    //    private Dictionary<Int16, Action> _globalActions = new Dictionary<short, Action>();

    //    public HotkeysRegistrator(Window window)
    //    {
    //        _windowHandle = new WindowInteropHelper(window).Handle;
    //        HwndSource source = HwndSource.FromHwnd(_windowHandle);
    //        source.AddHook(WndProc);
    //    }

    //    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
    //    {
    //        if (msg == 0x0312)
    //        {
    //            //Обработка горячей клавиши
    //        }
    //        return IntPtr.Zero;
    //    }
    //    public bool RegisterGlobalHotkey(Action action, Keys commonKey, params ModifierKeys[] keys)
    //    {
    //        uint mod = keys.Cast<uint>().Aggregate((current, modKey) => current | modKey);
    //        short atom = GlobalAddAtom("OurAmazingApp" + (_globalActions.Count + 1));
    //        bool status = RegisterHotKey(_windowHandle, atom, mod, (uint)commonKey);

    //        if (status)
    //        {
    //            _globalActions.Add(atom, action);
    //        }
    //        return status;
    //    }
    //}
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
