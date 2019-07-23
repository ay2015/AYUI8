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
    public class AyAniTranslate : AyAnimateBaseIn
    {
        #region 构造函数


        public AyAniTranslate(FrameworkElement _element)
            : base("Translate", _element)
        { base.AnimateSpeed = 350; }

        public AyAniTranslate(FrameworkElement _element, Action _completed)
            : base("Translate", _element, _completed)
        {
            base.AnimateSpeed = 350;
        }
        #endregion
        #region 属性


        private double? translateXFrom;

        public double? TranslateXFrom
        {
            get { return translateXFrom; }
            set { translateXFrom = value; }
        }

        private double? translateYFrom;

        public double? TranslateYFrom
        {
            get { return translateYFrom; }
            set { translateYFrom = value; }
        }
        

        private double? translateXAdd;

        public double? TranslateXAdd
        {
            get { return translateXAdd; }
            set { translateXAdd = value; }
        }

        private double? translateYAdd;

        public double? TranslateYAdd
        {
            get { return translateYAdd; }
            set { translateYAdd = value; }
        }


        private double? translateXTo;

        public double? TranslateXTo
        {
            get { return translateXTo; }
            set { translateXTo = value; }
        }

        private double? translateYTo;

        public double? TranslateYTo
        {
            get { return translateYTo; }
            set { translateYTo = value; }
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
            TranslateTransform translation = new TranslateTransform();
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
                translation = new TranslateTransform();
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
                        Win.RegisterName( translation);
                    }
                }
                else
                {
                    translation = new TranslateTransform();
                    Win.RegisterName( translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            double sx = translation.X;
            double sy = translation.Y;


            if (!TranslateXFrom.HasValue && !TranslateYFrom.HasValue && TranslateXTo.HasValue && TranslateXTo == sx && TranslateYTo.HasValue && TranslateYTo == sy)
            {
                return;
            }
            EasingDoubleKeyFrame k2_xFrom = null;
            EasingDoubleKeyFrame k2_yFrom = null;
            if (TranslateXFrom.HasValue)
            {
                k2_xFrom = new EasingDoubleKeyFrame(TranslateXFrom.Value, TimeSpan.FromMilliseconds(AniTime(0)));
            }

            if (TranslateYFrom.HasValue)
            {
                k2_yFrom = new EasingDoubleKeyFrame(TranslateYFrom.Value, TimeSpan.FromMilliseconds(AniTime(0)));
            }


            EasingDoubleKeyFrame k2_x = null;
            EasingDoubleKeyFrame k2_y = null;
            if (TranslateXAdd.HasValue)
            {
                k2_x = new EasingDoubleKeyFrame(TranslateXAdd.Value + sx, TimeSpan.FromMilliseconds(AniTime(1)));
            }
            else if (TranslateXTo.HasValue)
            {
                k2_x = new EasingDoubleKeyFrame(TranslateXTo.Value, TimeSpan.FromMilliseconds(AniTime(1)));
            }

            if (TranslateYAdd.HasValue)
            {
                k2_y = new EasingDoubleKeyFrame(TranslateYAdd.Value + sx, TimeSpan.FromMilliseconds(AniTime(1)));
            }
            else if (TranslateYTo.HasValue)
            {
                k2_y = new EasingDoubleKeyFrame(TranslateYTo.Value, TimeSpan.FromMilliseconds(AniTime(1)));
            }


            if (EasingFunction != null)
            {
                k2_x.EasingFunction = EasingFunction;
                k2_y.EasingFunction = EasingFunction;
            }


            Storyboard.SetTargetName(dau1, Win.GetName(translation));
            Storyboard.SetTargetProperty(dau1, new PropertyPath(TranslateTransform.XProperty));
            Storyboard.SetTargetName(dau2, Win.GetName(translation)); 
            Storyboard.SetTargetProperty(dau2, new PropertyPath(TranslateTransform.YProperty));
            if (AniAutoReverse.HasValue)
            {
                Story.AutoReverse = AniAutoReverse.Value;
            }

            if (k2_xFrom != null)
                dau1.KeyFrames.Add(k2_xFrom);
            if (k2_yFrom != null)
                dau2.KeyFrames.Add(k2_yFrom);

            if(k2_x!=null)
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
