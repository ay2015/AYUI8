using ay.Controls.Validate;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ay.Controls
{
    /// <summary>
    /// 提供服务
    /// </summary>
    public static class AyForm
    {
        [ThreadStatic]
        private static AyFormCollection _forms;

        internal static AyFormCollection Forms
        {
            get
            {
                if (_forms == null)
                {
                    _forms = new AyFormCollection();
                }
                return _forms;
            }
        }

        public static bool GetIsAyForm(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAyFormProperty);
        }

        public static void SetIsAyForm(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAyFormProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsAyForm.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAyFormProperty =
            DependencyProperty.RegisterAttached("IsAyForm", typeof(bool), typeof(AyForm), new FrameworkPropertyMetadata(false, OnIsAyFormChanged));

        private static void OnIsAyFormChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Panel box = d as Panel;
            if (box.IsNotNull())
            {
                bool _o = (bool)(e.OldValue);
                bool _s = (bool)e.NewValue;
                if (_o)
                {
                    KeyboardNavigation.SetTabNavigation(box, KeyboardNavigationMode.None);
                }
                if (_s)
                {
                    KeyboardNavigation.SetTabNavigation(box, KeyboardNavigationMode.Cycle);
                    SetEnterKeyIsTab(box, true);
                }
            }

        }

        public static bool GetEnterKeyIsTab(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnterKeyIsTabProperty);
        }

        public static void SetEnterKeyIsTab(DependencyObject obj, bool value)
        {
            obj.SetValue(EnterKeyIsTabProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnterKeyIsTab.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnterKeyIsTabProperty =
            DependencyProperty.RegisterAttached("EnterKeyIsTab", typeof(bool), typeof(AyForm), new FrameworkPropertyMetadata(false, OnEnterKeyIsTabChanged));

        private static void OnEnterKeyIsTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Panel box = d as Panel;
            if (box.IsNotNull())
            {
                bool _o = (bool)(e.OldValue);
                bool _s = (bool)e.NewValue;
                if (_o)
                {
                    box.KeyDown -= HandleEnterIsTabChanged;
                }
                if (_s)
                {
                    box.KeyDown += HandleEnterIsTabChanged;
                }
            }
        }
        /// <summary>
        /// AY 2016-8-3 19:53:46 增加
        /// 用于让表单支持，回车等于tab的效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void HandleEnterIsTabChanged(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                    UIElement focusElement = Keyboard.FocusedElement as UIElement;
                    if (focusElement != null)
                    {
                        focusElement.MoveFocus(request);
                    }
                    e.Handled = true;
                }
            }
            catch
            {

            }
        }


        /// <summary>
        /// 生日 2016-10-24 07:00:00
        /// 用于绑定提交的表单对象，建议是个容器
        /// 送礼：2016-10-27 05:37:53
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object GetForm(DependencyObject obj)
        {
            return (object)obj.GetValue(FormProperty);
        }

        public static void SetForm(DependencyObject obj, object value)
        {
            obj.SetValue(FormProperty, value);
        }

    
        public static readonly DependencyProperty FormProperty =
            DependencyProperty.RegisterAttached("Form", typeof(object), typeof(AyForm), new PropertyMetadata(null, OnFormChanged));

        private static void OnFormChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!WpfTreeHelper.IsInDesignMode)
            {
                FrameworkElement _1 = d as FrameworkElement;
                if (_1.IsNotNull())
                {
                    var oldForm = e.OldValue as FrameworkElement;//表单
                    if (oldForm!=null)
                    {
                        if (Forms.ContainsKey(oldForm))
                        {
                            var _2 = Forms[oldForm];
                            _2.Remove(_1);
                            if (_2.Count == 0)
                            {
                                Forms.Remove(oldForm);
                            }
                        }
                    }
                    var newForm = e.NewValue as FrameworkElement;
                    if (newForm != null)
                    {
                        if (!Forms.ContainsKey(newForm))
                        {
                            Forms[newForm] = new System.Collections.Generic.List<FrameworkElement>();
                        }
                        if (Forms[newForm].IndexOf(_1) == -1)
                        {
                            Forms[newForm].Add(_1);
                        }
                    }
                }
            }
        }

    }
}
