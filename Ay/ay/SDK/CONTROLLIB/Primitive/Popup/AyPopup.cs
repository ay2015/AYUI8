/*************************************************************************************
  ay2020

   Copyright (C) 2019-2020 AYUI. Software Inc.

   This program is provided to you  at http://www.ayjs.net

   ？需求1：输入法问题，弹层内存在键盘焦点控件，输入法有时候看不见，在4.0以后能用，不建议用网络上的修复，会导致文本框焦点弹窗控件，然后单击窗体，是关闭不了
   √ 需求2：解决 跟着窗体移动，窗体最大化还原定位，但是不会跟着 定位控件 的移动而动，只会根据窗体的locationchanged
   √ 需求3：解决 置顶其他应用，单击其他区域自动关闭
      需求4：弹层出入场动画
   √ 需求5：popup不允许超过win的三分之二的高度，使用AyBigPopup弹层解决，比如全屏菜单
  ***********************************************************************************/

using System;
using System.Collections.Generic;
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
    ///弹层
    /// </summary>
    public class AyPopup : Popup, IAyControl
    {
        public string ControlID { get { return ay.Controls.ControlGUID.AyPopup; } }

        #region 放弃修复

        //[DllImport("user32.dll")]
        //static extern IntPtr SetActiveWindow(IntPtr hWnd);
        //static AyPopup()
        //{
        //    EventManager.RegisterClassHandler(typeof(AyPopup), Popup.PreviewGotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnPreviewGotKeyboardFocus), true);
        //}

        //[DllImport("User32.dll")]
        //private static extern IntPtr SetFocus(IntPtr hWnd);



        //private static void OnPreviewGotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    var textBox = e.NewFocus as TextBoxBase;
        //    if (textBox != null)
        //    {
        //        var hwndSource = PresentationSource.FromVisual(textBox) as HwndSource;
        //        if (hwndSource != null)
        //        {
        //            SetFocus(hwndSource.Handle);
        //        }
        //    }
        //    //var source = (HwndSource)PresentationSource.FromVisual(Child);
        //    //if (source != null)
        //    //{
        //    //    SetFocus(source.Handle);
        //    //}
        //}       
        #endregion

        //#region 窗体工作
        //public Dictionary<Guid, Action> WindowsLocationChanged = new Dictionary<Guid, Action>();
        //public Dictionary<Guid, Action> WindowsSizeChanged = new Dictionary<Guid, Action>();
        //public Dictionary<Guid, Action> WindowsStateChanged = new Dictionary<Guid, Action>();
        //#endregion

        #region 初始化工作


        public AyPopup(UIElement frame)
        {
            if (!WpfTreeHelper.IsInDesignMode)
            {
                PlacementTarget= frame;
                RegisterMove();
                Unloaded += AyPopup_Unloaded;
            }
            AllowsTransparency = true;
            PopupAnimation = PopupAnimation.None;
        }
        public AyPopup()
        {
            if (!WpfTreeHelper.IsInDesignMode)
            {
                Loaded += AyPopup_Loaded;
                Unloaded += AyPopup_Unloaded;
            }
            AllowsTransparency = true;
            PopupAnimation = PopupAnimation.None;
        }

        private void AyPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= AyPopup_Unloaded;
            Window w = Window.GetWindow(PlacementTarget);
            if (null != w)
            {
                w.LocationChanged -= W_LocationChanged;
                w.StateChanged -= W_StateChanged;
            }
        }

        private void AyPopup_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyPopup_Loaded;
            //注册移动
            RegisterMove();
        }
        #endregion

        /// <summary>
        /// 目标所在的窗体移动，popup跟着移动
        /// </summary>
        public void RegisterMove()
        {
            Window w = Window.GetWindow(PlacementTarget);
            if (null != w)
            {
                w.LocationChanged -= W_LocationChanged;
                w.LocationChanged += W_LocationChanged;
                w.StateChanged -= W_StateChanged;
                w.StateChanged += W_StateChanged;
            }
        }

        private void W_LocationChanged(object sender, EventArgs e)
        {
            FixedMove();
        }


        private AyTimeSetTimeout _ThisTimer;
        /// <summary>
        /// 延迟执行计时器
        /// </summary>
        public AyTimeSetTimeout ThisTimer
        {
            get
            {
                if (_ThisTimer == null)
                {
                    _ThisTimer = new AyTimeSetTimeout(FixedMove);
                }
                return _ThisTimer;
            }
        }



        private void W_StateChanged(object sender, EventArgs e)
        {
            ThisTimer.Start();
        }

        private void FixedMove()
        {
            var offset = this.HorizontalOffset;
            this.HorizontalOffset = offset + 1;
            this.HorizontalOffset = offset;
        }

        #region 显示与隐藏
        public virtual void ShowPopup()
        {
            Child.Opacity = 0;
            IsOpen = true;
            //y轴变化

        }
        public virtual void HidePopup()
        {
            IsOpen = false;
        }
        #endregion

    }

}
