
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace ay.Animate
{
    /// <summary>
    /// 2019-06-13 10:55:23 version:3.0
    /// 可重复执行动画，不重复创建对象
    /// </summary>
    public abstract class AyAnimateBase : FrameworkElement, IAyAnimateProperty, INameRegister, IDisposable
    {
        #region 构造函数
        public AyAnimateBase()
        {
        }

        public AyAnimateBase(FrameworkElement _element)
        {
            this.Element = _element;
            win = new NameRegister(Element);
        }

        public AyAnimateBase(FrameworkElement _element, Action _completed)
        {
            this.Completed = _completed;
            this.Element = _element;
            win = new NameRegister(Element);
        }
        public AyAnimateBase(string _name, FrameworkElement _element)
        {
            this.AnimateName = _name;
            this.Element = _element;
            win = new NameRegister(Element);
        }
        public AyAnimateBase(string _name, FrameworkElement _element, Action _completed)
        {
            this.AnimateName = _name;
            this.Completed = _completed;
            this.Element = _element;
            win = new NameRegister(Element);
        }

        #endregion

        #region 注册
        private NameRegister win;
        /// <summary>
        /// 执行注册控件名字Name
        /// </summary>
        public NameRegister Win
        {
            get
            {
                if (win == null)
                {
                    win = new NameRegister(Element);
                }
                return win;
            }
        }
        #endregion

        #region 属性
        //private Storyboard story=new Storyboard();
        ///// <summary>
        ///// 故事版
        ///// </summary>
        //public Storyboard Story
        //{
        //    get { return story; }
        //    set { story = value; }
        //}
        private string _StoryName;

        public string StoryName
        {
            get { return _StoryName; }
            set { _StoryName = value; }
        }

        /// <summary>
        /// 故事
        /// </summary>
        public Storyboard Story
        {
            get
            {
                var _1 = (Storyboard)GetValue(StoryProperty);
                //Initialize();
                return _1;
            }
            set { SetValue(StoryProperty, value); }
        }

        public static readonly DependencyProperty StoryProperty =
            DependencyProperty.Register("Story", typeof(Storyboard), typeof(AyAnimateBase), new PropertyMetadata(new Storyboard()));

        /// <summary>
        /// 动画完成后执行
        /// </summary>
        public Action Completed { get; set; }
        private bool IsSetCompleted = false;
        /// <summary>
        /// 只设置一次complete
        /// </summary>
        /// <param name="ac"></param>
        public void OnlySetOnceCompleted(Action ac)
        {
            if (!IsSetCompleted)
            {
                Completed = ac;
                IsSetCompleted = true;
            }
           
        }

        private bool isCompleted = true;
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }
        /// <summary>
        /// 是否开始
        /// </summary>
        public bool IsBegined { get; set; }
        /// <summary>
        /// 频繁触发开始Begin事件，是否每次重新开始,默认不是
        /// </summary>
        public bool IsEveryTimeStartOver { get; set; } = false;

        /// <summary>
        ///被执行动画的元素
        /// </summary>
        [ConstructorArgumentAttribute("_element")]
        public FrameworkElement Element
        {
            get { return (FrameworkElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(AyAnimateBase), new PropertyMetadata(null));

        //private static void dd(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is AyAnimateBase ddd)
        //    {
        //        ddd.Initialize();
        //    }
        //}

        private string animateName;
        /// <summary>
        /// 动画名字
        /// </summary>
        public string AnimateName
        {
            get { return animateName; }
            set { animateName = value; }
        }
        /// <summary>
        /// 是否初始化
        /// </summary>
        public new bool IsInitialized { get; set; }
        /// <summary>
        /// 是否自动销毁
        /// </summary>
        public bool AutoDestory { get; set; }
        #endregion

        /// <summary>
        /// 初始化，要求子类必须实现
        /// </summary>
        protected abstract void Init();
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            //story = new Storyboard();
            Init();
        }
        /// <summary>
        /// 重置初始化动画，销毁后，可以调用
        /// </summary>
        public virtual void ReInitialize()
        {
            IsInitialized = false;
        }
        /// <summary>
        /// 开始动画，内部会设置IsInitialized=true
        /// </summary>
        public virtual void Begin()
        {
            if (!IsInitialized)
            {
                Initialize();
                IsInitialized = true;
            }
            if (Story == null) return;
            if (IsEveryTimeStartOver)
            {
                SetBaseView();
                Story.Begin(Element, true);
                IsBegined = true;
            }
            else
            {
                if (IsBegined) return;
                SetBaseView();
                Story.Begin(Element, true);
                IsBegined = true;
            }
        }
        /// <summary>
        /// 执行之前设置可见性
        /// </summary>
        protected virtual void SetBaseView() { }

        /// <summary>
        /// 暂停动画
        /// </summary>
        public virtual void Pauze()
        {
            SetStoryBoardActivity(false);
        }
        /// <summary>
        /// 继续动画
        /// </summary>
        public virtual void Resume()
        {
            SetStoryBoardActivity(true);
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        public virtual void Stop()
        {
            if (Story == null) return;
            Story.Stop(Element);
            IsCompleted = true;
            IsBegined = false;
        }
        /// <summary>
        /// 跳过到Filled状态
        /// </summary>
        public virtual void SkipToFill()
        {
            Story.SkipToFill(Element);
            IsCompleted = true;
            IsBegined = false;
        }


        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Destroy() { }
        #region 拓展
        /// <summary>
        /// 设置故事版状态
        /// </summary>
        /// <param name="play"></param>
        private void SetStoryBoardActivity(bool play)
        {
            if (Story == null) return;
            if (play)
            {
                Story.Resume(Element);
            }
            else
            {
                Story.Pause(Element);
            }
        }
        /// <summary>
        /// 客户端回调完成
        /// </summary>
        public virtual void CallClientCompleted()
        {
            IsCompleted = true;
            if (Completed != null)
            {
                Completed();
            }
            if (NextAyAnimateTreeItem != null)
            {
                NextAyAnimateTreeItem();
            }
            IsBegined = false;
            if (AutoDestory)
            {
                Destroy();
            }
        }
        /// <summary>
        /// 给Ay动画树用的变量，子动画无需管
        /// </summary>
        internal Action NextAyAnimateTreeItem;
        private double animateSpeed = 500;
        /// <summary>
        /// 动画执行总时长
        /// </summary>
        public double AnimateSpeed
        {
            get { return animateSpeed; }
            set { animateSpeed = value; }
        }




        /// <summary>
        /// 计算总时长
        /// </summary>
        /// <param name="percent">百分比</param>
        /// <returns></returns>
        public virtual double AniTime(double percent)
        {
            return AnimateSpeed * percent;
        }

        #endregion
        #region 释放
        /// <summary>
        /// 释放动画，提供给GC调用
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Completed = null;
            }
        }


        ~AyAnimateBase()
        {
            this.Dispose(false);
        }
        #endregion


    }

}
