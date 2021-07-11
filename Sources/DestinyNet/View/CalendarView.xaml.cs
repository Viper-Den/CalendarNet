using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
        public static readonly DependencyProperty EditCalenarProperty =
           DependencyProperty.Register("EditCalenar", typeof(ICommand),
               typeof(CalendarView), new PropertyMetadata(EditCalenarPropertyChanged));

        private static void EditCalenarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CalendarView)d).EditCalenar = (ICommand)e.NewValue;
        }
        public ICommand EditCalenar
        {
            get { return (ICommand)GetValue(EditCalenarProperty); }
            set
            {
                SetValue(EditCalenarProperty, value);
            }
        }
        public CalendarView()
        {
            InitializeComponent();
        }

        private void CheckBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(EditCalenar != null)
            {
                EditCalenar.Execute(DataContext);
            }
        }
    }

}
