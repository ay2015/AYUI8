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
    public class AyAniRotate : AyAnimateBaseIn
    {
        #region 构造函数
        public AyAniRotate(FrameworkElement _element)
            : base("Rotate", _element)
        { base.AnimateSpeed = 350; }

        public AyAniRotate(FrameworkElement _element, Action _completed)
            : base("Rotate", _element, _completed)
        {
            base.AnimateSpeed = 350;
        }
        #endregion

        #region 属性
        private double? rotateAngleAdd;

        public double? RotateAngleAdd
        {
            get { return rotateAngleAdd; }
            set { rotateAngleAdd = value; }
        }


        private double? rotateAngleTo;

        public double? RotateAngleTo
        {
            get { return rotateAngleTo; }
            set { rotateAngleTo = value; }
        }



        private RepeatBehavior? aniRepeatBehavior;

        public RepeatBehavior? AniRepeatBehavior
        {
            get { return aniRepeatBehavior; }
            set { aniRepeatBehavior = value; }
        }

        private bool? aniAutoReverse;

        public bool? AniAutoReverse
        {
            get { return aniAutoReverse; }
            set { aniAutoReverse = value; }
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
                Win.RegisterName(translation);
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
            }
            #endregion
            double angle = translation.Angle;
            //如果旋转角度和最终的是一致的就不执行下面代码
            if (RotateAngleTo.HasValue && RotateAngleTo == angle)
            {
                return;
            }
            EasingDoubleKeyFrame k2 = null;
            if (RotateAngleAdd.HasValue)
            {
                k2 = new EasingDoubleKeyFrame(RotateAngleAdd.Value + angle, TimeSpan.FromMilliseconds(AniTime(1)));
            }
            else if (RotateAngleTo.HasValue)
            {
                k2 = new EasingDoubleKeyFrame(RotateAngleTo.Value, TimeSpan.FromMilliseconds(AniTime(1)));
            }

            if (!RotateAngleAdd.HasValue && !RotateAngleTo.HasValue)
            {
                k2 = new EasingDoubleKeyFrame(RotateAngleAdd.Value + 360, TimeSpan.FromMilliseconds(AniTime(1)));
            }
            if (EasingFunction != null)
            {
                k2.EasingFunction = EasingFunction;
            }
            Story = (Storyboard)Story.CloneCurrentValue();
            Win.RegisterResource(Story);
            Storyboard.SetTargetName(dau, Win.GetName(translation));
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));

            if (AniRepeatBehavior.HasValue)
            {
                Story.RepeatBehavior = AniRepeatBehavior.Value;
            }

            if (AniAutoReverse.HasValue)
            {
                Story.AutoReverse = AniAutoReverse.Value;
            }


        
            Story = (Storyboard)Story.CloneCurrentValue();
            dau.KeyFrames.Add(k2);

            Story.Children.Add(dau);

            Story.Completed += Story_Completed;
        }

        public override void Destroy()
        {
            if (Story != null) {
                Story.Completed -= Story_Completed;
                Win.UnRegisterResource(Story);
            }

            Win.UnRegisterNameAll();
 
            dau.KeyFrames.Clear();
            dau = null;

            Story = null;
        }
        private void Story_Completed(object sndr, EventArgs evtArgs)
        {
            try
            {
                Element.Visibility = Visibility.Visible;
                Element.Opacity = 1;

                base.CallClientCompleted();
            }
            catch
            {
            }
        }

    }
}
