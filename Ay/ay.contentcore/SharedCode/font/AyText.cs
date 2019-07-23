using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ay.contentcore
{
    /// <summary>
    /// 为了逃避 全局TextBlock设置了字体统一，会影响button等控件，在外层设置字体大小的问题
    /// </summary>
    public class AyText:TextBlock
    {
    }
    /// <summary>
    /// 单独文本，不受全局字体和字色影响
    /// </summary>
    public class AyTextOnly : TextBlock
    {
    }
}
