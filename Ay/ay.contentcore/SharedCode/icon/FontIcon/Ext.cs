using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ay.contentcore.Mgr
{
    public static class Ext {
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
    }
}
