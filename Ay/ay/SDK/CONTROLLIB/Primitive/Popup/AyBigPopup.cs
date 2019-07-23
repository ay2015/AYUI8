/*************************************************************************************
  ay2020

   Copyright (C) 2019-2020 AYUI. Software Inc.

   This program is provided to you  at http://www.ayjs.net

   √ popup不允许超过win的三分之二的高度，使用AyBigPopup弹层解决，比如全屏菜单
  ***********************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace ay.SDK.CONTROLLIB.Primitive
{
    /// <summary>
    ///大面积 popup
    /// </summary>
    public class AyBigPopup : AyPopup
    {

        public AyBigPopup() : base()
        {
            if (!WpfTreeHelper.IsInDesignMode)
            {
                Closed += AyBigPopup_Closed;
            }
        }

        private void AyBigPopup_Closed(object sender, EventArgs e)
        {
            FixMaxWidthAndHeightTimer.IsLocked = true;
        }

        /// <summary>
        /// 参照的宽高，一般为顶级容器的宽高
        /// </summary>
        public UIElement ReferUIElement
        {
            get { return (UIElement)GetValue(ReferUIElementProperty); }
            set { SetValue(ReferUIElementProperty, value); }
        }

        public static readonly DependencyProperty ReferUIElementProperty =
            DependencyProperty.Register("ReferUIElement", typeof(UIElement), typeof(AyBigPopup), new FrameworkPropertyMetadata(null, OnReferUIElementChanged));

        private static void OnReferUIElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AyBigPopup _ownerclass)
            {
                Window w = Window.GetWindow(_ownerclass.PlacementTarget);
                if (null != w)
                {
                    w.SizeChanged -= _ownerclass.W_SizeChanged;
                    w.SizeChanged += _ownerclass.W_SizeChanged;
                }
            }
        }

        private void W_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FixMaxWidthAndHeight();
        }
        /// <summary>
        /// 显示弹层
        /// </summary>
        public override void ShowPopup()
        {
            IsOpen = true;
        }
        /// <summary>
        /// 隐藏弹层
        /// </summary>
        public override void HidePopup()
        {
            if (!FixMaxWidthAndHeightTimer.IsLocked)
            {
                if (IsOpen == false)
                {
                    FixMaxWidthAndHeight();
                }
            }
        }


        #region 最大70%高度区域问题

        private AyTimeDelayAutoUnLock _FixMaxWidthAndHeightTimer;
        /// <summary>
        /// 延迟后解锁
        /// </summary>
        public AyTimeDelayAutoUnLock FixMaxWidthAndHeightTimer
        {
            get
            {
                if (_FixMaxWidthAndHeightTimer == null)
                {
                    _FixMaxWidthAndHeightTimer = new AyTimeDelayAutoUnLock();
                }
                return _FixMaxWidthAndHeightTimer;
            }
        }

        public void FixMaxWidthAndHeight()
        {
            DependencyObject parent = Child;
            do
            {
                parent = VisualTreeHelper.GetParent(parent);

                if (parent != null && parent.ToString() == "System.Windows.Controls.Primitives.PopupRoot")
                {
                    var element = parent as FrameworkElement;

                    element.Width = ReferUIElement.RenderSize.Width;
                    element.Height = ReferUIElement.RenderSize.Height;

                    break;
                }
            }
            while (parent != null);
            Width = ReferUIElement.RenderSize.Width;
            Height = ReferUIElement.RenderSize.Height;
        }
        #endregion
    }

}
