using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    public interface IActionFilter
    {
        /// <summary>
        /// 执行之后
        /// </summary>
        /// <param name="filterContext"></param>
        void OnActionExecuted(IExecutionStrategy filterContext);

        /// <summary>
        /// 执行之前
        /// </summary>
        /// <param name="filterContext"></param>
        void OnActionExecuting(IExecutionStrategy filterContext);
    }
}
