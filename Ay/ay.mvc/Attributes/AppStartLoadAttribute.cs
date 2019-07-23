using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    /// <summary>
    /// 运行绑定的时候执行  授权类型的过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class AppStartLoadAttribute : Attribute
    {

    }
}
