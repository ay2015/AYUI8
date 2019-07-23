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

    public class AyAniTada : AyAnimateBaseIn
    {
       

        #region 构造函数
        public AyAniTada(FrameworkElement _element)
             : base("tada", _element)
        {
            base.AnimateSpeed = 1100;Initialize();
        }

        public AyAniTada(FrameworkElement _element, Action _completed)
            : base("tada", _element, _completed)
        {
            base.AnimateSpeed = 1100;
        }

        #endregion
        #region 属性

        private double tadaScale = 0.1;

        public double TadaScale
        {
            get { return tadaScale; }
            set { tadaScale = value; }
        }
        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dau = null;
        DoubleAnimationUsingKeyFrames dauScaleX = null;
        DoubleAnimationUsingKeyFrames dauScaleY = null;
        #endregion



        protected override void Init()
        {
            SetBaseView();

            RotateTransform translation = new RotateTransform();
            ScaleTransform translationScale = new ScaleTransform(1, 1);

           
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
                Win.RegisterName(translationScale);
                tg.Children.Add(translationScale);

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
                    translationScale = item as ScaleTransform;
                    if (translationScale != null)
                    {
                        break;
                    }
                }

                if (translation != null)
                {
                    //当前Y值
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

                if (translationScale != null)
                {

                    var tex = translationScale.GetValue(FrameworkElement.NameProperty);
                    if (tex != null && tex.ToString() != "")
                    {
        
                    }
                    else
                    {
                        Win.RegisterName(translationScale);
                    }
                }
                else
                {
                    translationScale = new ScaleTransform(1, 1);
                    Win.RegisterName(translationScale);
                    tg.Children.Add(translationScale);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            translation.CenterX = 0.5;
            translation.CenterY = 0;
            double angle = translation.Angle;
            var k2 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.1)));
            var k2_1 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k2_2 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k2_3 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k2_4 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.7)));
            var k2_5 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.9)));
            var k2_6 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k2_7 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_8 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k2_9 = new EasingDoubleKeyFrame(angle, TimeSpan.FromMilliseconds(AnimateSpeed));
            Story = (Storyboard)Story.Clone();
            Storyboard.SetTargetName(dau, Win.GetName(translation));
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));

            Win.RegisterResource(Story);

            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k2_1);
            dau.KeyFrames.Add(k2_2);
            dau.KeyFrames.Add(k2_3);
            dau.KeyFrames.Add(k2_4);
            dau.KeyFrames.Add(k2_5);
            dau.KeyFrames.Add(k2_6);
            dau.KeyFrames.Add(k2_7);
            dau.KeyFrames.Add(k2_8);
            dau.KeyFrames.Add(k2_9);

           Story.Children.Add(dau);


            dauScaleX = new DoubleAnimationUsingKeyFrames();
            dauScaleY = new DoubleAnimationUsingKeyFrames();
            double scaleX = translationScale.ScaleX;
            double scaleY = translationScale.ScaleY;
            double s1 = scaleX + tadaScale;
            double s2 = scaleX - tadaScale;

            double s3 = scaleY + tadaScale;
            double s4 = scaleY - tadaScale;

            var k3 = new EasingDoubleKeyFrame(s2, TimeSpan.FromMilliseconds(AniTime(0.1)));
            var k3_1 = new EasingDoubleKeyFrame(s2, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k3_2 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k3_3 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k3_4 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.7)));
            var k3_5 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.9)));
            var k3_6 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k3_7 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k3_8 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k3_9 = new EasingDoubleKeyFrame(scaleX, TimeSpan.FromMilliseconds(AnimateSpeed));


            var k4 = new EasingDoubleKeyFrame(s4, TimeSpan.FromMilliseconds(AniTime(0.1)));
            var k4_1 = new EasingDoubleKeyFrame(s4, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k4_2 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k4_3 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k4_4 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.7)));
            var k4_5 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.9)));
            var k4_6 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k4_7 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k4_8 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k4_9 = new EasingDoubleKeyFrame(scaleY, TimeSpan.FromMilliseconds(AnimateSpeed));



            Storyboard.SetTargetName(dauScaleX, Win.GetName(translationScale));
            Storyboard.SetTargetProperty(dauScaleX, new PropertyPath(ScaleTransform.ScaleXProperty));
            Storyboard.SetTargetName(dauScaleY, Win.GetName(translationScale));
            Storyboard.SetTargetProperty(dauScaleY, new PropertyPath(ScaleTransform.ScaleYProperty));

            dauScaleX.KeyFrames.Add(k3);
            dauScaleX.KeyFrames.Add(k3_1);
            dauScaleX.KeyFrames.Add(k3_2);
            dauScaleX.KeyFrames.Add(k3_3);
            dauScaleX.KeyFrames.Add(k3_4);
            dauScaleX.KeyFrames.Add(k3_5);
            dauScaleX.KeyFrames.Add(k3_6);
            dauScaleX.KeyFrames.Add(k3_7);
            dauScaleX.KeyFrames.Add(k3_8);
            dauScaleX.KeyFrames.Add(k3_9);

            Story.Children.Add(dauScaleX);


            dauScaleY.KeyFrames.Add(k4);
            dauScaleY.KeyFrames.Add(k4_1);
            dauScaleY.KeyFrames.Add(k4_2);
            dauScaleY.KeyFrames.Add(k4_3);
            dauScaleY.KeyFrames.Add(k4_4);
            dauScaleY.KeyFrames.Add(k4_5);
            dauScaleY.KeyFrames.Add(k4_6);
            dauScaleY.KeyFrames.Add(k4_7);
            dauScaleY.KeyFrames.Add(k4_8);
            dauScaleY.KeyFrames.Add(k4_9);

           Story.Children.Add(dauScaleY);

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
            dauScaleX.KeyFrames.Clear();
            dauScaleX = null;
            dauScaleY.KeyFrames.Clear();
            dauScaleY = null;

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
