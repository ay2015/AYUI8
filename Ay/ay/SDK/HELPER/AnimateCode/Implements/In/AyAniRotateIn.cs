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
    public class AyAniRotateIn : AyAnimateBaseIn
    {

        #region 构造函数
        public AyAniRotateIn(FrameworkElement _element)
          : base("swing", _element)
        { base.AnimateSpeed = 350; Initialize(); }

        public AyAniRotateIn(FrameworkElement _element, Action _completed)
            : base("swing", _element, _completed)
        {
            base.AnimateSpeed = 350;
        }
        #endregion
        #region 属性

        private double rotateAngle = -200;

        public double RotateAngle
        {
            get { return rotateAngle; }
            set { rotateAngle = value; }
        }

        private bool opacityNeed = true;

        public bool OpacityNeed
        {
            get { return opacityNeed; }
            set { opacityNeed = value; }
        }

        private IEasingFunction easingFunction;

        public IEasingFunction EasingFunction
        {
            get { return easingFunction; }
            set { easingFunction = value; }
        }

        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dau = null;
        DoubleAnimationUsingKeyFrames dauOpacty = null;
        #endregion
        protected override void Init()
        {
            SetBaseView();

            RotateTransform translation = new RotateTransform();

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
                if (translation != null)
                {
                    //当前Y值
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
            }
            #endregion

            double angle = translation.Angle;
            var k2_0 = new EasingDoubleKeyFrame(RotateAngle, TimeSpan.FromMilliseconds(AniTime(0)));
            var k2 = new EasingDoubleKeyFrame(angle, TimeSpan.FromMilliseconds(AniTime(1)));

            if (EasingFunction != null)
            {
                if (k2_0 != null) k2_0.EasingFunction = EasingFunction;
                if (k2 != null) k2.EasingFunction = EasingFunction;
            }


            Story = (Storyboard)Story.Clone();
            Storyboard.SetTargetName(dau, Win.GetName(translation));
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));

            Win.RegisterResource(Story);

            dau.KeyFrames.Add(k2_0);
            dau.KeyFrames.Add(k2);
            Story.Children.Add(dau);

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
