using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace UIMonthControl
{
    public class TextBoxFormat : TextBox
    {
        #region Time
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(TextBoxFormat), new PropertyMetadata(TimePropertyChanged));

        public static void TimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TextBoxFormat)d).Time = (DateTime)e.NewValue;
        }
        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set
            {
                SetValue(TimeProperty, value);
            }
        }
        #endregion
        public TextBoxFormat() : base()
        {
            ContextMenu = null;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, SuppressCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, null, SuppressCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, SuppressCommand));
        }

        private void SuppressCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
        }

        public void OnPasteCommand(object o, ExecutedRoutedEventArgs e)
        {
            //e.Handled = false;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if ((e.Key == Key.Back) || (e.Key == Key.Delete))
                e.Handled = true;
            base.OnPreviewKeyDown(e);
        }
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            if (SelectionLength == Text.Length)
                SelectionLength = 0;
            if (SelectionLength == 0)
            {   
                if(SelectionStart == Text.Length)
                    SelectionStart = Text.Length - 1;
                if (SelectionStart == 2)
                    SelectionStart = 3;
                 SelectionLength = 1;
            }
            var s = Text.Remove(SelectionStart, SelectionLength);
            s = s.Insert(SelectionStart, e.Text);
            e.Handled = IsValidTime(s);
            base.OnTextInput(e);
        }

        private bool IsValidTime(string text)
        {
            DateTime d;
            return !DateTime.TryParseExact(text, "HH:mm", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out d);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
        }



}
}
