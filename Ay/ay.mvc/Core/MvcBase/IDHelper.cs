using System;
using System.Diagnostics.Contracts;

public class IDHelper
{
    /// <summary>
    /// 获取一个全球唯一码GUID字符串,默认全部转换为小写
    /// 作者：AY
    /// 时间:2016-6-20 00:17:40
    /// </summary>
    [Pure]
    public static string GetGuid
    {
        get
        {
            return Guid.NewGuid().ToString().ToLower();
        }
    }
    /// <summary>
    /// 创建一个GUID，没有"-"分隔符的
    /// 作者：AY
    /// 时间:2016-6-20 00:17:40
    /// </summary>
    [Pure]
    public static string GetGuidNoSplit
    {
        get
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
    }
}
