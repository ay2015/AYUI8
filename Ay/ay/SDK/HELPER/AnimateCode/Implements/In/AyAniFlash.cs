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

    public class AyAniFlash : AyAnimateBaseIn
    {

        #region 构造函数

        public AyAniFlash(FrameworkElement _element)
            : base("flash", _element)
        { base.AnimateSpeed = 500;Initialize(); }

        public AyAniFlash(FrameworkElement _element, Action _completed)
            : base("flash", _element, _completed)
        { base.AnimateSpeed = 500; }
        #endregion
        #region 属性

        private int flashCount = 2;
        /// <summary>
        /// -1代表一直闪，>0的值
        /// </summary>
        public int FlashCount
        {
            get { return flashCount; }
            set
            {
                if (value == 0)
                {
                    throw new Exception("闪烁次数不能为0");
                }
                flashCount = value;
            }
        }
        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dau = null;
        #endregion


        protected override void Init()
        {
            SetBaseView();

            dau = new DoubleAnimationUsingKeyFrames();

            var k2 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k3 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));

            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k3);
            if (FlashCount < 0)
            {
                dau.RepeatBehavior = RepeatBehavior.Forever;
            }
            else
            {
                dau.RepeatBehavior = new RepeatBehavior(FlashCount);
            }
            Story = (Storyboard)Story.CloneCurrentValue();
            Story.Children.Add(dau);

            Storyboard.SetTarget(dau, Element);
            Storyboard.SetTargetProperty(dau, new PropertyPath(UIElement.OpacityProperty));
            Story.Completed -= Story_Completed;
            Story.Completed += Story_Completed;
        }

        public override void Destroy()
        {
            Story.Completed -= Story_Completed;
            dau.KeyFrames.Clear();
            dau = null;
            Story = null;
        }
        private void Story_Completed(object sndr, EventArgs evtArgs)
        {
            try
            {
                base.CallClientCompleted();
            }
            catch
            {
            }
        }

    }
}
