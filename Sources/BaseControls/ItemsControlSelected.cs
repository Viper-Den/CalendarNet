using System;
using System.Collections;
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

namespace BaseControls
{
    public class ItemsControlSelected : ItemsControl
    {
        //static ItemsControlSelected()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemsControlSelected), new FrameworkPropertyMetadata(typeof(ItemsControlSelected)));
        //}

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ICommand), typeof(ItemsControlSelected), new PropertyMetadata(SelectedItemPropertyChanged));

        private static void SelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsControlSelected)d).SelectedItem = (ICommand)e.NewValue;
        }
        public ICommand SelectedItem
        {
            get { return (ICommand)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            //foreach (var v in Items)
            //{
            //    var c = v as Control;
            //    if(c != null)
            //    {
            //        c.MouseDoubleClick += DoMouseDoubleClick;
            //    }
            //}
        }
        protected void DoMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var c = sender as Control;
            if ((SelectedItem != null) && (c != null))
            {
                SelectedItem.Execute(c.DataContext);
            }
        }
    }
}
