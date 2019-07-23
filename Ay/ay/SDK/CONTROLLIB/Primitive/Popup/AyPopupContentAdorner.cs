/*************************************************************************************
  ay2020

   Copyright (C) 2019-2020 AYUI. Software Inc.

   This program is provided to you  at http://www.ayjs.net

  ***********************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace ay.Controls
{
    /// <summary>
    ///弹层
    /// </summary>
    public class AyPopupContentAdorner : ContentControl,IAyControl
    {
        public string ControlID { get { return ay.Controls.ControlGUID.AyContentPopup; } }


        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(AyPopupContentAdorner), new PropertyMetadata(false));




    }

}
