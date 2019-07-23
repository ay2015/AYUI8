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
    public class AyAniBounce : AyAnimateBaseIn
    {
        #region 构造函数

        public AyAniBounce(FrameworkElement _element)
            : base("bounce", _element)
        { base.AnimateSpeed = 5000; Initialize(); }

        public AyAniBounce(FrameworkElement _element, bool autoDestroy = false)
    : base("bounce", _element)
        { base.AnimateSpeed = 1000; AutoDestory = autoDestroy; }

        public AyAniBounce(FrameworkElement _element, Action _completed)
            : base("bounce", _element, _completed)
        { base.AnimateSpeed = 1000; }

        #endregion
        #region 属性

        private double upHeight = 35;
        /// <summary>
        /// 默认是35
        /// </summary>
        public double UpHeight
        {
            get { return upHeight; }
            set { upHeight = value; }
        }


        private int bounces = 3;
        /// <summary>
        /// 默认值是3
        /// </summary>
        public int Bounces
        {
            get { return bounces; }
            set { bounces = value; }
        }

        private int bounciness = 2;
        /// <summary>
        /// 默认值是2
        /// </summary>
        public int Bounciness
        {
            get { return bounciness; }
            set { bounciness = value; }
        }

        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dau = null;
        #endregion

        protected override void Init()
        {

            SetBaseView();
            dau = new DoubleAnimationUsingKeyFrames();
            TranslateTransform translation = new TranslateTransform(0, 0);
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
            double danqianY = translation.Y;
            var k2 = new EasingDoubleKeyFrame((danqianY - UpHeight), TimeSpan.FromMilliseconds(AniTime(0.2)), new PowerEase { EasingMode = EasingMode.EaseOut });
            var k3 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(AniTime(1)), new BounceEase { EasingMode = EasingMode.EaseOut, Bounces = this.Bounces, Bounciness = this.Bounciness });

            //dau = new DoubleAnimationUsingKeyFrames {KeyFrames=new DoubleKeyFrameCollection { k2,k3} };
            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k3);
            Storyboard.SetTargetName(dau, Win.GetName(translation));
            Storyboard.SetTargetProperty(dau, new PropertyPath(TranslateTransform.YProperty));
            Story = (Storyboard)Story.CloneCurrentValue();
            Story.Children.Add(dau);
            //Story.Animate = this;
            Win.RegisterResource(Story);
            Story.Completed -= Story_Completed;
            Story.Completed += Story_Completed;

        }

        public override void Destroy()
        {
            Story.Completed -= Story_Completed;
            Win.UnRegisterNameAll();
            Win.UnRegisterResource(Story);
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
