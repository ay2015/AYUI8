using ay.mvc.CommonConvert;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public class SelectListItemNoNotify
{
    public string ID { get; set; }
    private string _value;
    public string Value
    {
        get { return _value; }
        set
        {
            _value = value;
        }
    }
    private string _text;
    public string Text
    {
        get { return _text; }
        set
        {
            _text = value;
        }
    }

    public string field { get; set; }

    public string systemname { get; set; }

    public string ext { get; set; }
}


public static class EnumHelper
{
    /// <summary>
    /// 对象深拷贝
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="obj">Object</param>
    /// <returns>Object</returns>
    public static T DeepCopy<T>(T obj)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException(string.Format("该类型:{0}不支持序列化", typeof(T).FullName), "obj");
        }
        if (obj == null)
        {
            return default(T);
        }
        System.Runtime.Serialization.IFormatter _formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        using (System.IO.Stream stream = new System.IO.MemoryStream())
        {
            _formatter.Serialize(stream, obj);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return (T)_formatter.Deserialize(stream);
        }

    }

    #region 根据枚举生成下拉列表数据源  
    /// <summary>  
    /// 根据枚举生成下拉列表的数据源  
    /// </summary>  
    /// <param name="enumType">枚举类型</param>  
    /// <param name="firstText">第一行文本(一般用于查询。例如：全部/请选择)</param>  
    /// <param name="firstValue">第一行值(一般用于查询。例如：全部/请选择的值)</param>  
    /// <returns></returns>  
    public static IList<SelectListItem> ToSelectList(Type enumType
        , string firstText = "请选择"
        , string firstValue = "-1")
    {
        IList<SelectListItem> listItem = new List<SelectListItem>();

        if (enumType.IsEnum)
        {
            AddFirst(listItem, firstText, firstValue);

            Array values = Enum.GetValues(enumType);
            if (null != values && values.Length > 0)
            {
                foreach (int item in values)
                {
                    listItem.Add(new SelectListItem { Value = item.ToString(), Text = Enum.GetName(enumType, item) });
                }
            }
        }
        else
        {
            throw new ArgumentException("请传入正确的枚举！");
        }
        return listItem;
    }

    /// <summary>  
    /// 根据枚举生成下拉列表的数据源  
    /// </summary>  
    /// <param name="enumType">枚举类型</param>  
    /// <param name="firstText">第一行文本(一般用于查询。例如：全部/请选择)</param>  
    /// <param name="firstValue">第一行值(一般用于查询。例如：全部/请选择的值)</param>  
    /// <returns></returns>  
    public static IList<SelectListItem> ToSelectList(Type enumType)
    {
        IList<SelectListItem> listItem = new List<SelectListItem>();

        if (enumType.IsEnum)
        {
            Array values = Enum.GetValues(enumType);
            if (null != values && values.Length > 0)
            {
                foreach (int item in values)
                {
                    listItem.Add(new SelectListItem { Value = item.ToString(), Text = Enum.GetName(enumType, item) });
                }
            }
        }
        else
        {
            throw new ArgumentException("请传入正确的枚举！");
        }
        return listItem;
    }
    static void AddFirst(IList<SelectListItem> listItem, string firstText, string firstValue)
    {
        //if (!string.IsNullOrWhiteSpace(firstText))
        //{
        //    if (string.IsNullOrWhiteSpace(firstValue))
        //        firstValue = "-1";
        listItem.Add(new SelectListItem { Text = firstText, Value = firstValue });
        //}
    }
    static void AddFirst(IList<SelectListItemNoNotify> listItem, string firstText, string firstValue)
    {
        //if (!string.IsNullOrWhiteSpace(firstText))
        //{
        //    if (string.IsNullOrWhiteSpace(firstValue))
        //        firstValue = "-1";
        listItem.Add(new SelectListItemNoNotify { Text = firstText, Value = firstValue });
        //}
    }

    /// <summary>  
    /// 根据枚举的描述生成下拉列表的数据源  
    /// </summary>  
    /// <param name="enumType"></param>  
    /// <returns></returns>  
    public static IList<SelectListItem> ToSelectListByDesc(
        Type enumType
        , string firstText = "请选择"
        , string firstValue = "-1"
        )
    {
        IList<SelectListItem> listItem = new List<SelectListItem>();

        if (enumType.IsEnum)
        {
            AddFirst(listItem, firstText, firstValue);
            string[] names = Enum.GetNames(enumType);
            names.ToList().ForEach(item =>
            {
                string description = string.Empty;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true); //获取属性字段数组    
                description = arr != null && arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : item;   //属性描述    

                listItem.Add(new SelectListItem() { Value = ((int)Enum.Parse(enumType, item)).ToString(), Text = description });
            });
        }
        else
        {
            throw new ArgumentException("请传入正确的枚举！");
        }
        return listItem;
    }
    public static IList<SelectListItemNoNotify> ToSelectListNoNotifyByDesc(
    Type enumType
    , string firstText = "请选择"
    , string firstValue = "-1"
    )
    {
        IList<SelectListItemNoNotify> listItem = new List<SelectListItemNoNotify>();

        if (enumType.IsEnum)
        {
            AddFirst(listItem, firstText, firstValue);
            string[] names = Enum.GetNames(enumType);
            names.ToList().ForEach(item =>
            {
                string description = string.Empty;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true); //获取属性字段数组    
                description = arr != null && arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : item;   //属性描述    

                listItem.Add(new SelectListItemNoNotify() { Value = ((int)Enum.Parse(enumType, item)).ToString(), Text = description });
            });
        }
        else
        {
            throw new ArgumentException("请传入正确的枚举！");
        }
        return listItem;
    }
    public static IList<SelectListItem> ToSelectListByDesc(
        Type enumType
        )
    {
        IList<SelectListItem> listItem = new List<SelectListItem>();

        if (enumType.IsEnum)
        {
            string[] names = Enum.GetNames(enumType);
            names.ToList().ForEach(item =>
            {
                string description = string.Empty;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true); //获取属性字段数组    
                description = arr != null && arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : item;   //属性描述    

                listItem.Add(new SelectListItem() { Value = ((int)Enum.Parse(enumType, item)).ToString(), Text = description });
            });
        }
        else
        {
            throw new ArgumentException("请传入正确的枚举！");
        }
        return listItem;
    }
    public static IList<SelectListItemNoNotify> ToSelectListNoNotifyByDesc(
      Type enumType
      )
    {
        IList<SelectListItemNoNotify> listItem = new List<SelectListItemNoNotify>();

        if (enumType.IsEnum)
        {
            string[] names = Enum.GetNames(enumType);
            names.ToList().ForEach(item =>
            {
                string description = string.Empty;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true); //获取属性字段数组    
                description = arr != null && arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : item;   //属性描述    

                listItem.Add(new SelectListItemNoNotify() { Value = ((int)Enum.Parse(enumType, item)).ToString(), Text = description });
            });
        }
        else
        {
            throw new ArgumentException("请传入正确的枚举！");
        }
        return listItem;
    }
    #endregion

    /// <summary>  
    /// 获取枚举的描述信息  
    /// </summary>  
    /// <param name="enumValue">枚举值</param>  
    /// <returns>描述</returns>  
    public static string GetDescription(this Enum enumValue)
    {
        string value = enumValue.ToString();
        FieldInfo field = enumValue.GetType().GetField(value);
        object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
        if (objs == null || objs.Length == 0) return value;
        System.ComponentModel.DescriptionAttribute attr = (System.ComponentModel.DescriptionAttribute)objs[0];
        return attr.Description;
    }

    public static T GetEnumValue<T>(string enumName, bool ignoreCase = false)
    {
        return (T)Enum.Parse(typeof(T), enumName, ignoreCase);
    }

    public static Dictionary<T, string> EnumToDictionary<T>()
    {
        return GetEnumValues<T>().ToDictionary(v => v, v => Enum.GetName(typeof(T), v));
    }

    public static IEnumerable<TEnum> GetEnumValues<TEnum>()
    {
        var type = typeof(TEnum);
        return Enum.GetValues(type).Cast<TEnum>();
    }

    /// <remarks>If a field doesn't have the defined attribute, null is provided. If a field has an attribute more than once, it causes an exception.</remarks>
    public static IDictionary<TEnum, TAttribute> GetEnumValueAttributes<TEnum, TAttribute>() where TAttribute : Attribute
    {
        var type = typeof(TEnum);
        return GetEnumFields(type).ToDictionary(f => (TEnum)f.GetRawConstantValue(), f => GetCustomAttributes<TAttribute>(f,false).FirstOrDefault());
    }

    public static IEnumerable<T> GetCustomAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
    {
        return attributeProvider.GetCustomAttributes(typeof(T), inherit).Cast<T>();
    }
    private static IEnumerable<FieldInfo> GetEnumFields(Type enumType)
    {
        return enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
    }
}
