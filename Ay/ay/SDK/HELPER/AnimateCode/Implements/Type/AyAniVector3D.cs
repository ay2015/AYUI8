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


    public class AyAniVector3D : AyAnimateTypeBase
    {
        public AyAniVector3D(FrameworkElement _element)
            : base("Vector3D", _element)
        { base.AnimateSpeed = 600; }

        public AyAniVector3D(FrameworkElement _element, Action _completed)
            : base("Vector3D", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Vector3D? toVector3D;
        public Vector3D? ToVector3D
        {
            get { return toVector3D; }
            set { toVector3D = value; }
        }
        private Vector3D? fromVector3D;
        public Vector3D? FromVector3D
        {
            get { return fromVector3D; }
            set { fromVector3D = value; }
        }

        public override void CreateStoryboard()
        {
            Vector3DAnimationUsingKeyFrames dau = new Vector3DAnimationUsingKeyFrames();

            EasingVector3DKeyFrame fromk = null;
            if (FromVector3D.HasValue) {
                fromk=new EasingVector3DKeyFrame(FromVector3D.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingVector3DKeyFrame tok = null;
            if (ToVector3D.HasValue)
            {
                tok = new EasingVector3DKeyFrame(ToVector3D.Value, TimeSpan.FromMilliseconds(AniTime(1)));
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
