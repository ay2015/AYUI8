using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Ay.MvcFramework.Internal.Attributes;

namespace Ay.MvcFramework
{
    public class ControllerBase : AyPropertyChanged
    {
        public ControllerBase()
        {

        }
        #region 拓展 AY 2017-8-11 16:01:27
        private dynamic _ViewBag = new System.Dynamic.ExpandoObject();

        /// <summary>
        /// 未填写
        /// </summary>
        public dynamic ViewBag
        {
            get { return _ViewBag; }
            set { Set(ref _ViewBag, value); }
        }
        #endregion


    }
}
