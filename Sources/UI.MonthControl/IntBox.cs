using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace UIMonthControl
{
    public class IntBox : TextBox
    {
        public IntBox() : base()
        {
            ContextMenu = null;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, SuppressCommand));
            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, null, SuppressCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, SuppressCommand));
            Text = "0";
        }
        private void SuppressCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
        }

        #region Value
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(IntBox), new PropertyMetadata(ValuePropertyChanged));

        public static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IntBox)d).Value = (int)e.NewValue;
        }
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                Text = value.ToString();
            }
        }
        #endregion

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = !IntCheck(e.Text);
            base.OnTextInput(e);
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (Text == "")
                Value = 0;
            base.OnTextChanged(e);
        }
        private bool IntCheck(string text)
        {
            if (Text == "0")
            {
                SelectionLength = 0;
                SelectionLength = 1;
            }
            var s = Text.Remove(SelectionStart, SelectionLength);
            s = s.Insert(SelectionStart, text);
            return int.TryParse(s, out var i);
        }
    }
}
