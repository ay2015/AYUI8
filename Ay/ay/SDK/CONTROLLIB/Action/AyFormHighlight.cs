using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Shapes;
using i = System.Windows.Interactivity;

namespace ay.Controls
{

    [DefaultTrigger(typeof(ButtonBase), typeof(i.EventTrigger), new object[] { "Click" })]
    [DefaultTrigger(typeof(Shape), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    [DefaultTrigger(typeof(UIElement), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    public class AyFormHighlight : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// 需要验证的表单
        /// </summary>
        public object Form
        {
            get { return (object)GetValue(FormProperty); }
            set { SetValue(FormProperty, value); }
        }

        public static readonly DependencyProperty FormProperty =
            DependencyProperty.Register("Form", typeof(object), typeof(AyFormHighlight), new PropertyMetadata(null, OnSubmitFormChanged));


        /// <summary>
        /// 是否释放表单，因为可能你用了AyFormSubmit，那里已经释放表单了，没必要继续释放了
        /// </summary>
        public bool IsReleaseForm
        {
            get { return (bool)GetValue(IsReleaseFormProperty); }
            set { SetValue(IsReleaseFormProperty, value); }
        }

        public static readonly DependencyProperty IsReleaseFormProperty =
            DependencyProperty.Register("IsReleaseForm", typeof(bool), typeof(AyFormHighlight), new PropertyMetadata(false));



        private static void OnSubmitFormChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _2 = d as AyFormHighlight;
            if (_2.IsNotNull())
            {
                if (_2.IsReleaseForm)
                {
                    var form = e.NewValue as FrameworkElement;
                    form.Unloaded -= _2.Form_Unloaded;
                    form.Unloaded += _2.Form_Unloaded;

                }
            }
        }


        private void Form_Unloaded(object sender, RoutedEventArgs e)
        {
            var form = sender as FrameworkElement;
            if (AyForm.Forms.ContainsKey(form))
            {
                AyForm.Forms.Remove(form);
            }
        }

        protected override void Invoke(object parameter)
        {
            if (Form.IsNotNull())
            {
                var _curForm = Form as FrameworkElement;
                var _1 = AyForm.Forms[_curForm];
                foreach (var item in _1)
                {
                    var _2 = item as IAyHighlight;
                    if (_2.IsNotNull())
                    {
                        _2.HighlightElement();
                    }
                }
            }
        }
    }




}


