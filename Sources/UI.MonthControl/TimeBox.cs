using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace UIMonthControl
{
    public class TimeBox : TextBox
    {
        private int[] _pos = { 0, 1, 3, 3, 4 };
        private int[] _posNew = { 1, 3, 3, 4, 0 }; // 11:11
        public TimeBox() : base()
        {
            ContextMenu = null;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, SuppressCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, SuppressCommand));
        }

        #region Time
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time), typeof(DateTime), typeof(TimeBox), new PropertyMetadata(TimePropertyChanged));

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
                Text = value.ToString("HH:mm");
                OnTimeChanged?.Invoke(this);
            }
        }
        #endregion
        public Action<TimeBox> OnTimeChanged { get; set; }
        private void SuppressCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = ((e.Key == Key.Back) || (e.Key == Key.Delete));
            base.OnPreviewKeyDown(e);
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
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
            var isTime = DateTime.TryParseExact(s, "HH:mm", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out var d);
            if (isTime)
            {
                Text = s;
                SetValue(TimeProperty, DateTime.ParseExact(s, "HH:mm", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None));
                SelectionStart = _posNew[selectionStart];
            }
            return isTime;
        }
    }
}
