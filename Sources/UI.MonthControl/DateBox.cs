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
    public class DateBox : TextBox
    {
        public DateBox() : base()
        {
            ContextMenu = null;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, SuppressCommand));
            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, null, SuppressCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, SuppressCommand));
        }

        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DateBox), new PropertyMetadata(TimePropertyChanged));

        public static void TimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateBox)d).Date = (DateTime)e.NewValue;
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set
            {
                SetValue(DateProperty, value);
                Text = value.ToString("dd.MM.yyyy");
            }
        }
        #endregion

        private void SuppressCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if ((e.Key == Key.Back) || (e.Key == Key.Delete))
                e.Handled = true;
            base.OnPreviewKeyDown(e);
        }
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = !DateCheck(e.Text);
            base.OnTextInput(e);
        }
        private bool DateCheck(string text)
        {
            if (SelectionLength == Text.Length)
                SelectionLength = 0;
            if (SelectionLength == 0)
            {
                if (SelectionStart == Text.Length)
                    SelectionStart = Text.Length - 1;
                if (SelectionStart == 2)
                    SelectionStart = 3;
                if (SelectionStart == 5)
                    SelectionStart = 6;
                SelectionLength = 1;
            }
            var s = Text.Remove(SelectionStart, SelectionLength);
            s = s.Insert(SelectionStart, text);
            var isDate = DateTime.TryParseExact(s, "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out var d);
            if (isDate)
                SetValue(DateProperty, DateTime.ParseExact(s, "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None));
            return isDate;
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
        }

    }
}
