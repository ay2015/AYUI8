using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    public interface IAuthorizationFilter
    {

        bool OnAuthorization(IExecutionStrategy filterContext);

    }
}
