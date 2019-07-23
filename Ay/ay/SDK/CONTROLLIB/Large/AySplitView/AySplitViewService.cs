/**-----------------------------------------------
 * ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权=========
 * 作者：ay
 * 联系QQ：875556003
 * 时间2016-6-24 15:27:39
 * 最后修改：2017-9-7 10:19:16
 * -----------------------------------------*/
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ay.Controls
{
    public static class AySplitViewService
    {
        public static AySplitView GetToggleMenu(DependencyObject obj)
        {
            return (AySplitView)obj.GetValue(ToggleMenuProperty);
        }

        public static void SetToggleMenu(DependencyObject obj, AySplitView value)
        {
            obj.SetValue(ToggleMenuProperty, value);
        }

        // Using a DependencyProperty as the backing store for ToggleMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleMenuProperty =
            DependencyProperty.RegisterAttached("ToggleMenu", typeof(AySplitView), typeof(AySplitViewService), new PropertyMetadata(null, ToggleMenuChanged));

        private static void ToggleMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = d as Button;
            if (_1.IsNotNull())
            {
                var _2 = e.NewValue as AySplitView;
                _1.Click += (send, er) =>
                {
                    _2.IsPaneOpen = !_2.IsPaneOpen;
                };
            }
            else
            {
                var _11 = d as ToggleButton;
                if (_11.IsNotNull())
                {
                    var _2 = e.NewValue as AySplitView;
                    _2.IsPaneOpen = _11.IsChecked.Value;
                }
            }
        }

    }
}
