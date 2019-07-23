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
    public class AyAniScale : AyAnimateBaseIn
    {
        #region 构造函数
        public AyAniScale()
        {
            AnimateName = "Scale";
            base.AnimateSpeed = 350;
        }
        public AyAniScale(FrameworkElement _element)
       : base("Scale", _element)
        { base.AnimateSpeed = 350; }

        public AyAniScale(FrameworkElement _element, Action _completed)
            : base("Scale", _element, _completed)
        {
            base.AnimateSpeed = 350;
        }
        #endregion
        #region 属性

        private double? scaleXFrom;

        public double? ScaleXFrom
        {
            get { return scaleXFrom; }
            set { scaleXFrom = value; }
        }

        private double? scaleYFrom;

        public double? ScaleYFrom
        {
            get { return scaleYFrom; }
            set { scaleYFrom = value; }
        }


        private double? scaleXAdd;

        public double? ScaleXAdd
        {
            get { return scaleXAdd; }
            set { scaleXAdd = value; }
        }

        private double? scaleYAdd;

        public double? ScaleYAdd
        {
            get { return scaleYAdd; }
            set { scaleYAdd = value; }
        }


        private double? scaleXTo;

        public double? ScaleXTo
        {
            get { return scaleXTo; }
            set { scaleXTo = value; }
        }

        private double? scaleYTo;

        public double? ScaleYTo
        {
            get { return scaleYTo; }
            set { scaleYTo = value; }
        }



        private bool? aniAutoReverse;

        public bool? AniAutoReverse
        {
            get { return aniAutoReverse; }
            set { aniAutoReverse = value; }
        }

        private IEasingFunction easingFunction;

        public IEasingFunction EasingFunction
        {
            get { return easingFunction; }
            set { easingFunction = value; }
        }
        #endregion

        #region KeyFrames
        DoubleAnimationUsingKeyFrames dau1 = null;
        DoubleAnimationUsingKeyFrames dau2 = null;
        #endregion


        protected override void Init()
        {
            SetBaseView();
            ScaleTransform translation = new ScaleTransform();
            dau1 = new DoubleAnimationUsingKeyFrames();
            dau2 = new DoubleAnimationUsingKeyFrames();
            #region 基本工作，确定类型和name
            //是否存在TranslateTransform
            //动画要的类型是否存在
            //动画要的类型的name是否存在，不存在就注册，结束后取消注册，删除动画
            var ex = Element.RenderTransform;
            if (ex == null || (ex as System.Windows.Media.MatrixTransform) != null)
            {
                var tg = new TransformGroup();
                translation = new ScaleTransform();
                Win.RegisterName(translation);
                tg.Children.Add(translation);
                Element.RenderTransform = tg;
            }
            else
            {
                var tg = ex as TransformGroup;
                foreach (var item in tg.Children)
                {
                    translation = item as ScaleTransform;
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
                    translation = new ScaleTransform();

                    Win.RegisterName(translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            double sx = translation.ScaleX;
            double sy = translation.ScaleY;


            if (!ScaleXFrom.HasValue && !ScaleYFrom.HasValue && ScaleXTo.HasValue && ScaleXTo == sx && ScaleYTo.HasValue && ScaleYTo == sy)
            {
                return;
            }



            EasingDoubleKeyFrame k2_xFrom = null;
            EasingDoubleKeyFrame k2_yFrom = null;
            if (ScaleXFrom.HasValue)
            {
                k2_xFrom = new EasingDoubleKeyFrame(ScaleXFrom.Value, TimeSpan.FromMilliseconds(AniTime(0)));
            }

            if (ScaleYFrom.HasValue)
            {
                k2_yFrom = new EasingDoubleKeyFrame(ScaleYFrom.Value, TimeSpan.FromMilliseconds(AniTime(0)));
            }


            EasingDoubleKeyFrame k2_x = null;
            EasingDoubleKeyFrame k2_y = null;
            if (ScaleXAdd.HasValue)
            {
                k2_x = new EasingDoubleKeyFrame(ScaleXAdd.Value + sx, TimeSpan.FromMilliseconds(AniTime(1)));
            }
            else if (ScaleXTo.HasValue)
            {
                k2_x = new EasingDoubleKeyFrame(ScaleXTo.Value, TimeSpan.FromMilliseconds(AniTime(1)));
            }

            if (ScaleYAdd.HasValue)
            {
                k2_y = new EasingDoubleKeyFrame(ScaleYAdd.Value + sx, TimeSpan.FromMilliseconds(AniTime(1)));
            }
            else if (ScaleYTo.HasValue)
            {
                k2_y = new EasingDoubleKeyFrame(ScaleYTo.Value, TimeSpan.FromMilliseconds(AniTime(1)));
            }


            if (EasingFunction != null)
            {
                if (k2_x != null)
                {
                    k2_x.EasingFunction = EasingFunction;
                }
         
                if (k2_y != null)
                {
                    k2_y.EasingFunction = EasingFunction;
                }
              
            }


            Storyboard.SetTargetName(dau1, Win.GetName(translation));
            Storyboard.SetTargetProperty(dau1, new PropertyPath(ScaleTransform.ScaleXProperty));
            Storyboard.SetTargetName(dau2, Win.GetName(translation));
            Storyboard.SetTargetProperty(dau2, new PropertyPath(ScaleTransform.ScaleYProperty));
            if (AniAutoReverse.HasValue)
            {
                Story.AutoReverse = AniAutoReverse.Value;
            }

            if (k2_xFrom != null)
                dau1.KeyFrames.Add(k2_xFrom);
            if (k2_yFrom != null)
                dau2.KeyFrames.Add(k2_yFrom);

            if (k2_x != null)
                dau1.KeyFrames.Add(k2_x);
            if (k2_y != null)
                dau2.KeyFrames.Add(k2_y);

            Win.RegisterResource(Story);
            Story = (Storyboard)Story.CloneCurrentValue();
            Story.Children.Add(dau1);
            Story.Children.Add(dau2);

            Story.Completed += Story_Completed;
        }

        public override void Destroy()
        {
            Story.Completed -= Story_Completed;
            Win.UnRegisterNameAll();
            Win.UnRegisterResource(Story);
            dau1.KeyFrames.Clear();
            dau1 = null;
            dau2.KeyFrames.Clear();
            dau2 = null;
            Story = null;
        }
        private void Story_Completed(object sndr, EventArgs evtArgs)
        {
            try
            {
                Element.Visibility = Visibility.Visible;
                Element.Opacity = 1;

                base.CallClientCompleted();
            }
            catch
            {
            }
        }

    }
}
