using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class FilterAttribute : Attribute, IMvcFilter
    {
        /// <summary>
        /// 自己额外加的
        /// 2017-8-17 16:08:26
        /// </summary>
        public FilterScope FilterScope { get; set; } = FilterScope.Action;
        protected FilterAttribute() { }

        public bool AllowMultiple { get; }

        public int Order { get; set; }
    }

    /// <summary>
    /// ay 2017-8-17 16:07:06
    /// 为了解决 同类型filter
    /// </summary>
    public enum FilterScope
    {
        Controller = 100, //执行控制器的，不执行action的
        Action = 101,//Action执行,不执行controller的
        Both = 102 //2个都执行
    }
}
