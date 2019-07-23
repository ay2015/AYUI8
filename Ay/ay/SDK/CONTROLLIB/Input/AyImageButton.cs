using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ay.Controls
{
    public enum AyImageButtonMode {
        HorizonFour,
        VerticalFour,
        ContentOpacity,
        Manner,
        AllOpacity
    }
    public class AyImageButton:Button
    {
        static AyImageButton()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AyImageButton), new FrameworkPropertyMetadata(typeof(AyImageButton)));
        }

        public AyImageButtonMode RenderMode
        {
            get { return (AyImageButtonMode)GetValue(RenderModeProperty); }
            set { SetValue(RenderModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RenderMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RenderModeProperty =
            DependencyProperty.Register("RenderMode", typeof(AyImageButtonMode), typeof(AyImageButton), new PropertyMetadata(AyImageButtonMode.HorizonFour));




        public Stretch BackgroundStretch
        {
            get { return (Stretch)GetValue(BackgroundStretchProperty); }
            set { SetValue(BackgroundStretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundStretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundStretchProperty =
            DependencyProperty.Register("BackgroundStretch", typeof(Stretch), typeof(AyImageButton), new PropertyMetadata(Stretch.Uniform));


        /// <summary>
        /// Image4Button
        /// </summary>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(AyImageButton), new PropertyMetadata(null));



        public ImageSource HoverIcon
        {
            get { return (ImageSource)GetValue(HoverIconProperty); }
            set { SetValue(HoverIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverIconProperty =
            DependencyProperty.Register("HoverIcon", typeof(ImageSource), typeof(AyImageButton), new PropertyMetadata(null));



        public ImageSource PressedIcon
        {
            get { return (ImageSource)GetValue(PressedIconProperty); }
            set { SetValue(PressedIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedIconProperty =
            DependencyProperty.Register("PressedIcon", typeof(ImageSource), typeof(AyImageButton), new PropertyMetadata(null));




        public ImageSource DisabledIcon
        {
            get { return (ImageSource)GetValue(DisabledIconProperty); }
            set { SetValue(DisabledIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisabledIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisabledIconProperty =
            DependencyProperty.Register("DisabledIcon", typeof(ImageSource), typeof(AyImageButton), new PropertyMetadata(null));



    }
   

}
