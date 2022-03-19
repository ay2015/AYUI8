using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ay.Controls
{
    public class TouchScrolling : DependencyObject
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TouchScrolling), new UIPropertyMetadata(false, IsEnabledChanged));

        static Dictionary<object, MouseCapture> _captures = new Dictionary<object, MouseCapture>();

        static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ScrollViewer;
            if (target == null) return;

            if ((bool)e.NewValue)
            {
                target.Loaded += target_Loaded;
            }
            else
            {
                target_Unloaded(target, new RoutedEventArgs());
            }
        }

        static void target_Unloaded(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Target Unloaded");

            var target = sender as ScrollViewer;
            if (target == null) return;

            _captures.Remove(sender);

            target.Loaded -= target_Loaded;
            target.Unloaded -= target_Unloaded;
            target.PreviewMouseLeftButtonDown -= target_PreviewMouseLeftButtonDown;
            target.PreviewMouseMove -= target_PreviewMouseMove;

            target.PreviewMouseLeftButtonUp -= target_PreviewMouseLeftButtonUp;
        }

        static void target_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target == null) return;

            _captures[sender] = new MouseCapture
            {
                VerticalOffset = target.VerticalOffset,
                Point = e.GetPosition(target),
            };
        }

        static void target_Loaded(object sender, RoutedEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target == null) return;

            //System.Diagnostics.Debug.WriteLine("Target Loaded");

            target.Unloaded += target_Unloaded;
            target.PreviewMouseLeftButtonDown += target_PreviewMouseLeftButtonDown;
            target.PreviewMouseMove += target_PreviewMouseMove;

            target.PreviewMouseLeftButtonUp += target_PreviewMouseLeftButtonUp;
        }

        static void target_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target == null) return;

            target.ReleaseMouseCapture();
        }

        static void target_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_captures.ContainsKey(sender)) return;

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _captures.Remove(sender);
                return;
            }

            var target = sender as ScrollViewer;
            if (target == null) return;

            var capture = _captures[sender];

            var point = e.GetPosition(target);

            var dy = point.Y - capture.Point.Y;
            if (Math.Abs(dy) > 5)
            {
                target.CaptureMouse();
            }

            target.ScrollToVerticalOffset(capture.VerticalOffset - dy);
        }

        internal class MouseCapture
        {
            public Double VerticalOffset { get; set; }
            public Point Point { get; set; }
        }
    }
}
