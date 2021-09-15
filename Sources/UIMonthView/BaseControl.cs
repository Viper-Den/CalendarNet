using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MonthEvent
{
    public class ContentTemplate : ContentControl
    {

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate(); 
            Label myLabel = (Label)this.FindName("Label");
            foreach (var b in GetChildrens())
            {
                b.ClearValue(FrameworkElement.DataContextProperty);
                b.SetValue(FrameworkElement.DataContextProperty, DataContext);

            }
        }
        public List<DependencyObject> GetChildrens()
        {
            
            return GetChildren(this);
        }
        protected List<DependencyObject> GetChildren(DependencyObject parent)
        {
            var list = new List<DependencyObject> { };
            var i = VisualTreeHelper.GetChildrenCount(parent);
            for (int count = 0; count < i; count++)
            {
                var child = VisualTreeHelper.GetChild(parent, count);
                var b = (child is FrameworkElement);
                b = (child is Control);
                list.Add(child);
                list.AddRange(GetChildren(child));
            }
            return list;
        }
    }
        public class BaseControl : Control
        {
            static BaseControl()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseControl), new FrameworkPropertyMetadata(typeof(BaseControl)));
            }

            #region ItemTemplate
            /// <summary>
            ///     The DependencyProperty for the ItemTemplate property.
            ///     Flags:              none
            ///     Default Value:      null
            /// </summary>
            public static readonly DependencyProperty ItemTemplateProperty =
                    DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(BaseControl),
                            new FrameworkPropertyMetadata((DataTemplate)null, new PropertyChangedCallback(OnItemTemplateChanged)));

            /// <summary>
            ///     ItemTemplate is the template used to display each item.
            /// </summary>
            public DataTemplate ItemTemplate
            {
                get { return (DataTemplate)GetValue(ItemTemplateProperty); }
                set { SetValue(ItemTemplateProperty, value); }
            }

            /// <summary>
            ///     Called when ItemTemplateProperty is invalidated on "d."
            /// </summary>
            /// <param name="d">The object on which the property was invalidated.</param>
            /// <param name="e">EventArgs that contains the old and new values for this property</param>
            private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                ((BaseControl)d).ItemTemplate = (DataTemplate)e.NewValue;
            }
            #endregion
        }
    }
