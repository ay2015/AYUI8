/*************************************************************************************
  ay2020

   Copyright (C) 2019-2020 AYUI. Software Inc.

   This program is provided to you  at http://www.ayjs.net

   需求1：输入法问题，弹层内存在键盘焦点控件，输入法有时候看不见
   需求2：跟着窗体移动
   需求3：其他区域关闭
   需求4：弹层出入厂动画

  ***********************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace ay.SDK.CONTROLLIB.Primitive
{
    /// <summary>
    ///PlacementTarget 是TextBox的子类的 弹层
    /// </summary>
    public class AyInputPopup : AyPopup
    {
        #region 对外公开

        static AyInputPopup()
        {
            PlacementTargetProperty.OverrideMetadata(
        typeof(AyInputPopup),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPlacementTargetChanged)));
        }

        private static void OnPlacementTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AyInputPopup p)
            {
                Console.WriteLine("asdf");
            }
        }



        #endregion





    }

}
