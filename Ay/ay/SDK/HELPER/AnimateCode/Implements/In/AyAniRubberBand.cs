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
        public class AyAniRubberBand : AyAnimateBaseIn
        {
        #region 构造函数
        public AyAniRubberBand(FrameworkElement _element)
       : base("rubberBand", _element)
        { base.AnimateSpeed = 900;Initialize(); }

        public AyAniRubberBand(FrameworkElement _element, Action _completed)
            : base("rubberBand", _element, _completed)
        { base.AnimateSpeed = 900; }


        #endregion
        #region 属性
        private int oscillations = 3;
        /// <summary>
        /// 抖动次数
        /// </summary>
        public int Oscillations
        {
            get { return oscillations; }
            set { oscillations = value; }
        }


        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dauX = null;
        DoubleAnimationUsingKeyFrames dauY = null;

        #endregion


        protected override void Init()
        {
            SetBaseView();

            ScaleTransform translation = new ScaleTransform(1,1);

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
            Win.RegisterResource(Story);
            double danqianX = translation.ScaleX;
            double danqianY = translation.ScaleY;
            #endregion


            Story = (Storyboard)Story.Clone();
            //var k1 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(0));
            var k2 = new EasingDoubleKeyFrame(danqianX + 0.3, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k3 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(1)), new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = this.Oscillations });
            Storyboard.SetTargetName(dauX, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauX, new PropertyPath(ScaleTransform.ScaleXProperty));

            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k3);
            Story.Children.Add(dauX);

            var k0 = new EasingDoubleKeyFrame(danqianY - 0.2, TimeSpan.FromMilliseconds(0));
            var k4 = new EasingDoubleKeyFrame(danqianY - 0.2, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k5 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(AniTime(1)), new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = this.Oscillations });
            Storyboard.SetTargetName(dauY, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauY, new PropertyPath(ScaleTransform.ScaleYProperty));
            dauY.KeyFrames.Add(k0);
            dauY.KeyFrames.Add(k4);
            dauY.KeyFrames.Add(k5);
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
