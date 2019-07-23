using System;
using System.Collections.Generic;
using System.Windows;

namespace ay.Controls
{
    public class AyAnimateService
    {
        [ThreadStatic]
        private static Dictionary<object, WeakReference> _references = null;

        public static Dictionary<object, WeakReference> References
        {
            get
            {
                if (_references == null)
                {
                    _references = new Dictionary<object, WeakReference>();
                }
                return _references;
            }
        }



        public static string GetAnimateKey(DependencyObject obj)
        {
            return (string)obj.GetValue(AnimateKeyProperty);
        }

        public static void SetAnimateKey(DependencyObject obj, string value)
        {
            obj.SetValue(AnimateKeyProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnimateKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimateKeyProperty =
            DependencyProperty.RegisterAttached("AnimateKey", typeof(string), typeof(DependencyObject), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnAnimateKeyChanged)));

        private static void OnAnimateKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement t)
            {
                References[e.NewValue] = new WeakReference(t);
            }
        }
    }

}
