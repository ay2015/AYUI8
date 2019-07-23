/**----------------------------------------------- 
 * * ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权========= 
 * 
 * 作者：ay * 联系QQ：875556003 * 时间2019-06-14
 * -----------------------------------------*/
using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.Animate
{
    public class AyAniZoomInRight : AyAnimateBaseIn
    {


        #region 构造函数
        public AyAniZoomInRight(FrameworkElement _element)
          : base("ZoomInRight", _element)
        { base.AnimateSpeed = 800; }

        public AyAniZoomInRight(FrameworkElement _element, Action _completed)
            : base("ZoomInRight", _element, _completed)
        { base.AnimateSpeed = 800; }

        #endregion
        #region 属性

        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dauX = null;
        DoubleAnimationUsingKeyFrames dauY = null;
        DoubleAnimationUsingKeyFrames dauOpacty = null;
        DoubleAnimationUsingKeyFrames dauTranslateY = null;
        #endregion


        protected override void Init()
        {
            SetBaseView();


            ScaleTransform translation = new ScaleTransform(1, 1);
            TranslateTransform translationTranslate = new TranslateTransform(0, 0);
  
        
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

                Win.RegisterName(translationTranslate.GetHashCode().ToString(), translationTranslate);
                tg.Children.Add(translationTranslate);


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

                foreach (var item in tg.Children)
                {
                    translationTranslate = item as TranslateTransform;
                    if (translationTranslate != null)
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

                if (translationTranslate != null)
                {

                    var tex = translationTranslate.GetValue(FrameworkElement.NameProperty);
                    if (tex != null && tex.ToString() != "")
                    {
                
                    }
                    else
                    {

                        Win.RegisterName(translationTranslate.GetHashCode().ToString(), translationTranslate);
                    }
                }
                else
                {
                    translationTranslate = new TranslateTransform(0, 0);

                    Win.RegisterName(translationTranslate.GetHashCode().ToString(), translationTranslate);
                    tg.Children.Add(translationTranslate);
                    Element.RenderTransform = tg;
                }

            }
            #endregion
            Win.RegisterResource(Story);
            Story = (Storyboard)Story.CloneCurrentValue();
            double danqianX = translation.ScaleX;
            double danqianY = translation.ScaleY;

          
            var keyspline = new KeySpline(0.55, 0.055, 0.675, 0.19);
            var keyspline2 = new KeySpline(0.175, 0.885, 0.320, 1);
            var k3_0 = new SplineDoubleKeyFrame();
            k3_0.KeySpline = keyspline;
            k3_0.Value = 0.1;
            k3_0.KeyTime = TimeSpan.FromMilliseconds(0);

            var k3_1 = new SplineDoubleKeyFrame();
            k3_1.KeySpline = keyspline2;
            k3_1.Value = 0.475;
            k3_1.KeyTime = TimeSpan.FromMilliseconds(AniTime(0.6));

            var k3_2 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));


            Storyboard.SetTargetName(dauX, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauX, new PropertyPath(ScaleTransform.ScaleXProperty));
            dauX.KeyFrames.Add(k3_0);
            dauX.KeyFrames.Add(k3_1);
            dauX.KeyFrames.Add(k3_2);
            Story.Children.Add(dauX);


            var k4_0 = new SplineDoubleKeyFrame();
            k4_0.KeySpline = keyspline;
            k4_0.Value = 0.1;
            k4_0.KeyTime = TimeSpan.FromMilliseconds(0);

            var k4_1 = new SplineDoubleKeyFrame();
            k4_1.KeySpline = keyspline2;
            k4_1.Value = 0.475;
            k4_1.KeyTime = TimeSpan.FromMilliseconds(AniTime(0.6));

            var k4_2 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));
            Storyboard.SetTargetName(dauY, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauY, new PropertyPath(ScaleTransform.ScaleYProperty));
            dauY.KeyFrames.Add(k4_0);
            dauY.KeyFrames.Add(k4_1);
            dauY.KeyFrames.Add(k4_2);
            Story.Children.Add(dauY);




             dauTranslateY = new DoubleAnimationUsingKeyFrames();

            var k5_0 = new SplineDoubleKeyFrame();
            k5_0.KeySpline = keyspline;
            k5_0.Value =1000;
            k5_0.KeyTime = TimeSpan.FromMilliseconds(0);

            var k5_1 = new SplineDoubleKeyFrame();
            k5_1.KeySpline = keyspline2;
            k5_1.Value = -10;
            k5_1.KeyTime = TimeSpan.FromMilliseconds(AniTime(0.6));

            var k5_2 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)));


            Storyboard.SetTargetName(dauTranslateY, Win.GetName(translationTranslate));
            Storyboard.SetTargetProperty(dauTranslateY, new PropertyPath(TranslateTransform.XProperty));

            dauTranslateY.KeyFrames.Add(k5_0);
            dauTranslateY.KeyFrames.Add(k5_1);
            dauTranslateY.KeyFrames.Add(k5_2);
            Story.Children.Add(dauTranslateY);
    


            dauOpacty = new DoubleAnimationUsingKeyFrames();
            var k6 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(0)));
            var k6_1 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0.6)));

            dauOpacty.KeyFrames.Add(k6);
            dauOpacty.KeyFrames.Add(k6_1);
            Storyboard.SetTarget(dauOpacty, Element);
            dauOpacty.FillBehavior = FillBehavior.Stop;
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
                Element.Opacity = 1;
                base.CallClientCompleted();
            }
            catch
            {
            }
        }
    }
}
