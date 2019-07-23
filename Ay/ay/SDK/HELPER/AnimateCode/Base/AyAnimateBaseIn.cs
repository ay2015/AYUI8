
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ay.Animate
{
    /// <summary>
    /// 2019-06-13 10:55:23 version:3.0
    /// 可重复执行动画，不重复创建对象
    /// </summary>
    public abstract class AyAnimateBaseIn : AyAnimateBase
    {
        public AyAnimateBaseIn()
        {
        }

        public AyAnimateBaseIn(FrameworkElement _element) : base(_element)
        {
        }
        public AyAnimateBaseIn(string _name, FrameworkElement _element) : base(_name, _element)
        {

        }
        public AyAnimateBaseIn(FrameworkElement _element, Action _completed) : base(_element, _completed)
        {
        }

        public AyAnimateBaseIn(string _name, FrameworkElement _element, Action _completed) : base(_name, _element, _completed)
        {
        }

        protected override void SetBaseView()
        {
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;
            Element.Opacity = 1;
        }

    }
}
