using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class ActionFilterAttribute : FilterAttribute, IActionFilter
    {
        protected ActionFilterAttribute() { }

        public virtual void OnActionExecuted(IExecutionStrategy filterContext)
        {

        }
        public virtual void OnActionExecuting(IExecutionStrategy filterContext) { }
    }

}
