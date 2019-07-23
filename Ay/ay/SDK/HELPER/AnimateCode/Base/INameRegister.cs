/*************************************************************************************
  ay2020

   Copyright (C) 2019-2020 AYUI. Software Inc.

   This program is provided to you  at http://www.ayjs.net

  ***********************************************************************************/

namespace ay.Animate
{
    /// <summary>
    /// 给控件注册名称
    /// </summary>
    public interface INameRegister
    {
        /// <summary>
        /// 基本上注册名字都是跟的window上
        /// 请在构造函数实例化
        /// </summary>
        NameRegister Win { get;  }
    }


}
