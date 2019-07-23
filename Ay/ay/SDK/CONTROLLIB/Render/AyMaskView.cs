using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
    [TemplatePart(Name = "PART_MaskHost", Type = typeof(ContentPresenter))]
    public class AyMaskView : ContentControl
    {
        static AyMaskView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AyMaskView), new FrameworkPropertyMetadata(typeof(AyMaskView)));
        }
    }

    [TemplatePart(Name = "PART_MaskHost", Type = typeof(ContentPresenter))]
    public class AyMaskTextView: AyMaskView
    {

        static AyMaskTextView()
        {
            TextProperty = DependencyProperty.Register("Text", typeof(object), typeof(AyMaskTextView), new UIPropertyMetadata(null));
            MaskTemplateProperty = DependencyProperty.Register("MaskTemplate", typeof(DataTemplate), typeof(AyMaskTextView), new UIPropertyMetadata(null));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AyMaskTextView), new FrameworkPropertyMetadata(typeof(AyMaskTextView)));
        }
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty MaskTemplateProperty;
        /// <summary>
        /// 模板
        /// </summary>
        public DataTemplate MaskTemplate
        {
            get
            {
                return (DataTemplate)GetValue(MaskTemplateProperty);
            }
            set
            {
                SetValue(MaskTemplateProperty, value);
            }
        }
        public object Text
        {
            get
            {
                return GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

    }
}
