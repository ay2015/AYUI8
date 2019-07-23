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

    public class AyAniWobble : AyAnimateBaseIn
    {
        #region 构造函数
        public AyAniWobble(FrameworkElement _element)
            : base("wobble", _element)
        {
            base.AnimateSpeed = 1200;Initialize();
        }

        public AyAniWobble(FrameworkElement _element, Action _completed)
            : base("wobble", _element, _completed)
        {
            base.AnimateSpeed = 1200;

        }

        #endregion
        #region KeyFrames
        DoubleAnimationUsingKeyFrames dau = null;

        DoubleAnimationUsingKeyFrames dauTranslateX = null;
        #endregion



        protected override void Init()
        {
            SetBaseView();


            RotateTransform translation = new RotateTransform();
            TranslateTransform translationTranslate = new TranslateTransform(0, 0);

             dau = new DoubleAnimationUsingKeyFrames();
            #region 基本工作，确定类型和name
            //是否存在TranslateTransform
            //动画要的类型是否存在
            //动画要的类型的name是否存在，不存在就注册，结束后取消注册，删除动画
            var ex = Element.RenderTransform;
            if (ex == null || (ex as System.Windows.Media.MatrixTransform) != null)
            {
                var tg = new TransformGroup();
                translation = new RotateTransform();
                Win.RegisterName(translation);
                tg.Children.Add(translation);
                Win.RegisterName( translationTranslate);
                tg.Children.Add(translationTranslate);

                Element.RenderTransform = tg;
            }
            else
            {
                var tg = ex as TransformGroup;
                foreach (var item in tg.Children)
                {
                    translation = item as RotateTransform;
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
                     
                        Win.RegisterName(translation);
                    }
                }
                else
                {
                    translation = new RotateTransform();
                    Win.RegisterName(translation);
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
                        
                        Win.RegisterName(translationTranslate);
                    }
                }
                else
                {
                    translationTranslate = new TranslateTransform(0, 0);
                    Win.RegisterName(translationTranslate);
                    tg.Children.Add(translationTranslate);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            translation.CenterX = 0.5;
            translation.CenterY = 1;
            Story = (Storyboard)Story.Clone();
            double angle = translation.Angle;
            var k2 = new EasingDoubleKeyFrame(angle - 5, TimeSpan.FromMilliseconds(AniTime(0.15)));
            var k2_1 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k2_2 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.45)));
            var k2_3 = new EasingDoubleKeyFrame(angle + 2, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_4 = new EasingDoubleKeyFrame(angle - 1, TimeSpan.FromMilliseconds(AniTime(0.75)));
            var k2_9 = new EasingDoubleKeyFrame(angle, TimeSpan.FromMilliseconds(AnimateSpeed));

            Storyboard.SetTargetName(dau, Win.GetName(translation));
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));

            Win.RegisterResource(Story);

            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k2_1);
            dau.KeyFrames.Add(k2_2);
            dau.KeyFrames.Add(k2_3);
            dau.KeyFrames.Add(k2_4);
            dau.KeyFrames.Add(k2_9);

            Story.Children.Add(dau);



            dauTranslateX = new DoubleAnimationUsingKeyFrames();
            double lateX = translationTranslate.X;
            double elementWidth = Element.RenderSize.Width;
            double s1 = lateX + (-0.25 * elementWidth);
            double s2 = lateX + (0.2 * elementWidth);

            double s3 = lateX + (-0.15 * elementWidth);
            double s4 = lateX + (0.1 * elementWidth);
            double s5 = lateX + (-0.05 * elementWidth);

            var k3 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.15)));
            var k3_1 = new EasingDoubleKeyFrame(s2, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k3_2 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.45)));
            var k3_3 = new EasingDoubleKeyFrame(s4, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k3_4 = new EasingDoubleKeyFrame(s5, TimeSpan.FromMilliseconds(AniTime(0.75)));
            var k3_9 = new EasingDoubleKeyFrame(lateX, TimeSpan.FromMilliseconds(AnimateSpeed));

            Storyboard.SetTargetName(dauTranslateX, Win.GetName(translationTranslate));
            Storyboard.SetTargetProperty(dauTranslateX, new PropertyPath(TranslateTransform.XProperty));

            dauTranslateX.KeyFrames.Add(k3);
            dauTranslateX.KeyFrames.Add(k3_1);
            dauTranslateX.KeyFrames.Add(k3_2);
            dauTranslateX.KeyFrames.Add(k3_3);
            dauTranslateX.KeyFrames.Add(k3_4);
            dauTranslateX.KeyFrames.Add(k3_9);

           Story.Children.Add(dauTranslateX);

            Story.Completed -= Story_Completed;
            Story.Completed += Story_Completed;
        }

        public override void Destroy()
        {
            Story.Completed -= Story_Completed;
            Win.UnRegisterNameAll();
            Win.UnRegisterResource(Story);
            dau.KeyFrames.Clear();
            dau = null;
            dauTranslateX.KeyFrames.Clear();
            dauTranslateX = null;


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
