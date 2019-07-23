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


    /// <summary>
    /// 拓展父类属性，给类型动画空间
    /// </summary>
    public abstract class AyAnimateTypeBase : AyAnimateBase
    {
        private FillBehavior? _FillBehaviorAy;

        public FillBehavior? FillBehaviorAy
        {
            get { return _FillBehaviorAy; }
            set { _FillBehaviorAy = value; }
        }
        public AyAnimateTypeBase()
        {

        }
        public AyAnimateTypeBase(string _name, FrameworkElement _element)
            : base(_name, _element)
        { }

        public AyAnimateTypeBase(string _name, FrameworkElement _element, Action _completed)
            : base(_name, _element, _completed)
        {

        }

        #region 单独给需要的类型的动画预留的
        private PropertyPath aniPropertyPath;

        public PropertyPath AniPropertyPath
        {
            get { return aniPropertyPath; }
            set { aniPropertyPath = value; }
        }

        #endregion


        private RepeatBehavior? aniRepeatBehavior;

        public RepeatBehavior? AniRepeatBehavior
        {
            get { return aniRepeatBehavior; }
            set { aniRepeatBehavior = value; }
        }

        private bool? aniAutoReverse;

        public bool? AniAutoReverse
        {
            get { return aniAutoReverse; }
            set { aniAutoReverse = value; }
        }

        #region 缓冲动画层控制，下面属性，只需要赋值1个
        private int aniEasingMode;
        /// <summary>
        /// easymode  1代表to   2代表out   3代表  inout   0什么都不加
        /// </summary>
        public int AniEasingMode
        {
            get { return aniEasingMode; }
            set { aniEasingMode = value; }
        }

        private IEasingFunction aniEasingFunction;

        public IEasingFunction AniEasingFunction
        {
            get { return aniEasingFunction; }
            set { aniEasingFunction = value; }
        }

        #endregion


        private string elementname;

        public string ElementName
        {
            get { return elementname; }
            set { elementname = value; }
        }

        /// <summary>
        /// 是否自动附加属性
        /// </summary>

        private bool isAutoAddName;

        internal bool IsAutoAddName
        {
            get { return isAutoAddName; }
            set { isAutoAddName = value; }
        }


        private CircleEase cirDefault;

        public CircleEase CirDefault
        {
            get { return cirDefault; }
            set { cirDefault = value; }
        }
        /// <summary>
        /// 拓展类额外的动画完成时候需要的操作
        /// </summary>
        private Action extCompleteAction;

        protected Action ExtCompleteAction
        {
            get { return extCompleteAction; }
            set { extCompleteAction = value; }
        }

        /// <summary>
        /// 类型动画子类去实现
        /// 2016-7-8 10:37:08
        /// </summary>
        public abstract void CreateStoryboard();

        protected void SetPropertyPath(object para)
        {
            if (AniPropertyPath == null)
            {
                AniPropertyPath = new PropertyPath(para);
            }
        }

        protected override void SetBaseView()
        {
            //Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;
            Element.Opacity = 1;
        }

        /// <summary>
        ///  模板模式,真正对外暴露的方法名要一致
        /// </summary>
        /// <returns></returns>
        protected override void Init()
        {
            SetBaseView();
            ///2017-6-9 16:03:52 增加 方便动画结束后的控制
            if (FillBehaviorAy.HasValue)
            {
                Story.FillBehavior = FillBehaviorAy.Value;
            }

            IsCompleted = false;
            RegisterName();
            CreateStoryboard();
            CompleteTypeStory();
        }
        //       public override AyAnimateBase Animate()
        //{
        //    RegisterName();
        //    CreateStoryboard();
        //    CompleteTypeStory();
        //    return this;
        //}

        private void RegisterName()
        {
            Element.Visibility = Visibility.Visible;

            var tex1 = Story.GetValue(FrameworkElement.NameProperty);
            if (tex1 == null || tex1.ToString() == "")
            {
                StoryName = "aystory" + base.AnimateName + AyCommon.GetGuidNoSplit;

                Win.Resources.Add(StoryName, Story);
            }
            else
            {
                StoryName = tex1.ToString();
            }
            Story = (Storyboard)Story.CloneCurrentValue();
            var tex = Element.GetValue(FrameworkElement.NameProperty);
            if (tex == null || tex.ToString() == "")
            {
                ElementName = "ay" + base.AnimateName + Element.GetHashCode();
                Win.RegisterName(ElementName, Element);
                IsAutoAddName = true;
            }
            else
            {
                ElementName = tex.ToString();
            }

            CircleEase cirDefault = null;
            switch (AniEasingMode)
            {

                case 1:
                    cirDefault = new CircleEase();
                    cirDefault.EasingMode = EasingMode.EaseIn;
                    break;
                case 2:
                    cirDefault = new CircleEase();
                    cirDefault.EasingMode = EasingMode.EaseOut;
                    break;
                case 3:
                    cirDefault = new CircleEase();
                    cirDefault.EasingMode = EasingMode.EaseInOut;
                    break;
                default:

                    break;
            }
        }

        private void CompleteTypeStory()
        {
            if (AniRepeatBehavior.HasValue)
            {
                Story.RepeatBehavior = AniRepeatBehavior.Value;
            }

            if (AniAutoReverse.HasValue)
            {
                Story.AutoReverse = AniAutoReverse.Value;
            }
            Story.Completed -= Story_Completed;
            Story.Completed += Story_Completed;
        }

        private void Story_Completed(object sender, EventArgs e)
        {
            RaiseStoryComplete();
        }
        public override void Destroy()
        {
            if (Story != null)
            {
                Story.Completed -= Story_Completed;
                Story.Stop();

                Story.Children.Clear();
                Story = null;
            }
        }

        public void RaiseStoryComplete()
        {
            try
            {
                if (IsAutoAddName)
                {
                    Win.UnregisterName(ElementName);
                }
                Win.UnRegisterResourceByName(StoryName);
                if (ExtCompleteAction != null) ExtCompleteAction();
                base.CallClientCompleted();
            }
            catch
            {

            }
        }

    }
}
