using System;
using System.Windows;
using System.Windows.Automation.Peers;

namespace ay.Controls
{
    public class AyTextBox : AyTextBoxBase, IAyControl, IControlPlaceholder
    {

        public new string ControlID { get { return ControlGUID.AyTextBox; } }

        public static readonly DependencyProperty IsKeepPlaceholderProperty;

        public static readonly DependencyProperty PlaceholderProperty;

        public static readonly DependencyProperty PlaceholderTemplateProperty;
        /// <summary>
        /// 当获得键盘焦点时候，是否保持水印
        /// </summary>
		public bool IsKeepPlaceholder
        {
            get
            {
                return (bool)GetValue(IsKeepPlaceholderProperty);
            }
            set
            {
                SetValue(IsKeepPlaceholderProperty, value);
            }
        }
        /// <summary>
        /// 水印
        /// </summary>
		public object Placeholder
        {
            get
            {
                return GetValue(PlaceholderProperty);
            }
            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }
        /// <summary>
        /// 水印模板
        /// </summary>
		public DataTemplate PlaceholderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(PlaceholderTemplateProperty);
            }
            set
            {
                SetValue(PlaceholderTemplateProperty, value);
            }
        }
        /// <summary>
        /// 左侧内容
        /// </summary>
        public object LeftContent
        {
            get { return (object)GetValue(LeftContentProperty); }
            set { SetValue(LeftContentProperty, value); }
        }

        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.Register("LeftContent", typeof(object), typeof(AyTextBox), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 右侧内容
        /// </summary>
        public object RightContent
        {
            get { return (object)GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(AyTextBox), new FrameworkPropertyMetadata(null));



        static AyTextBox()
        {
            IsKeepPlaceholderProperty = DependencyProperty.Register("IsKeepPlaceholder", typeof(bool), typeof(AyTextBox), new UIPropertyMetadata(true));
            PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(object), typeof(AyTextBox), new UIPropertyMetadata(null));
            PlaceholderTemplateProperty = DependencyProperty.Register("PlaceholderTemplate", typeof(DataTemplate), typeof(AyTextBox), new UIPropertyMetadata(null));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AyTextBox), new FrameworkPropertyMetadata(typeof(AyTextBox)));
        }

        public AyTextBox()
        {

        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ay.UIAutomation.TextBoxAutomationPeer(this);
        }
        public override bool Validate()
        {
            throw new System.NotImplementedException();
        }

        public override void HighlightElement()
        {
            throw new System.NotImplementedException();
        }
        public override bool ValidateButNotShowError()
        {
            throw new System.NotImplementedException();
        }

        public override void ShowError()
        {
            throw new System.NotImplementedException();
        }
    }
}