/*************************************************************************************
  ay2020

   Copyright (C) 2019-2020 AYUI. Software Inc.

   This program is provided to you  at http://www.ayjs.net

  ***********************************************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ay.Animate
{
    /// <summary>
    /// Ay系统设计的动画声明周期
    /// </summary>
    public interface IAyAnimateLifecycle
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();
        /// <summary>
        /// 开始
        /// </summary>
        void Begin();
        /// <summary>
        /// 暂停
        /// </summary>
        void Pauze();
        /// <summary>
        /// 继续
        /// </summary>
        void Resume();
        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
        /// <summary>
        /// 跳到完成
        /// </summary>
        void SkipToFill();

        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy();

    }

}
