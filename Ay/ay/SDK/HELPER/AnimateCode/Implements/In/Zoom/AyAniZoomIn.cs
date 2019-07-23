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
    public class AyAniZoomIn : AyAnimateBaseIn
    {

        #region 构造函数
        public AyAniZoomIn(FrameworkElement _element)
    : base("ZoomIn", _element)
        { base.AnimateSpeed = 450; }

        public AyAniZoomIn(FrameworkElement _element, Action _completed)
            : base("ZoomIn", _element, _completed)
        { base.AnimateSpeed = 450; }

        #endregion
        #region 属性
        private IEasingFunction easingFunction;

        public IEasingFunction EasingFunction
        {
            get { return easingFunction; }
            set { easingFunction = value; }
        }

        private bool opacityNeed = true;

        public bool OpacityNeed
        {
            get { return opacityNeed; }
            set { opacityNeed = value; }
        }

        #endregion
        #region KeyFrames
        DoubleAnimationUsingKeyFrames dauX = null;
        DoubleAnimationUsingKeyFrames dauY = null;
        DoubleAnimationUsingKeyFrames dauOpacty = null;
        #endregion

        protected override void Init()
        {
            SetBaseView();

            dauX = new DoubleAnimationUsingKeyFrames();
            dauY = new DoubleAnimationUsingKeyFrames();

            ScaleTransform translation = new ScaleTransform(1, 1);

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


            var k2 = new EasingDoubleKeyFrame(0.3, TimeSpan.FromMilliseconds(0));
            var k3 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));
            Storyboard.SetTargetName(dauX, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauX, new PropertyPath(ScaleTransform.ScaleXProperty));
            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k3);
            Story.Children.Add(dauX);

            var k4 = new EasingDoubleKeyFrame(0.3, TimeSpan.FromMilliseconds(0));
            var k5 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));
            if (EasingFunction != null)
            {
                k3.EasingFunction = EasingFunction;
                k5.EasingFunction = EasingFunction;
            }
            Storyboard.SetTargetName(dauY, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauY, new PropertyPath(ScaleTransform.ScaleYProperty));
            dauY.KeyFrames.Add(k4);
            dauY.KeyFrames.Add(k5);
            Story.Children.Add(dauY);

            if (OpacityNeed)
            {
                dauOpacty = new DoubleAnimationUsingKeyFrames();
                var k6 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(0)));
                var k6_1 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));

                dauOpacty.KeyFrames.Add(k6);
                dauOpacty.KeyFrames.Add(k6_1);
                Storyboard.SetTarget(dauOpacty, Element);
                dauOpacty.FillBehavior = FillBehavior.Stop;
                Storyboard.SetTargetProperty(dauOpacty, new PropertyPath(UIElement.OpacityProperty));
                Story.Children.Add(dauOpacty);
            }

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
                if (OpacityNeed)
                {
                    Element.Opacity = 1;
                }
                base.CallClientCompleted();
            }
            catch
            {
            }
        }
    }
}
