using System.Windows;
using System.Windows.Controls;

namespace MonthEvent
{
        public class BaseControl : Control
        {
            static BaseControl()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseControl), new FrameworkPropertyMetadata(typeof(BaseControl)));
            }

            #region ItemTemplate
            public static readonly DependencyProperty ItemTemplateProperty =
                    DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(BaseControl),
                            new FrameworkPropertyMetadata((DataTemplate)null, new PropertyChangedCallback(OnItemTemplateChanged)));
            public DataTemplate ItemTemplate
            {
                get { return (DataTemplate)GetValue(ItemTemplateProperty); }
                set { SetValue(ItemTemplateProperty, value); }
            }
            private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                ((BaseControl)d).ItemTemplate = (DataTemplate)e.NewValue;
            }
            #endregion
        }
    }
