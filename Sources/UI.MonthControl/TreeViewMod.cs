using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UIMonthControl
{
    public class TreeViewMod : TreeView
    {
        #region SelectedItemCommand
        public static readonly DependencyProperty SelectedItemCommandProperty = DependencyProperty.Register(nameof(SelectedItemCommand),
                typeof(ICommand), typeof(TreeViewMod), new PropertyMetadata(SelectedItemCommandChanged));

        private static void SelectedItemCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TreeViewMod)d).SelectedItemCommand = (ICommand)e.NewValue;
        }
        public ICommand SelectedItemCommand
        {
            get { return (ICommand)GetValue(SelectedItemCommandProperty); }
            set { SetValue(SelectedItemCommandProperty, value); }
        }
        #endregion

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemCommand?.Execute(e.NewValue);
        }
    }
}
