using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Ay.MvcFramework.Internal.Attributes;
using System.Collections;
using Ay.MvcFramework;

[Serializable]
public class AyPropertyChanged : INotifyPropertyChanged, ICloneable
{
    #region 实现通知

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string _AYID;

    /// <summary>
    /// 标识AYID
    /// </summary>
    public string AYID
    {
        get { return _AYID; }
        set { Set(ref _AYID, value); }
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

    public void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
    {
        var propertyName = ExtractPropertyName(propertyExpression);
        this.OnPropertyChanged(propertyName);
    }

    public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
        if (propertyExpression == null)
        {
            throw new ArgumentNullException("propertyExpression");
        }

        var memberExpression = propertyExpression.Body as MemberExpression;
        if (memberExpression == null)
        {
            throw new ArgumentException("PropertySupport_NotMemberAccessExpression_Exception", "propertyExpression");
        }

        var property = memberExpression.Member as PropertyInfo;
        if (property == null)
        {
            throw new ArgumentException("PropertySupport_ExpressionNotProperty_Exception", "propertyExpression");
        }

        var getMethod = property.GetGetMethod(true);
        if (getMethod.IsStatic)
        {
            throw new ArgumentException("PropertySupport_StaticExpression_Exception", "propertyExpression");
        }

        return memberExpression.Member.Name;
    }
    #endregion

    public object Clone()
    {
        BindingFlags BINDING_FLAGS
        = BindingFlags.Instance | BindingFlags.Public
        | BindingFlags.GetProperty | BindingFlags.SetProperty
        | BindingFlags.GetField | BindingFlags.SetField;
        //先创建一个新实例
        object newObject = Activator.CreateInstance(this.GetType());

        //取出成员数组
        FieldInfo[] fields = newObject.GetType().GetFields(BINDING_FLAGS);

        int i = 0;

        foreach (FieldInfo fi in this.GetType().GetFields())
        {
            //如果成员类型支持Clone;
            Type ICloneType = fi.FieldType.GetInterface("ICloneable", true);

            if (ICloneType != null)
            {
                //从对象取得ICloneable
                ICloneable IClone = (ICloneable)fi.GetValue(this);

                //给成员设置值
                fields[i].SetValue(newObject, IClone.Clone());
            }
            else
            {
                //如果不支持clone，则
                fields[i].SetValue(newObject, fi.GetValue(this));
            }

            //如果它是IEnumerable
            Type IEnumerableType = fi.FieldType.GetInterface("IEnumerable", true);
            if (IEnumerableType != null)
            {
                //取得IEnumerable接口
                IEnumerable IEnum = (IEnumerable)fi.GetValue(this);

                //支持IList和IDictionary interfaces
                Type IListType = fields[i].FieldType.GetInterface("IList", true);
                Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);

                int j = 0;
                if (IListType != null)
                {
                    //取得IList接口
                    IList list = (IList)fields[i].GetValue(newObject);

                    foreach (object obj in IEnum)
                    {
                        //检查是否实现ICloneable
                        ICloneType = obj.GetType().GetInterface("ICloneable", true);

                        if (ICloneType != null)
                        {
                            ICloneable clone = (ICloneable)obj;

                            list[j] = clone.Clone();
                        }

                        j++;
                    }
                }
                else if (IDicType != null)
                {
                    //取得dictionary接口.
                    IDictionary dic = (IDictionary)fields[i].GetValue(newObject);
                    j = 0;
                    foreach (DictionaryEntry de in IEnum)
                    {

                        ICloneType = de.Value.GetType().GetInterface("ICloneable", true);

                        if (ICloneType != null)
                        {
                            ICloneable clone = (ICloneable)de.Value;

                            dic[de.Key] = clone.Clone();
                        }
                        j++;
                    }
                }
            }
            i++;
        }
        System.Reflection.PropertyInfo[] properties = newObject.GetType().GetProperties(BINDING_FLAGS);
        for (int k = 0; k < properties.Length; k++)
        {
            object val = DynaAccessUtils.GetProperty(this, properties[k].Name);
            if (val == null) continue;
            DynaAccessUtils.SetProperty(newObject, properties[k].Name, val);
        }
        return newObject;
    }



}

