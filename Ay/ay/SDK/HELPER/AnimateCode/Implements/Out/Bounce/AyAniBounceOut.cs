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
    public class AyAniBounceOut : AyAnimateBaseOut
    {
        #region 构造函数
        public AyAniBounceOut(FrameworkElement _element)
    : base("BounceOut", _element)
        { base.AnimateSpeed = 600; }

        public AyAniBounceOut(FrameworkElement _element, Action _completed)
            : base("BounceOut", _element, _completed)
        {
            base.AnimateSpeed = 600;
        }
        #endregion
        #region 属性

        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dauX = null;
        DoubleAnimationUsingKeyFrames dauY = null;
        DoubleAnimationUsingKeyFrames dauOpacty = null;
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
            Story = (Storyboard)Story.CloneCurrentValue();
            double danqianX = translation.ScaleX;
            double danqianY = translation.ScaleY;


            var k2 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(0));
            var k2_0 = new EasingDoubleKeyFrame(0.9, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k2_1 = new EasingDoubleKeyFrame(1.1, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k2_2 = new EasingDoubleKeyFrame(1.1, TimeSpan.FromMilliseconds(AniTime(0.55)));
            var k2_4 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)));

            dauX.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTargetName(dauX, Win.GetName(translation));


            Storyboard.SetTargetProperty(dauX, new PropertyPath(ScaleTransform.ScaleXProperty));
            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k2_0);
            dauX.KeyFrames.Add(k2_1);
            dauX.KeyFrames.Add(k2_2);
            dauX.KeyFrames.Add(k2_4);
            Story.Children.Add(dauX);

            dauY.FillBehavior = FillBehavior.Stop;

            Storyboard.SetTargetName(dauY, Win.GetName(translation));


            Storyboard.SetTargetProperty(dauY, new PropertyPath(ScaleTransform.ScaleYProperty));
            dauY.KeyFrames.Add(k2);
            dauY.KeyFrames.Add(k2_0);
            dauY.KeyFrames.Add(k2_1);
            dauY.KeyFrames.Add(k2_2);
            dauY.KeyFrames.Add(k2_4);
            Story.Children.Add(dauY);

            dauOpacty = new DoubleAnimationUsingKeyFrames();
            var k3 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0)));
            var k3_0 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k3_1 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0.55)));
            var k3_2 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)));

            dauOpacty.KeyFrames.Add(k3);
            dauOpacty.KeyFrames.Add(k3_0);
            dauOpacty.KeyFrames.Add(k3_1);
            dauOpacty.KeyFrames.Add(k3_2);
            dauOpacty.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTarget(dauOpacty, Element);
            Storyboard.SetTargetProperty(dauOpacty, new PropertyPath(UIElement.OpacityProperty));
            Story.Children.Add(dauOpacty);
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
            dauOpacty.KeyFrames.Clear();
            dauOpacty = null;
            Story = null;
        }
        private void Story_Completed(object sndr, EventArgs evtArgs)
        {
            try
            {
                Element.Visibility = Visibility.Collapsed;
                base.CallClientCompleted();
            }
            catch
            {
            }
        }
    }
}
