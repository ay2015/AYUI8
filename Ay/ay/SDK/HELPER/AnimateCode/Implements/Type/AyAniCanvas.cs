/**----------------------------------------------- 
 * * ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权========= 
 * 
 * 作者：ay * 联系QQ：875556003
 * 时间2019-06-14
 * -----------------------------------------*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.Animate
{
    public class AyAniCanvas : AyAnimateTypeBase
    {
        #region 构造函数
        public AyAniCanvas(FrameworkElement _element)
          : base("Canvas", _element)
        { base.AnimateSpeed = 600; }

        public AyAniCanvas(FrameworkElement _element, Action _completed)
            : base("Canvas", _element, _completed)
        { base.AnimateSpeed = 600; }
        #endregion
        #region 属性


        private double? toCanvasLeft;

        public double? ToCanvasLeft
        {
            get { return toCanvasLeft; }
            set { toCanvasLeft = value; }
        }

        private double? toCanvasTop;

        public double? ToCanvasTop
        {
            get { return toCanvasTop; }
            set { toCanvasTop = value; }
        }

        private double? toCanvasRight;

        public double? ToCanvasRight
        {
            get { return toCanvasRight; }
            set { toCanvasRight = value; }
        }

        private double? toCanvasBottom;

        public double? ToCanvasBottom
        {
            get { return toCanvasBottom; }
            set { toCanvasBottom = value; }
        }
        #endregion





        public override void CreateStoryboard()
        {
            DoubleAnimationUsingKeyFrames dauLeft = null;
            DoubleAnimationUsingKeyFrames dauRight = null;
            DoubleAnimationUsingKeyFrames dauTop = null;
            DoubleAnimationUsingKeyFrames dauBottom = null;
            EasingDoubleKeyFrame leftk = null;
            if (ToCanvasLeft.HasValue)
            {
                dauLeft = new DoubleAnimationUsingKeyFrames();
                leftk = new EasingDoubleKeyFrame(ToCanvasLeft.Value, TimeSpan.FromMilliseconds(AniTime(1)));

                if (AniEasingFunction != null)
                {
                    leftk.EasingFunction = AniEasingFunction;
                }
                else if (CirDefault != null)
                {
                    leftk.EasingFunction = CirDefault;
                }


                dauLeft.KeyFrames.Add(leftk);
            }

            EasingDoubleKeyFrame rightk = null;
            if (ToCanvasRight.HasValue)
            {
                dauRight = new DoubleAnimationUsingKeyFrames();
                rightk = new EasingDoubleKeyFrame(ToCanvasRight.Value, TimeSpan.FromMilliseconds(AniTime(1)));
                if (AniEasingFunction != null)
                {
                    rightk.EasingFunction = AniEasingFunction;
                }
                else if (CirDefault != null)
                {
                    rightk.EasingFunction = CirDefault;
                }
                dauRight.KeyFrames.Add(rightk);
            }


            EasingDoubleKeyFrame topk = null;
            if (ToCanvasTop.HasValue)
            {
                dauTop = new DoubleAnimationUsingKeyFrames();
                topk = new EasingDoubleKeyFrame(ToCanvasTop.Value, TimeSpan.FromMilliseconds(AniTime(1)));
                if (AniEasingFunction != null)
                {
                    topk.EasingFunction = AniEasingFunction;
                }
                else if (CirDefault != null)
                {
                    topk.EasingFunction = CirDefault;
                }
                dauTop.KeyFrames.Add(topk);
            }

            EasingDoubleKeyFrame bottomk = null;
            if (ToCanvasBottom.HasValue)
            {
                dauBottom = new DoubleAnimationUsingKeyFrames();
                bottomk = new EasingDoubleKeyFrame(ToCanvasBottom.Value, TimeSpan.FromMilliseconds(AniTime(1)));
                if (AniEasingFunction != null)
                {
                    bottomk.EasingFunction = AniEasingFunction;
                }
                else if (CirDefault != null)
                {
                    bottomk.EasingFunction = CirDefault;
                }
                dauBottom.KeyFrames.Add(bottomk);
            }
         

            if (leftk != null)
            {

                Storyboard.SetTarget(dauLeft, Element);
                Storyboard.SetTargetProperty(dauLeft, new PropertyPath(Canvas.LeftProperty));
                Story.Children.Add(dauLeft);
            }
            if (rightk != null)
            {
                Storyboard.SetTarget(dauRight, Element);

                Storyboard.SetTargetProperty(dauRight, new PropertyPath(Canvas.RightProperty));
                Story.Children.Add(dauRight);
            }
            if (topk != null)
            {
                Storyboard.SetTarget(dauTop, Element);

                Storyboard.SetTargetProperty(dauTop, new PropertyPath(Canvas.TopProperty));
                Story.Children.Add(dauTop);
            }
            if (bottomk != null)
            {
                Storyboard.SetTarget(dauBottom, Element);

                Storyboard.SetTargetProperty(dauBottom, new PropertyPath(Canvas.BottomProperty));
                Story.Children.Add(dauBottom);
            }
        }
    }
}
