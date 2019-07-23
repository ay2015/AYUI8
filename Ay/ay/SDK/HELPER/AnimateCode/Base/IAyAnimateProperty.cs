/*************************************************************************************
  ay2020

   Copyright (C) 2019-2020 AYUI. Software Inc.

   This program is provided to you  at http://www.ayjs.net

  ***********************************************************************************/
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ay.Animate
{
    /// <summary>
    /// AY动画生命周期-属性部分
    /// 版本3.0 
    /// 2019-6-14 09:19:56
    /// </summary>
    public interface IAyAnimateProperty : IAyAnimateLifecycle
    {
        #region 属性
        /// <summary>
        /// 故事版
        /// </summary>
        Storyboard Story { get; set; }
        /// <summary>
        /// 动画的名字
        /// </summary>
        string AnimateName { get; set; }
        /// <summary>
        /// 被动画的元素
        /// </summary>
        FrameworkElement Element { get; set; }
        /// <summary>
        /// 是否初始化
        /// </summary>
        bool IsInitialized { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        bool IsCompleted { get; set; }
        /// <summary>
        /// 是否开始
        /// </summary>
        bool IsBegined { get; set; }
        /// <summary>
        /// 完成后执行
        /// </summary>
        Action Completed { get; set; }

        /// <summary>
        /// 是否执行完自动销毁
        /// </summary>
        bool AutoDestory { get; set; }

        /// <summary>
        /// 频繁触发开始Begin事件，是否每次重新开始,默认不是
        /// </summary>
        bool IsEveryTimeStartOver { get; set; }
        #endregion


    }

}
