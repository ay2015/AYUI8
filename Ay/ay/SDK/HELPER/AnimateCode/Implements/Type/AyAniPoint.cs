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


    public class AyAniPoint : AyAnimateTypeBase
    {
        public AyAniPoint(FrameworkElement _element)
            : base("Point", _element)
        { base.AnimateSpeed = 600; }

        public AyAniPoint(FrameworkElement _element, Action _completed)
            : base("Point", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Point? toPoint;
        public Point? ToPoint
        {
            get { return toPoint; }
            set { toPoint = value; }
        }
        private Point? fromPoint;
        public Point? FromPoint
        {
            get { return fromPoint; }
            set { fromPoint = value; }
        }

        public override void CreateStoryboard()
        {
            PointAnimationUsingKeyFrames dau = new PointAnimationUsingKeyFrames();

            EasingPointKeyFrame fromk = null;
            if (FromPoint.HasValue) {
                fromk=new EasingPointKeyFrame(FromPoint.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingPointKeyFrame tok = null;
            if (ToPoint.HasValue)
            {
                tok = new EasingPointKeyFrame(ToPoint.Value, TimeSpan.FromMilliseconds(AniTime(1)));
                dau.KeyFrames.Add(tok);
            }


            if (AniEasingFunction != null)
            {
                if (fromk!=null) fromk.EasingFunction = AniEasingFunction;
                if (tok!=null) tok.EasingFunction = AniEasingFunction;
            }
            else if (CirDefault != null)
            {
                if (fromk != null) fromk.EasingFunction = CirDefault;
                if (tok != null) tok.EasingFunction = CirDefault;
            }

            Storyboard.SetTarget(dau, Element);
            Storyboard.SetTargetProperty(dau, AniPropertyPath);
            Story.Children.Add(dau);
        }
    }
}
