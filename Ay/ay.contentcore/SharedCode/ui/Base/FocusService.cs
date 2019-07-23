using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ay.Controls
{
    public class FocusService : DependencyObject
    {
        public FocusService()
        {

        }


        public static bool GetClickLostFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClickLostFocusProperty);
        }

        public static void SetClickLostFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(ClickLostFocusProperty, value);
        }

        // Using a DependencyProperty as the backing store for ClickLostFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickLostFocusProperty =
            DependencyProperty.RegisterAttached("ClickLostFocus", typeof(bool), typeof(FocusService), new PropertyMetadata(false, OnClickLostFocusChanged));

        private static void OnClickLostFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _0 = d as FrameworkElement;
            if (_0!=null)
            {
                var _1 = (bool)e.NewValue;
                if (_1)
                {
                    var _11 = d as Panel;
                    if (_11!=null && _11.Background.IsNull())
                    {
                        _11.Background = new SolidColorBrush(Colors.Transparent);
                    }
                    _0.MouseDown += _0_MouseDown;
                }
                else
                {
                    _0.MouseDown -= _0_MouseDown;
                }
            }
        }

        private static void _0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }


    }
}
