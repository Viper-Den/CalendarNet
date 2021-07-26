using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DestinyNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected ManagerViewModel _ManagerViewModel;

        public MainWindow()
        {
            InitializeComponent();
            //DataContext = managerViewModel;
            //_ManagerViewModel = managerViewModel;
        }

        private void ButtonMonth_Click(object sender, RoutedEventArgs e)
        {
            _ManagerViewModel.SelectiewModelEnum = ViewModelEnum.Month;
        }

        private void ButtonWeek_Click(object sender, RoutedEventArgs e)
        {
            _ManagerViewModel.SelectiewModelEnum = ViewModelEnum.Week;
        }
        private void ButtonYear_Click(object sender, RoutedEventArgs e)
        {
            _ManagerViewModel.SelectiewModelEnum = ViewModelEnum.Year;
        }
        private void ButtonToDo_Click(object sender, RoutedEventArgs e)
        {
            _ManagerViewModel.SelectiewModelEnum = ViewModelEnum.ToDo;
        }
    }
}
