
using System;
using System.Windows;

namespace ay.Animate
{
    public abstract class AyAnimateBaseOut : AyAnimateBase
    {
        public AyAnimateBaseOut()
        {
        }

        public AyAnimateBaseOut(FrameworkElement _element) : base(_element)
        {
        }
        public AyAnimateBaseOut(string _name, FrameworkElement _element) : base(_name, _element)
        {

        }
        public AyAnimateBaseOut(FrameworkElement _element, Action _completed) : base(_element, _completed)
        {
        }

        public AyAnimateBaseOut(string _name, FrameworkElement _element, Action _completed) : base(_name, _element, _completed)
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
