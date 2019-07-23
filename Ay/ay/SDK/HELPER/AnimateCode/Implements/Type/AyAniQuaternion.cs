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
using System.Windows.Media.Media3D;

namespace ay.Animate
{


    public class AyAniQuaternion : AyAnimateTypeBase
    {
        public AyAniQuaternion(FrameworkElement _element)
            : base("Quaternion", _element)
        { base.AnimateSpeed = 600; }

        public AyAniQuaternion(FrameworkElement _element, Action _completed)
            : base("Quaternion", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Quaternion? toQuaternion;
        public Quaternion? ToQuaternion
        {
            get { return toQuaternion; }
            set { toQuaternion = value; }
        }
        private Quaternion? fromQuaternion;
        public Quaternion? FromQuaternion
        {
            get { return fromQuaternion; }
            set { fromQuaternion = value; }
        }

        public override void CreateStoryboard()
        {
            QuaternionAnimationUsingKeyFrames dau = new QuaternionAnimationUsingKeyFrames();

            EasingQuaternionKeyFrame fromk = null;
            if (FromQuaternion.HasValue) {
                fromk=new EasingQuaternionKeyFrame(FromQuaternion.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingQuaternionKeyFrame tok = null;
            if (ToQuaternion.HasValue)
            {
                tok = new EasingQuaternionKeyFrame(ToQuaternion.Value, TimeSpan.FromMilliseconds(AniTime(1)));
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
