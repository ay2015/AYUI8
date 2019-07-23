

using ay.Controls;
using System;

public static class AyTableViewService
{
    public static RowOldNewValue GetRowObject(string ROWID)
    {
        if (AYUI.Session.ContainsKey(ROWID))
        {
            return AYUI.Session[ROWID] as RowOldNewValue;
        }
        else
        {
            return null;
        }
    }
    //public static void SetRowCompleted(this RowOldNewValue rowvalue)
    //{
    //    //rowvalue.RowActionType = 1;
    //    AYUI.Session[rowvalue.RowId] = rowvalue;
    //}

    public static bool HasChanged(this RowOldNewValue value)
    {
        if (value == null)
        {
            return false;
        }
        var obj1 = value.OldValue;
        var obj2 = value.NewValue;
        Type type1 = obj1.GetType();
        Type type2 = obj2.GetType();

        System.Reflection.PropertyInfo[] properties1 = type1.GetProperties();
        System.Reflection.PropertyInfo[] properties2 = type2.GetProperties();

        bool IsMatch = true;
        for (int i = 0; i < properties1.Length; i++)
        {
            string s = properties1[i].Name;
            if (s == "Selected" || s == "IsMouseOver" || s == "IsExpanded" || s == "IsEnabled" || s == "IsChecked" || s == "IsCheckedThree"
                || s == "AYID" || s == "IsEditRow" || s == "IsAppendRow"
                )
            {
                continue;
            }
            var _s1 = properties1[i].GetValue(obj1, null);
            var _s2 = properties2[i].GetValue(obj2, null);
            if (_s1.ToObjectString() != _s2.ToObjectString())
            {
                IsMatch = false;
                break;
            }
        }

        return !IsMatch;
    }

    /// <summary>  
    /// 模型赋值  
    /// </summary>  
    /// <param name="target">目标</param>  
    /// <param name="source">数据源</param>  
    public static void CopyModel(object target, object source)
    {
        Type type1 = target.GetType();
        Type type2 = source.GetType();
        foreach (var mi in type2.GetProperties())
        {
            string s = mi.Name;
            if (s == "Selected" || s == "IsMouseOver" || s == "IsExpanded" || s == "IsEnabled" || s == "IsChecked" || s == "IsCheckedThree"
               || s == "AYID" || s == "IsEditRow" || s == "IsAppendRow"
                )
            {
                continue;
            }
            var des = type1.GetProperty(mi.Name);
            if (des != null)
            {
                try
                {
                    des.SetValue(target, mi.GetValue(source, null), null);
                }
                catch
                { }
            }
        }
    }

    /// <summary>
    /// 验证行是否通过
    /// </summary>
    /// <param name="rowvalue"></param>
    /// <param name="passed">验证通过</param>
    /// <param name="failded">验证失败</param>
    public static void Validate(this RowOldNewValue rowvalue, Action passed, Action failded = null)
    {
        rowvalue.ValidateFail = failded;
        rowvalue.ValidatePassed = passed;
        //rowvalue.RowActionType = 2;
        //AYUI.Session[rowvalue.RowId] = rowvalue;

    }

}
/// <summary>
/// 2017-12-13 16:10:28
/// AY 存储新旧对象
/// </summary>
public class RowOldNewValue : AyPropertyChanged
{

    public AyTableViewStatuss RowStatus { get; set; }
    public string RowId { get; set; }

    public Action ValidateFail { get; set; }

    public Action ValidatePassed { get; set; }

    private object _OldValue;

    /// <summary>
    /// 旧值
    /// </summary>
    public object OldValue
    {
        get { return _OldValue; }
        set { Set(ref _OldValue, value); }
    }

    private object _NewValue;

    /// <summary>
    /// 新值
    /// </summary>
    public object NewValue
    {
        get { return _NewValue; }
        set { Set(ref _NewValue, value); }
    }



    //private short _RowActionType=1;

    ///// <summary>
    ///// 1 代表完成，直接关闭行编辑   2代表验证
    ///// </summary>
    //internal short RowActionType
    //{
    //    get { return _RowActionType; }
    //    set { Set(ref _RowActionType, value); }
    //}


}

