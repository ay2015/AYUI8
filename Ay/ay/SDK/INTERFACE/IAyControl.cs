using System;

/// <summary>
/// Ay控件共用属性和方法
/// </summary>
public interface IAyControl
{
    /// <summary>
    /// 控件唯一ID
    /// </summary>
    string ControlID { get; }
}
/// <summary>
/// AY控件高亮接口
/// </summary>
public interface IAyHighlight
{
    void HighlightElement();
}
/// <summary>
/// AY控件验证接口
/// </summary>
public interface IAyValidate
{
    bool Validate();
    bool ValidateButNotShowError();
    void ShowError();
}
public interface IControlPlaceholder
{
    /// <summary>
    /// 水印
    /// </summary>
    object Placeholder { get; set; }
    /// <summary>
    /// 水印模板
    /// </summary>
    System.Windows.DataTemplate PlaceholderTemplate { get; set; }
    /// <summary>
    /// 当获得键盘焦点时候，是否保持水印
    /// </summary>
    bool IsKeepPlaceholder { get; set; }


}


