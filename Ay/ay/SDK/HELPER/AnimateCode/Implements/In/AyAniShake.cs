/**----------------------------------------------- 
 * * ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权========= 
 * 
 * 作者：ay * 联系QQ：875556003 * 时间2019-06-14 
 * -----------------------------------------*/
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.Animate
{
    public class AyAniShake : AyAnimateBaseIn
    {

        #region 构造函数
        public AyAniShake(FrameworkElement _element)
          : base("shake", _element)
        {
            base.AnimateSpeed = 320;
            Initialize();
        }

        public AyAniShake(FrameworkElement _element, Action _completed)
            : base("shake", _element, _completed)
        {
            base.AnimateSpeed = 320;
        }
        #endregion
        #region 属性
        private double moveWidth = 12;

        public double MoveWidth
        {
            get { return moveWidth; }
            set { moveWidth = value; }
        }


        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dau = null;
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
                Win.RegisterName(translation);
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
                        Win.RegisterName(translation);
                    }
                }
                else
                {
                    translation = new TranslateTransform(0, 0);
                    Win.RegisterName(translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion
            double danqianX = translation.X;
            var k2 = new EasingDoubleKeyFrame((danqianX - MoveWidth), TimeSpan.FromMilliseconds(AniTime(0.25)));
            var k2_1 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k2_2 = new EasingDoubleKeyFrame((danqianX + MoveWidth), TimeSpan.FromMilliseconds(AniTime(0.75)));
            var k2_3 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(1)));
            Story = (Storyboard)Story.Clone();
            Storyboard.SetTargetName(dau, Win.GetName(translation));

            Storyboard.SetTargetProperty(dau, new PropertyPath(TranslateTransform.XProperty));

            Win.RegisterResource(Story);
            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k2_1);
            dau.KeyFrames.Add(k2_2);
            dau.KeyFrames.Add(k2_3); ;
            dau.RepeatBehavior = new RepeatBehavior(3);
            Story.Children.Add(dau);



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
                Element.Opacity = 1;
                Element.Visibility = Visibility.Visible;
                base.CallClientCompleted();
            }
            catch
            {
            }
        }
    }
}
