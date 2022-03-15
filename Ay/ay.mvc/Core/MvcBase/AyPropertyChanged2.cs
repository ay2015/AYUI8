using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Ay.MvcFramework.Internal.Attributes;
using System.Collections;
using Ay.MvcFramework;

[Serializable]
public class AyPropertyChanged2 : INotifyPropertyChanged
{
    #region 实现通知

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// </summary>
    /// 
    /// </summary>
    /// <typeparam name="T">属性类型</typeparam>
    /// <param name="storage">被设置的属性</param>
    /// <param name="isCheckEquals">是否检查属性相等</param>
    /// <param name="value">将要设置的属性值</param>
    /// <returns></returns>
    public bool Set<T>(ref T storage, T value, bool isCheckEquals = true, [CallerMemberName] string propertyName = null)
    {
        if (isCheckEquals)
            if (object.Equals(storage, value)) { return false; }
        storage = value;
        this.OnPropertyChanged2(propertyName);
        return true;
    }
     

    private void OnPropertyChanged2([CallerMemberName] string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public void OnPropertyChanged(params string[] propertyNames)
    {
        if (propertyNames == null) throw new ArgumentNullException("propertyNames");

        foreach (var name in propertyNames)
        {
            OnPropertyChanged(name);
        }
    }




    #endregion




}

