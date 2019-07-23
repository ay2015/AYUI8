using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ay.Controls
{
    ///提示设置
    public class AyToolTipSetter : Behavior<UIElement>
    {

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AyToolTipSetter), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            if (Tooltip.IsNotNull())
            {
                if (_apUserToolTip1.IsNotNull())
                {
                    _apUserToolTip1 = new ToolTip();
                    _apUserToolTip1.BorderThickness = new Thickness(0);
                    _apUserToolTip1.Background = new SolidColorBrush(Colors.Transparent);
                    _apUserToolTip1.Padding = new Thickness(0);
                    _apUserToolTip1.Placement = PlacementMode.Bottom;
                    _apUserToolTip1.Padding = new Thickness(0, 0, 0, 10);
                    _apUserToolTip1.HorizontalOffset = 0;
                    _apUserToolTip1.VerticalOffset = 0;
                    _apUserToolTip1.Opened += popup_Opened;
                    _apUserToolTip1.PlacementTarget = this.AssociatedObject;
                    _apUserToolTip1.VerticalContentAlignment = VerticalAlignment.Center;
                    _apUserToolTip.Content = Tooltip;
                }
            }
            else
            {
                (this.AssociatedObject as FrameworkElement).ToolTip = apUserToolTip;
            }
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            (this.AssociatedObject as FrameworkElement).ToolTip = null;
            _apUserToolTip = null;
            at = null;
            _tb = null;
        }

        public AyTooltip Tooltip
        {
            get { return (AyTooltip)GetValue(TooltipProperty); }
            set { SetValue(TooltipProperty, value); }
        }
        public static readonly DependencyProperty TooltipProperty =
            DependencyProperty.Register("Tooltip", typeof(AyTooltip), typeof(AyTooltip), new PropertyMetadata(null));
        private ToolTip _apUserToolTip1;



        #region 默认样式
        ToolTip _apUserToolTip = null;
        AyTooltip at = null;
        TextBlock _tb = null;
        /// <summary>
        /// 用户提示
        /// </summary>
        public ToolTip apUserToolTip
        {
            get
            {
                if (_apUserToolTip.IsNull())
                {
                    CreatePopupEx();
                }
                return _apUserToolTip;
            }
            set { _apUserToolTip = value; }
        }

        private void CreatePopupEx()
        {
            if (_apUserToolTip.IsNull())
            {
                _apUserToolTip = new ToolTip();
                _apUserToolTip.BorderThickness = new Thickness(0);
                _apUserToolTip.Background = new SolidColorBrush(Colors.Transparent);
                _apUserToolTip.Padding = new Thickness(0);
                _apUserToolTip.Placement = PlacementMode.Left;
                _apUserToolTip.Padding = new Thickness(0, 0, 10, 0);
                _apUserToolTip.HorizontalOffset = 0;
                _apUserToolTip.VerticalOffset = 0;
                _apUserToolTip.Opened += popup_Opened;
                _apUserToolTip.PlacementTarget = this.AssociatedObject;
                _apUserToolTip.VerticalContentAlignment = VerticalAlignment.Center;

                if (at.IsNull())
                {
                    at = new AyTooltip();
                    at.RadiusX = 3;
                    at.RadiusY = 3;
                    at.BorderBrush = SolidColorBrushConverter.From16JinZhi("#333333");
                    at.MaxWidth = 150;
                    at.Foreground = SolidColorBrushConverter.From16JinZhi("#333333");
                    at.Background = SolidColorBrushConverter.From16JinZhi("#FFFFFF");
                    at.Placement = Dock.Right;
                    _tb = new TextBlock();
                    //_tb.Margin = new Thickness(2,2,0,2);
                    _tb.TextWrapping = TextWrapping.Wrap;
                    _tb.FontSize = 12;
                    _tb.SetBinding(TextBlock.ForegroundProperty, new Binding { Path = new PropertyPath("Foreground"), Source = at, Mode = BindingMode.TwoWay });
                    _tb.SetBinding(TextBlock.TextProperty, new Binding { Path = new PropertyPath("Text"), Source = this, Mode = BindingMode.TwoWay });
                    at.TooltipContent = _tb;
                    _apUserToolTip.Content = at;


                }
            }
        }


        void popup_Opened(object sender, EventArgs e)
        {
            var p = sender as ToolTip;
            if (p != null)
            {
                UpdateToolTipStyle();
            }
        }

        internal void UpdateToolTipStyle()
        {
            Point relativeLocation = at.TranslatePoint(new Point(0, 0), this.AssociatedObject);
            if (relativeLocation.X < 0)
            {
                at.Placement = Dock.Right;
                apUserToolTip.Padding = new Thickness(0, 0, 10, 0);
            }
            else if (relativeLocation.X > 0)
            {
                at.Placement = Dock.Left;
                apUserToolTip.Padding = new Thickness(10, 0, 0, 0);
            }
            //if (relativeLocation.Y > 0)
            //{
            //    at.Placement = Dock.Top;
            //    apUserToolTip.Padding = new Thickness(0, 10, 0, 0);
            //}
            //else if (relativeLocation.Y < 0)
            //{
            //    at.Placement = Dock.Bottom;
            //    apUserToolTip.Padding = new Thickness(0, 0, 0, 10);
            //}
        }

        #endregion
    }

}


