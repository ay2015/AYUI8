
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ay.Controls.Validate
{
    /// <summary>
    /// key 是form目标对象  value是一组实现了IAyValidate接口的对象
    /// 生日 2016-10-24 06:05:19
    /// </summary>
    internal class AyFormCollection : Dictionary<FrameworkElement, List<FrameworkElement>>
    {

    }
}
