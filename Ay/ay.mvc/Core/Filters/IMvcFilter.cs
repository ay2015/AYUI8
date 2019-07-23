using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    //     定义用于指定筛选器顺序以及是否允许多个筛选器的成员。
    public interface IMvcFilter
    {
        bool AllowMultiple { get; }
        //
        // 摘要:
        //     在类中实现时，获取筛选器顺序。
        //AY 2017-8-11 09:49:23  暂时没用
        // 返回结果:
        //     筛选器顺序。
        int Order { get; }
    }
}
