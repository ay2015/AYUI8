/**----------------------------------------------- 
 * * ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权========= 
 * 
 * 作者：ay * 联系QQ：875556003
 * 时间2019-06-14
 * -----------------------------------------*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.Animate
{

    public class AyAniColor : AyAnimateTypeBase
    {
        public AyAniColor()
        {
            AnimateName = "Color";
            base.AnimateSpeed = 600;
        }
        public AyAniColor(FrameworkElement _element)
            : base("Color", _element)
        { base.AnimateSpeed = 600; }

        public AyAniColor(FrameworkElement _element, Action _completed)
            : base("Color", _element, _completed)
        { base.AnimateSpeed = 600; }

        public AyAniColor(FrameworkElement _element, Color? fromColor, Color? toColor, PropertyPath propertyPath)
    : base("Color", _element)
        {
            base.AnimateSpeed = 600;
            this.FromColor = fromColor;
            this.ToColor = toColor;
            this.AniPropertyPath = propertyPath;
            Initialize();
        }




        public Color? ToColor
        {
            get { return (Color?)GetValue(ToColorProperty); }
            set { SetValue(ToColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToColorProperty =
            DependencyProperty.Register("ToColor", typeof(Color?), typeof(AyAniColor), new FrameworkPropertyMetadata(Colors.Transparent, new PropertyChangedCallback(ToColorChanged)));

        private static void ToColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AyAniColor aa)
            {
                aa.ReInitialize();
            }
        }

        public Color? FromColor
        {
            get { return (Color?)GetValue(FromColorProperty); }
            set { SetValue(FromColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromColorProperty =
            DependencyProperty.Register("FromColor", typeof(Color?), typeof(AyAniColor), new PropertyMetadata(Colors.Transparent, new PropertyChangedCallback(FromColorChanged)));

        private static void FromColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
                       if (d is AyAniColor aa)
            {
                aa.ReInitialize();
            }
        }

        public override void CreateStoryboard()
        {
            ColorAnimationUsingKeyFrames dau = new ColorAnimationUsingKeyFrames();

            EasingColorKeyFrame fromk = null;
            if (FromColor.HasValue)
            {
                fromk = new EasingColorKeyFrame(FromColor.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingColorKeyFrame tok = null;
            if (ToColor.HasValue)
            {
                tok = new EasingColorKeyFrame(ToColor.Value, TimeSpan.FromMilliseconds(AniTime(1)));
                dau.KeyFrames.Add(tok);
            }


            if (AniEasingFunction != null)
            {
                if (fromk != null) fromk.EasingFunction = AniEasingFunction;
                if (tok != null) tok.EasingFunction = AniEasingFunction;
            }
            else if (CirDefault != null)
            {
                if (fromk != null) fromk.EasingFunction = CirDefault;
                if (tok != null) tok.EasingFunction = CirDefault;
            }
      
            //Storyboard.SetTargetName(dau, ElementName);
            Storyboard.SetTarget(dau, Element);
            Storyboard.SetTargetProperty(dau, AniPropertyPath);
            Story.Children.Add(dau);
        }
        private PropertyPath _SampleBackgroundPropertyPath;
        /// <summary>
        /// 示例容器背景路径
        /// (Panel.Background).(SolidColorBrush.Color)
        /// </summary>
        public PropertyPath SampleBackgroundPropertyPath
        {
            get
            {
                if (_SampleBackgroundPropertyPath == null)
                {
                    _SampleBackgroundPropertyPath = new PropertyPath("(Panel.Background).(SolidColorBrush.Color)");
                }
                return _SampleBackgroundPropertyPath;
            }
        }
        private PropertyPath _SampleBorderBrushPropertyPath;
        /// <summary>
        /// 示例描边路径
        /// (Border.BorderBrush).(SolidColorBrush.Color)
        /// </summary>
        public PropertyPath SampleBorderBrushPropertyPath
        {
            get
            {
                if (_SampleBorderBrushPropertyPath == null)
                {
                    _SampleBorderBrushPropertyPath = new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)");
                }
                return _SampleBorderBrushPropertyPath;
            }
        }

        private PropertyPath _SampleFillPropertyPath;
        /// <summary>
        /// 示例颜色路径
        /// (Shape.Fill).(SolidColorBrush.Color)
        /// </summary>
        public PropertyPath SampleFillPropertyPath
        {
            get
            {
                if (_SampleFillPropertyPath == null)
                {
                    _SampleFillPropertyPath = new PropertyPath("(Shape.Fill).(SolidColorBrush.Color)");
                }
                return _SampleFillPropertyPath;
            }
        }
        private PropertyPath _SampleStrokePropertyPath;
        /// <summary>
        /// 示例Shape描边路径
        ///(Shape.Stroke).(SolidColorBrush.Color)
        /// </summary>
        public PropertyPath SampleStrokePropertyPath
        {
            get
            {
                if (_SampleStrokePropertyPath == null)
                {
                    _SampleStrokePropertyPath = new PropertyPath("(Shape.Stroke).(SolidColorBrush.Color)");
                }
                return _SampleStrokePropertyPath;
            }
        }
        private PropertyPath _SampleForegroundPropertyPath;
        /// <summary>
        /// 示例字体颜色路径
        ///(TextElement.Foreground).(SolidColorBrush.Color)
        /// </summary>
        public PropertyPath SampleForegroundPropertyPath
        {
            get
            {
                if (_SampleForegroundPropertyPath == null)
                {
                    _SampleForegroundPropertyPath = new PropertyPath("(TextElement.Foreground).(SolidColorBrush.Color)");
                }
                return _SampleForegroundPropertyPath;
            }
        }
    }
}
