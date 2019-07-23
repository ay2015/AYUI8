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
    public class AyAniSlideInRight : AyAnimateBaseIn
    {
        #region 构造函数
        public AyAniSlideInRight(FrameworkElement _element)
     : base("SlideInRight", _element)
        { base.AnimateSpeed = 300; }

        public AyAniSlideInRight(FrameworkElement _element, Action _completed)
            : base("SlideInRight", _element, _completed)
        {
            base.AnimateSpeed = 300;
        }
        #endregion
        #region 属性

        private double fromDistance = 0;

        public double FromDistance
        {
            get { return fromDistance; }
            set { fromDistance = value; }
        }
        private double toDistance = 0;

        public double ToDistance
        {
            get { return toDistance; }
            set { toDistance = value; }
        }

        private FillBehavior aniEndBehavior = FillBehavior.Stop;

        public FillBehavior AniEndBehavior
        {
            get { return aniEndBehavior; }
            set { aniEndBehavior = value; }
        }

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
        DoubleAnimationUsingKeyFrames dau = null;
        DoubleAnimationUsingKeyFrames dauOpacty = null;
        #endregion






        protected override void Init()
        {
            SetBaseView();

            TranslateTransform translation = new TranslateTransform(0, 0);

            dau = new DoubleAnimationUsingKeyFrames();
            #region 基本工作，确定类型和name
            //是否存在TranslateTransform
            //动画要的类型是否存在
            //动画要的类型的name是否存在，不存在就注册，结束后取消注册，删除动画
            var ex = Element.RenderTransform;
            if (ex == null || (ex as System.Windows.Media.MatrixTransform) != null)
            {
                var tg = new TransformGroup();
                translation = new TranslateTransform(0, 0);
                Win.RegisterName(translation.GetHashCode().ToString(), translation);
                tg.Children.Add(translation);
                Element.RenderTransform = tg;
            }
            else
            {
                var tg = ex as TransformGroup;
                foreach (var item in tg.Children)
                {
                    translation = item as TranslateTransform;
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
                    translation = new TranslateTransform(0, 0);
                    Win.RegisterName(translation.GetHashCode().ToString(), translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            if (FromDistance == 0)
            {
                FromDistance = Element.RenderSize.Width * 1.5;
            }


            var k2 = new EasingDoubleKeyFrame(FromDistance, TimeSpan.FromMilliseconds(AniTime(0)));
            var k3 = new EasingDoubleKeyFrame(ToDistance, TimeSpan.FromMilliseconds(AniTime(1)));
            if (EasingFunction != null)
            {
                k3.EasingFunction = EasingFunction;
            }

            Storyboard.SetTargetName(dau, Win.GetName(translation));

            Storyboard.SetTargetProperty(dau, new PropertyPath(TranslateTransform.XProperty));
            dau.FillBehavior = AniEndBehavior;

            Win.RegisterResource(Story);
            Story = (Storyboard)Story.CloneCurrentValue();
            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k3);
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
