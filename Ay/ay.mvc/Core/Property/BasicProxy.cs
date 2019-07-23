using System;
using System.Reflection;
using System.Collections;

namespace Ay.MvcFramework
{
    /// <summary>
    /// BasicProxy 的摘要说明。
    /// </summary>
    internal sealed class BasicProxy : IDynaAccessProxy
    {
        /// <summary>
        /// 类提取器的标志
        /// </summary>
        private static readonly BindingFlags BINDING_FLAGS
            = BindingFlags.Instance | BindingFlags.Public
            | BindingFlags.GetProperty | BindingFlags.SetProperty
            | BindingFlags.GetField | BindingFlags.SetField;

        private Type _proxyType = null;
        /// <summary>
        /// 设置器Map(propertyName,IGetterSetter)
        /// </summary>
        private Hashtable _properties = new Hashtable();
        private string[] _propertyNames = new string[0];

        public BasicProxy(Type type)
        {
            _proxyType = type;
            PreparePropertiess(type);


            _propertyNames = new string[_properties.Count];
            _properties.Keys.CopyTo(_propertyNames, 0);
        }

        private void PreparePropertiess(Type type)
        {
            // 提取属性访问器
            PropertyInfo[] properties = type.GetProperties(BINDING_FLAGS);
            for (int i = 0; i < properties.Length; i++)
            {
                _properties.Add(properties[i].Name, new PropertyGetterSetter(properties[i]));
            }
            // 提取字段访问器
            FieldInfo[] fields = type.GetFields(BINDING_FLAGS);
            for (int i = 0; i < fields.Length; i++)
            {
                _properties.Add(fields[i].Name, new FieldGetterSetter(fields[i]));
            }

        }

         #region IDynaAccessProxy methods

        public string[] GetPropertyNames(object target)
        {
            return _propertyNames;
        }

        public void SetProperty(object target, string propertyName, object propertyValue)
        {
            try
            {
                IGetterSetter hSet = (IGetterSetter)_properties[propertyName];
                if (hSet == null)
                {
                    throw new DynaAccessException("No Set method for property " + propertyName + " on instance of " + _proxyType.Name);
                }

                // 尽最大努力传递正确的值
                if (!hSet.PropertyType.IsInstanceOfType(propertyValue) && propertyValue != null)
                {
                    if (hSet.PropertyType.Equals(typeof(string)))
                    {
                        hSet.Set(target, propertyValue.ToString());
                    }
                    else if (hSet.PropertyType.Equals(typeof(Guid)))
                    {
                        hSet.Set(target, new Guid(propertyValue.ToString()));
                    }
                    else
                        hSet.Set(target, Convert.ChangeType(propertyValue, hSet.PropertyType));
                }
                else
                {
                    hSet.Set(target, propertyValue);
                }
            }
            catch (DynaAccessException dae)
            {
                throw dae;
            }
            catch (Exception e)
            {
                throw new DynaAccessException(e);
            }
        }

        public object GetProperty(object target, string propertyName)
        {
            try
            {
                IGetterSetter hGet = (IGetterSetter)_properties[propertyName];
                if (hGet == null)
                {
                    throw new DynaAccessException("No Get method for property " + propertyName + " on instance of " + _proxyType.Name);
                }
                return hGet.Get(target);
            }
            catch (DynaAccessException dae)
            {
                throw dae;
            }
            catch (Exception e)
            {
                throw new DynaAccessException(e);
            }
        }

        public bool HasProperty(object target, string propertyName)
        {
            try
            {
                return _properties.ContainsKey(propertyName);
            }
            catch (Exception)
            {
                return false;
            }

        }

        public Type GetPropertyType(object target, string propertyName)
        {
            IGetterSetter hSet = (IGetterSetter)_properties[propertyName];
            if (hSet == null)
            {
                throw new DynaAccessException("No Set method for property " + propertyName + " on instance of " + _proxyType.Name);
            }
            return hSet.PropertyType;
        }

        public Type ProxyType
        {
            get
            {
                return _proxyType;
            }
        }

        #endregion
    }
}
