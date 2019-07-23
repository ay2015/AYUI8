using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public AuthorizeAttribute() { }

        public string Roles { get; set; }
        public string Users { get; set; }

        public virtual bool OnAuthorization(IExecutionStrategy filterContext) { return true; }

    }

}
