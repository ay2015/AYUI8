using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Shapes;
using i = System.Windows.Interactivity;

namespace ay.Controls
{

    [DefaultTrigger(typeof(ButtonBase), typeof(i.EventTrigger), new object[] { "Click" })]
    [DefaultTrigger(typeof(Shape), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    [DefaultTrigger(typeof(UIElement), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    public class AyFormSubmit : TriggerAction<FrameworkElement>
    {
        public delegate void Handler(object sender, RoutedEventArgs e);
        public event Handler Submit;

        /// <summary>
        /// 需要验证的表单
        /// </summary>
        public object Form
        {
            get { return (object)GetValue(FormProperty); }
            set { SetValue(FormProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Form.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormProperty =
            DependencyProperty.Register("Form", typeof(object), typeof(AyFormSubmit), new PropertyMetadata(null, OnSubmitFormChanged));

        private static void OnSubmitFormChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _2 = d as AyFormSubmit;
            if (_2.IsNotNull())
            {
                var form = e.NewValue as FrameworkElement;
                if (form.IsNotNull())
                {
                    form.Unloaded -= _2.Form_Unloaded;
                    form.Unloaded += _2.Form_Unloaded;
                }
            }
        }



        /// <summary>
        /// 滚动条
        /// </summary>
        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }

        public static readonly DependencyProperty ScrollViewerProperty =
            DependencyProperty.Register("ScrollViewer", typeof(ScrollViewer), typeof(AyFormSubmit), new PropertyMetadata(null));


        private void Form_Unloaded(object sender, RoutedEventArgs e)
        {
            var form = sender as FrameworkElement;
            if (AyForm.Forms.ContainsKey(form))
            {
                AyForm.Forms.Remove(form);
            }
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        public ICommand SubmitCommand
        {
            get { return (ICommand)GetValue(SubmitCommandProperty); }
            set { SetValue(SubmitCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubmitCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubmitCommandProperty =
            DependencyProperty.Register("SubmitCommand", typeof(ICommand), typeof(AyFormSubmit), new PropertyMetadata(null));



        private AyTimeSetTimeout _ShowErrorTime;
        /// <summary>
        /// 无注释
        /// </summary>
        public AyTimeSetTimeout ShowErrorTime
        {
            get
            {
                return _ShowErrorTime;
            }
        }

        IAyValidate templi = null;
        protected override void Invoke(object parameter)
        {
            if (Form.IsNotNull())
            {
                if (ScrollViewer != null)
                {
                    var _curForm = Form as FrameworkElement;
                    var _1 = AyForm.Forms[_curForm];
                    bool hasFalse = false;
                    foreach (var item in _1)
                    {
                        var _2 = item as IAyValidate;
                        if (_2.IsNotNull())
                        {
                            var _3 = _2.ValidateButNotShowError();
                            if (!_3)
                            {
                                hasFalse = true;
                                var currentScrollPosition = ScrollViewer.VerticalOffset;
                                var point = new Point(0, currentScrollPosition);

                                // 计算出目标位置并滚动
                                var targetPosition = item.TransformToVisual(ScrollViewer).Transform(point);
                                ScrollViewer.ScrollToVerticalOffset(targetPosition.Y);
                                templi = _2;
                                if (_ShowErrorTime == null)
                                {
                                    _ShowErrorTime = new AyTimeSetTimeout(100, () =>
                                    {
                                        templi.ShowError();
                                    });
                                }
                                _ShowErrorTime.Start();
                                break;
                            }
                        }
                    }
                    if (hasFalse)
                    {
                        return;
                    }
                    if (Submit != null)
                    {
                        Submit(true, new RoutedEventArgs() { });
                    }
                    if (SubmitCommand != null)
                    {
                        SubmitCommand.Execute(true);
                    }
                }
                else
                {
                    var _curForm = Form as FrameworkElement;
                    var _1 = AyForm.Forms[_curForm];
                    bool hasFalse = false;
                    foreach (var item in _1)
                    {
                        var _2 = item as IAyValidate;
                        if (_2.IsNotNull())
                        {
                            var _3 = _2.Validate();
                            if (!_3)
                            {
                                hasFalse = true;
                                break;
                            }
                        }
                    }
                    if (hasFalse)
                    {
                        return;
                    }
                    if (Submit != null)
                    {
                        Submit(true, new RoutedEventArgs() { });
                    }
                    if (SubmitCommand != null)
                    {
                        SubmitCommand.Execute(true);
                    }
                }
            }

        }


    }




}


