using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    /// <summary>
    /// 错误统一处理 对外
    /// AY 2017-9-21 17:22:37
    /// </summary>
    public interface IExceptionFilter
    {
        void OnException(ExceptionContext contextfilterContext);
    }
}
