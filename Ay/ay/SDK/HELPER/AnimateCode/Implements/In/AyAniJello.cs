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
    public class AyAniJello : AyAnimateBaseIn
    {

        #region 构造函数
        public AyAniJello(FrameworkElement _element)
         : base("jello", _element)
        { base.AnimateSpeed = 900;Initialize(); }

        public AyAniJello(FrameworkElement _element, Action _completed)
            : base("jello", _element, _completed)
        { base.AnimateSpeed = 900; }

        #endregion
        #region 属性

        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dauX = null;
        DoubleAnimationUsingKeyFrames dauY = null;
        #endregion

        protected override void Init()
        {
            SetBaseView();
            SkewTransform translation = new SkewTransform(0, 0);

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
                translation = new SkewTransform(1, 1);
                Win.RegisterName(translation.GetHashCode().ToString(), translation);
                tg.Children.Add(translation);
                Element.RenderTransform = tg;
            }
            else
            {
                var tg = ex as TransformGroup;
                foreach (var item in tg.Children)
                {
                    translation = item as SkewTransform;
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
                    translation = new SkewTransform(0,0);
                    Win.RegisterName(translation.GetHashCode().ToString(), translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            Win.RegisterResource(Story);
            double danqianX = translation.AngleX;
            double danqianY = translation.AngleY;


            var k2 = new EasingDoubleKeyFrame(danqianX -12.5, TimeSpan.FromMilliseconds(AniTime(0.222)));
            var k2_0 = new EasingDoubleKeyFrame(danqianX +6.25, TimeSpan.FromMilliseconds(AniTime(0.333)));
            var k2_1 = new EasingDoubleKeyFrame(danqianX - 3.125, TimeSpan.FromMilliseconds(AniTime(0.444)));
            var k2_2 = new EasingDoubleKeyFrame(danqianX + 1.5625, TimeSpan.FromMilliseconds(AniTime(0.555)));
            var k2_3 = new EasingDoubleKeyFrame(danqianX -0.78125, TimeSpan.FromMilliseconds(AniTime(0.666)));
            var k2_4 = new EasingDoubleKeyFrame(danqianX + 0.390625, TimeSpan.FromMilliseconds(AniTime(0.777)));
            var k2_5 = new EasingDoubleKeyFrame(danqianX - 0.1953125, TimeSpan.FromMilliseconds(AniTime(0.888)));
            var k2_6 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(1)));

            var k3 = new EasingDoubleKeyFrame(danqianY - 12.5, TimeSpan.FromMilliseconds(AniTime(0.222)));
            var k3_0 = new EasingDoubleKeyFrame(danqianY + 6.25, TimeSpan.FromMilliseconds(AniTime(0.333)));
            var k3_1 = new EasingDoubleKeyFrame(danqianY - 3.125, TimeSpan.FromMilliseconds(AniTime(0.444)));
            var k3_2 = new EasingDoubleKeyFrame(danqianY + 1.5625, TimeSpan.FromMilliseconds(AniTime(0.555)));
            var k3_3 = new EasingDoubleKeyFrame(danqianY - 0.78125, TimeSpan.FromMilliseconds(AniTime(0.666)));
            var k3_4 = new EasingDoubleKeyFrame(danqianY + 0.390625, TimeSpan.FromMilliseconds(AniTime(0.777)));
            var k3_5 = new EasingDoubleKeyFrame(danqianY - 0.1953125, TimeSpan.FromMilliseconds(AniTime(0.888)));
            var k3_6 = new EasingDoubleKeyFrame(danqianY , TimeSpan.FromMilliseconds(AniTime(1)));





            Storyboard.SetTargetName(dauX, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauX, new PropertyPath(SkewTransform.AngleXProperty));
            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k2_0);
            dauX.KeyFrames.Add(k2_1);
            dauX.KeyFrames.Add(k2_2);
            dauX.KeyFrames.Add(k2_3);
            dauX.KeyFrames.Add(k2_4);
            dauX.KeyFrames.Add(k2_5);
            dauX.KeyFrames.Add(k2_6);
            Story = (Storyboard)Story.CloneCurrentValue();
            Story.Children.Add(dauX);


            Storyboard.SetTargetName(dauY, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauY, new PropertyPath(SkewTransform.AngleYProperty));
            dauY.KeyFrames.Add(k3);
            dauY.KeyFrames.Add(k3_0);
            dauY.KeyFrames.Add(k3_1);
            dauY.KeyFrames.Add(k3_2);
            dauY.KeyFrames.Add(k3_3);
            dauY.KeyFrames.Add(k3_4);
            dauY.KeyFrames.Add(k3_5);
            dauY.KeyFrames.Add(k3_6);
            Story.Children.Add(dauY);


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
