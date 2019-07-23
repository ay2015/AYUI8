/**----------------------------------------------- 
 * * ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权========= 
 * 
 * 作者：ay * 联系QQ：875556003 * 时间2019-06-14 
 * -----------------------------------------*/
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.Animate
{
    public class AyAniPulse : AyAnimateBaseIn
    {

        #region 构造函数
        public AyAniPulse(FrameworkElement _element)
         : base("pulse", _element)
        {
            base.AnimateSpeed = 900;
             Initialize();
        }
        public AyAniPulse(FrameworkElement _element, Action _completed)
            : base("pulse", _element, _completed)
        { base.AnimateSpeed = 900; }

        #endregion
        #region 属性

        private double scaleXDiff = 0.14;

        public double ScaleXDiff
        {
            get { return scaleXDiff; }
            set { scaleXDiff = value; }
        }

        private double scaleYDiff = 0.08;

        public double ScaleYDiff
        {
            get { return scaleYDiff; }
            set { scaleYDiff = value; }
        }

        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dauX = null;
        DoubleAnimationUsingKeyFrames dauY = null;

        #endregion

        protected override void Init()
        {
    
            SetBaseView();

            ScaleTransform translation = new ScaleTransform(1, 1);

            dauX = new DoubleAnimationUsingKeyFrames();
            dauY = new DoubleAnimationUsingKeyFrames();
            #region 基本工作，确定类型和name
            //是否存在TranslateTransform
            //动画要的类型是否存在
            //动画要的类型的name是否存在，不存在就注册，结束后取消注册，删除动画
            var ex = Element.RenderTransform;
            if (ex == null || (ex as System.Windows.Media.MatrixTransform) != null)
            {
                var tg = new TransformGroup();
                translation = new ScaleTransform(1, 1);
                Win.RegisterName(translation.GetHashCode().ToString(), translation);
                tg.Children.Add(translation);
                Element.RenderTransform = tg;
            }
            else
            {
                var tg = ex as TransformGroup;
                foreach (var item in tg.Children)
                {
                    translation = item as ScaleTransform;
                    if (translation != null)
                    {
                        break;
                    }
                }
                if (translation != null)
                {

                    var tex = translation.GetValue(FrameworkElement.NameProperty);
                    if (tex != null && tex.ToString() != "")
                    {

                    }
                    else
                    {
                        Win.RegisterName(translation.GetHashCode().ToString(), translation);
                    }
                }
                else
                {
                    translation = new ScaleTransform(1, 1);
                    Win.RegisterName(translation.GetHashCode().ToString(), translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion


            Win.RegisterResource(Story);


            double danqianX = translation.ScaleX;
            double danqianY = translation.ScaleY;
            Story = (Storyboard)Story.CloneCurrentValue();
            //var k1 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(0));
            var k2 = new EasingDoubleKeyFrame(danqianX + ScaleXDiff, TimeSpan.FromMilliseconds(AniTime(0.6)), new ExponentialEase { EasingMode = EasingMode.EaseOut });
            var k3 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(1)));
            Storyboard.SetTargetName(dauX, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauX, new PropertyPath(ScaleTransform.ScaleXProperty));
            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k3);
            Story.Children.Add(dauX);

            var k4 = new EasingDoubleKeyFrame(danqianY + ScaleYDiff, TimeSpan.FromMilliseconds(AniTime(0.6)), new ExponentialEase { EasingMode = EasingMode.EaseOut });
            var k5 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(AniTime(1)));
            Storyboard.SetTargetName(dauY, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauY, new PropertyPath(ScaleTransform.ScaleYProperty));
            dauY.KeyFrames.Add(k4);
            dauY.KeyFrames.Add(k5);
            Story.Children.Add(dauY);


            //<DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)" Storyboard.TargetName="textBlock">
            //    <EasingDoubleKeyFrame KeyTime="0" Value="1"/>
            //    <EasingDoubleKeyFrame KeyTime="0:0:0.6" Value="1.14">
            //        <EasingDoubleKeyFrame.EasingFunction>
            //            <ExponentialEase EasingMode="EaseOut"/>
            //        </EasingDoubleKeyFrame.EasingFunction>
            //    </EasingDoubleKeyFrame>
            //    <EasingDoubleKeyFrame KeyTime="0:0:0.9" Value="1"/>
            //</DoubleAnimationUsingKeyFrames>
            //<DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)" Storyboard.TargetName="textBlock">
            //    <EasingDoubleKeyFrame KeyTime="0" Value="1"/>
            //    <EasingDoubleKeyFrame KeyTime="0:0:0.6" Value="1.08">
            //        <EasingDoubleKeyFrame.EasingFunction>
            //            <ExponentialEase EasingMode="EaseOut"/>
            //        </EasingDoubleKeyFrame.EasingFunction>
            //    </EasingDoubleKeyFrame>
            //    <EasingDoubleKeyFrame KeyTime="0:0:0.9" Value="1"/>
            //</DoubleAnimationUsingKeyFrames>
            Story.Completed -= Story_Completed;
            Story.Completed += Story_Completed;
        }

        public override void Destroy()
        {
            Story.Completed -= Story_Completed;
            Win.UnRegisterNameAll();
            Win.UnRegisterResource(Story);
            dauX.KeyFrames.Clear();
            dauX = null;
            dauY.KeyFrames.Clear();
            dauY = null;
            Story = null;
        }


        private void Story_Completed(object sndr, EventArgs evtArgs)
        {
            try
            {
                Element.Opacity = 1;
                Element.Visibility = Visibility.Visible;
                base.CallClientCompleted();
            }
            catch
            {
            }
        }
    }
}
