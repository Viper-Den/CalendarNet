using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Destiny.UI.Controls
{
    public class IconsPack : Control
    {
        private const string InvertedIconPrefix = "Inverted";
        private static ResourceDictionary iconsResource;

        static IconsPack()
        {
            iconsResource = GetStaticResourceDictionary();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconsPack), new FrameworkPropertyMetadata(typeof(IconsPack)));
        }

        #region Command
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(IconsPack), new PropertyMetadata(AddEventPropertyChanged));
        public static void AddEventPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IconsPack)d).Command = (ICommand)e.NewValue;
        }
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion
        #region KindIcon
        //public static readonly DependencyProperty KindIconProperty = DependencyProperty.Register(nameof(IconKind), typeof(IconType), typeof(IconsPack),
        //       new FrameworkPropertyMetadata(default(IconType), FrameworkPropertyMetadataOptions.AffectsRender, KindChangedCallBack));

        public static readonly DependencyProperty KindIconProperty =  DependencyProperty.Register(nameof(IconKind), typeof(IconType), typeof(IconsPack), new PropertyMetadata(KindIconPropertyChanged));
        private static void KindIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IconsPack)d).IconKind = (IconType)e.NewValue;
        }        
        public IconType IconKind
        {
            get => (IconType)GetValue(KindIconProperty);
            set 
            { 
                SetValue(KindIconProperty, value);
                UpdateData();
            }
        }
        private void UpdateData()
        {
            string iconKey = IsInverted ? InvertedIconPrefix + IconKind.ToString() : IconKind.ToString();
            Image = GetIcon(iconKey);
        }
        #endregion
        #region Image
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(IconsPack), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        #endregion
        #region IsInverted
        public static readonly DependencyProperty IsInvertedProperty =
            DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(IconsPack), new PropertyMetadata(KindChangedCallBack));
        private static void KindChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IconsPack)d).IsInverted = (bool)e.NewValue;
        }
        public bool IsInverted
        {
            get { return (bool)GetValue(IsInvertedProperty); }
            set 
            {
                SetValue(IsInvertedProperty, value);
                UpdateData();
            }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateData();
            this.PreviewMouseLeftButtonDown += DoPreviewMouseLeftButtonUp;
        }
        
        private void DoPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Command?.Execute(sender);
        }

        private static ResourceDictionary GetStaticResourceDictionary()
        {
            var uri = new Uri($"pack://application:,,,/Destiny.UI;component/Themes/Icons.xaml");
            return new ResourceDictionary { Source = uri };
        }

        private ImageSource GetIcon(string iconKey)
        {
            DrawingImage image = null;

            if (iconsResource.Contains(iconKey) && iconsResource[iconKey] is DrawingImage drawingImage)
                image = drawingImage;
            else
                Console.WriteLine($"Icon '{iconKey}' is not found");

            return image;
        }
    }
}
