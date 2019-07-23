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
    public class AyAniBounceOutUp : AyAnimateBaseOut
    {

        #region 构造函数
        public AyAniBounceOutUp(FrameworkElement _element)
           : base("BounceOutUp", _element)
        { base.AnimateSpeed = 900; }

        public AyAniBounceOutUp(FrameworkElement _element, Action _completed)
            : base("BounceOutUp", _element, _completed)
        {
            base.AnimateSpeed = 900;
        }

        #endregion
        #region 属性



        private double oneValue = 0;

        public double OneValue
        {
            get { return oneValue; }
            set { oneValue = value; }
        }

        private double twoValue = 20;

        public double TwoValue
        {
            get { return twoValue; }
            set { twoValue = value; }
        }

        private double threeValue = -45;

        public double ThreeValue
        {
            get { return threeValue; }
            set { threeValue = value; }
        }

        private double fourValue = 2000;

        public double FourValue
        {
            get { return fourValue; }
            set { fourValue = value; }
        }

        #endregion

        #region KeyFrames
    
        DoubleAnimationUsingKeyFrames dauY = null;
        DoubleAnimationUsingKeyFrames dauOpacty = null;
        #endregion




        protected override void Init()
        {
            SetBaseView();


            TranslateTransform translation = new TranslateTransform(0, 0);

            dauY = new DoubleAnimationUsingKeyFrames();
            #region 基本工作，确定类型和name
            //是否存在TranslateTransform
            //动画要的类型是否存在
            //动画要的类型的name是否存在，不存在就注册，结束后取消注册，删除动画
            var ex = Element.RenderTransform;
            if (ex == null || (ex as System.Windows.Media.MatrixTransform) != null)
            {
                var tg = new TransformGroup();
                translation =new TranslateTransform(0, 0);
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
            Win.RegisterResource(Story);
            Story = (Storyboard)Story.CloneCurrentValue();
            var k2 = new EasingDoubleKeyFrame(OneValue, TimeSpan.FromMilliseconds(0));
            var k2_0 = new EasingDoubleKeyFrame(TwoValue, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k2_1 = new EasingDoubleKeyFrame(ThreeValue, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k2_2 = new EasingDoubleKeyFrame(ThreeValue, TimeSpan.FromMilliseconds(AniTime(0.45)));
            var k2_3 = new EasingDoubleKeyFrame(FourValue, TimeSpan.FromMilliseconds(AniTime(1)));


            Storyboard.SetTargetName(dauY, Win.GetName(translation));
            Storyboard.SetTargetProperty(dauY, new PropertyPath(TranslateTransform.YProperty));
            dauY.KeyFrames.Add(k2);
            dauY.KeyFrames.Add(k2_0);
            dauY.KeyFrames.Add(k2_1);
            dauY.KeyFrames.Add(k2_2);
            dauY.KeyFrames.Add(k2_3);
            Story.Children.Add(dauY);
            dauY.FillBehavior = FillBehavior.Stop;

            dauOpacty = new DoubleAnimationUsingKeyFrames();
            var k3 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0)));
            var k3_0 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0.45)));
            var k3_1 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)));

            dauOpacty.KeyFrames.Add(k3);
            dauOpacty.KeyFrames.Add(k3_0);
            dauOpacty.KeyFrames.Add(k3_1);
            Storyboard.SetTarget(dauOpacty, Element);
            dauOpacty.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTargetProperty(dauOpacty, new PropertyPath(UIElement.OpacityProperty));
            Story.Children.Add(dauOpacty);
            Story.Completed += Story_Completed;
        }

        public override void Destroy()
        {
            Story.Completed -= Story_Completed;
            Win.UnRegisterNameAll();
            Win.UnRegisterResource(Story);
            dauY.KeyFrames.Clear();
            dauY = null;
            dauOpacty.KeyFrames.Clear();
            dauOpacty = null;
            Story = null;
        }
        private void Story_Completed(object sndr, EventArgs evtArgs)
        {
            try
            {
                Element.Visibility = Visibility.Collapsed;
                base.CallClientCompleted();
            }
            catch
            {
            }
        }


    }
}
