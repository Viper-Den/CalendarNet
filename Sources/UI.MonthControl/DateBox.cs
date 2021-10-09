using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UIMonthControl
{
    public class DateBox : TextBox
    {
        private int[] _pos = { 0, 1, 3, 3, 4, 6, 6, 7, 8, 9, 9 };
        private int[] _posNew = { 1, 3, 3, 4, 6, 6, 7, 8, 9, 9 }; // 11.11.1111
        public DateBox() : base()
        {
            ContextMenu = null;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, SuppressCommand));
            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, null, SuppressCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, SuppressCommand));
        }

        #region Date
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(DateBox), new PropertyMetadata(TimePropertyChanged));

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
                OnDateChanged?.Invoke(this);
            }
        }
        #endregion

        public Action<DateBox> OnDateChanged { get; set; }
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
            e.Handled = true;
            DateCheck(e.Text);
            base.OnTextInput(e);
        }
        private bool DateCheck(string text)
        {
            SelectionStart = _pos[SelectionStart];
            SelectionLength = 1;
            var s = Text.Remove(SelectionStart, SelectionLength);
            var selectionStart = SelectionStart;
            s = s.Insert(SelectionStart, text);

            var isDate = DateTime.TryParseExact(s, "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out var d);
            if (isDate)
            {
                Text = s;
                SetValue(DateProperty, DateTime.ParseExact(s, "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None));
                SelectionStart = _posNew[selectionStart];
            }
            return isDate;
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
        }
    }
}
