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


    public class AyAniInt64 : AyAnimateTypeBase
    {
        public AyAniInt64(FrameworkElement _element)
            : base("Int64", _element)
        { base.AnimateSpeed = 600; }

        public AyAniInt64(FrameworkElement _element, Action _completed)
            : base("Int64", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Int64? toInt64;
        public Int64? ToInt64
        {
            get { return toInt64; }
            set { toInt64 = value; }
        }
        private Int64? fromInt64;
        public Int64? FromInt64
        {
            get { return fromInt64; }
            set { fromInt64 = value; }
        }

        public override void CreateStoryboard()
        {
            Int64AnimationUsingKeyFrames dau = new Int64AnimationUsingKeyFrames();

            EasingInt64KeyFrame fromk = null;
            if (FromInt64.HasValue) {
                fromk=new EasingInt64KeyFrame(FromInt64.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingInt64KeyFrame tok = null;
            if (ToInt64.HasValue)
            {
                tok = new EasingInt64KeyFrame(ToInt64.Value, TimeSpan.FromMilliseconds(AniTime(1)));
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
