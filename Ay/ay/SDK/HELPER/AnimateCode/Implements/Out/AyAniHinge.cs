/**----------------------------------------------- 
 * * ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权========= 
 * 
 * 作者：ay * 联系QQ：875556003
 * 时间2019-06-14
 * -----------------------------------------*/
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.Animate
{

    /// <summary>
    /// 支持animatespeed
    /// </summary>
    public class AyAniHinge : AyAnimateBaseOut
    {

        #region 构造函数
        public AyAniHinge(FrameworkElement _element)
    : base("hinge", _element)
        {
            base.AnimateSpeed = 2000;
        }

        public AyAniHinge(FrameworkElement _element, Action _completed)
            : base("hinge", _element, _completed)
        {
            base.AnimateSpeed = 2000;

        }


        #endregion
        #region 属性

        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dau = null;
        DoubleAnimationUsingKeyFrames dauOpacty = null;
        DoubleAnimationUsingKeyFrames dauTranslateY = null;
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
                        Win.RegisterName(translation.GetHashCode().ToString(), translation);
                    }
                }
                else
                {
                    translation = new RotateTransform();
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

            Story = (Storyboard)Story.CloneCurrentValue();
            var k2 = new EasingDoubleKeyFrame(80, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k2_2 = new EasingDoubleKeyFrame(40, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k2_1 = new EasingDoubleKeyFrame(75, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_3 = new EasingDoubleKeyFrame(60, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k2_4 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)), new CircleEase() { EasingMode = EasingMode.EaseOut });

            Storyboard.SetTargetName(dau, Win.GetName(translation));

            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));
            dau.FillBehavior = FillBehavior.Stop;
            Win.RegisterResource(Story);
            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k2_1);
            dau.KeyFrames.Add(k2_2);
            dau.KeyFrames.Add(k2_3);
            dau.KeyFrames.Add(k2_4);


            Story.Children.Add(dau);



            dauTranslateY = new DoubleAnimationUsingKeyFrames();
            var k3 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k3_1 = new EasingDoubleKeyFrame(700, TimeSpan.FromMilliseconds(AniTime(1)));

            Storyboard.SetTargetName(dauTranslateY, Win.GetName(translationTranslate));
            Storyboard.SetTargetProperty(dauTranslateY, new PropertyPath(TranslateTransform.YProperty));

            dauTranslateY.KeyFrames.Add(k3);
            dauTranslateY.KeyFrames.Add(k3_1);
            Story.Children.Add(dauTranslateY);
            dauTranslateY.FillBehavior = FillBehavior.Stop;


            dauOpacty = new DoubleAnimationUsingKeyFrames();
            var k4 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k4_0 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)));

            dauOpacty.KeyFrames.Add(k4);
            dauOpacty.KeyFrames.Add(k4_0);

            Storyboard.SetTarget(dauOpacty, Element);
            dauOpacty.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTargetProperty(dauOpacty, new PropertyPath(UIElement.OpacityProperty));
            Story.Children.Add(dauOpacty);


            //PointAnimationUsingKeyFrames porender = new PointAnimationUsingKeyFrames();
            //var k5 = new EasingPointKeyFrame(new Point(0, 0), TimeSpan.FromMilliseconds(0));
            //var k5_0 = new EasingPointKeyFrame(new Point(0, 0), TimeSpan.FromMilliseconds(AniTime(0.8)));
            ////var k5_1 = new EasingPointKeyFrame(new Point(0.4, 0.9), TimeSpan.FromMilliseconds(AniTime(0.8001)));
            //porender.KeyFrames.Add(k5);
            //porender.KeyFrames.Add(k5_0);
            ////porender.KeyFrames.Add(k5_1);

            //Storyboard.SetTarget(porender, Element);
            //dauOpacty.FillBehavior = FillBehavior.Stop;
            //Storyboard.SetTargetProperty(porender, new PropertyPath(UIElement.RenderTransformOriginProperty));
            //story.Children.Add(porender);


            Story.Completed += Story_Completed;
        }

        public override void Destroy()
        {
            Story.Completed -= Story_Completed;
            Win.UnRegisterNameAll();
            Win.UnRegisterResource(Story);
            dau.KeyFrames.Clear();
            dau = null;
            dauTranslateY.KeyFrames.Clear();
            dauTranslateY = null;
            dauOpacty.KeyFrames.Clear();
            dauOpacty = null;
            Story = null;
        }
        private void Story_Completed(object sndr, EventArgs evtArgs)
        {
            try
            {
                Element.Visibility = Visibility.Collapsed;
                Element.Opacity = 0;

                base.CallClientCompleted();
            }
            catch
            {
            }
        }
    }
}
