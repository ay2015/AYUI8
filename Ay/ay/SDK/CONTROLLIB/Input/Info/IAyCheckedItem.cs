
/// <summary>
/// AY
/// 生日：2016-12-19 16:21:05
/// 提供一组check的行为特性
/// </summary>
public interface IAyCheckedItem
{
    /// <summary>
    /// 是否选中
    /// </summary>
    bool IsChecked { get; set; }
    /// <summary>
    /// 显示的内容
    /// </summary>
    string ItemText { get; set; }
    /// <summary>
    /// 隐藏选中的值
    /// </summary>
    string ItemValue { get; set; }

}

