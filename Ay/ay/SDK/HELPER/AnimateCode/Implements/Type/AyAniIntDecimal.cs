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

    public class AyAniDecimal : AyAnimateTypeBase
    {
        public AyAniDecimal(FrameworkElement _element)
            : base("Decimal", _element)
        { base.AnimateSpeed = 600; }

        public AyAniDecimal(FrameworkElement _element, Action _completed)
            : base("Decimal", _element, _completed)
        { base.AnimateSpeed = 600; }

        private decimal? toDecimal;
        public decimal? ToDecimal
        {
            get { return toDecimal; }
            set { toDecimal = value; }
        }
        private decimal? fromDecimal;
        public decimal? FromDecimal
        {
            get { return fromDecimal; }
            set { fromDecimal = value; }
        }

        public override void CreateStoryboard()
        {
            DecimalAnimationUsingKeyFrames dau = new DecimalAnimationUsingKeyFrames();

            EasingDecimalKeyFrame fromk = null;
            if (FromDecimal.HasValue) {
                fromk=new EasingDecimalKeyFrame(FromDecimal.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingDecimalKeyFrame tok = null;
            if (ToDecimal.HasValue)
            {
                tok = new EasingDecimalKeyFrame(ToDecimal.Value, TimeSpan.FromMilliseconds(AniTime(1)));
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
