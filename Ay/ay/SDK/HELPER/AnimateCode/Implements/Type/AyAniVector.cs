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

    public class AyAniVector : AyAnimateTypeBase
    {
        public AyAniVector(FrameworkElement _element)
            : base("Vector", _element)
        { base.AnimateSpeed = 600; }

        public AyAniVector(FrameworkElement _element, Action _completed)
            : base("Vector", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Vector? toVector;
        public Vector? ToVector
        {
            get { return toVector; }
            set { toVector = value; }
        }
        private Vector? fromVector;
        public Vector? FromVector
        {
            get { return fromVector; }
            set { fromVector = value; }
        }

        public override void CreateStoryboard()
        {
            VectorAnimationUsingKeyFrames dau = new VectorAnimationUsingKeyFrames();

            EasingVectorKeyFrame fromk = null;
            if (FromVector.HasValue) {
                fromk=new EasingVectorKeyFrame(FromVector.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingVectorKeyFrame tok = null;
            if (ToVector.HasValue)
            {
                tok = new EasingVectorKeyFrame(ToVector.Value, TimeSpan.FromMilliseconds(AniTime(1)));
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
