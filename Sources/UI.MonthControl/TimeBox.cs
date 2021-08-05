using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace UIMonthControl
{
    public class TimeBox : TextBox
    {
        public TimeBox() : base()
        {
            ContextMenu = null;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, SuppressCommand));
            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, null, SuppressCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, SuppressCommand));
        }

        #region Time
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(TimeBox), new PropertyMetadata(TimePropertyChanged));

        public static void TimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TimeBox)d).Time = (DateTime)e.NewValue;
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

        private void SuppressCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = ((e.Key == Key.Back) || (e.Key == Key.Delete));
            base.OnPreviewKeyDown(e);
        }
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = !TimeCheck(e.Text);
            base.OnTextInput(e);
        }
        private bool TimeCheck(string text)
        {
            if (SelectionLength == Text.Length)
                SelectionLength = 0;
            if (SelectionLength == 0)
            {
                if (SelectionStart == Text.Length)
                    SelectionStart = Text.Length - 1;
                if (SelectionStart == 2)
                    SelectionStart = 3;
                SelectionLength = 1;
            }
            var s = Text.Remove(SelectionStart, SelectionLength);
            s = s.Insert(SelectionStart, text);
            return DateTime.TryParseExact(s, "HH:mm", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out var d);
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
        }

    }
}
