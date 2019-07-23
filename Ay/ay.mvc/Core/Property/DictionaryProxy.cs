using System;
using System.Collections;

namespace Ay.MvcFramework
{
    /// <summary>
    /// DictionaryProxy 的摘要说明。
    /// </summary>
    internal sealed class DictionaryProxy : IDynaAccessProxy
    {
        private string[] _propertyNames = new string[0];

        public DictionaryProxy()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region IDynaAccessProxy methods

        public string[] GetPropertyNames(object target)
        {
            if (target is IDictionary)
            {
                string[] propertyNames = new string[((IDictionary)target).Count];
                ((IDictionary)target).Keys.CopyTo(propertyNames, 0);
                return propertyNames;
            }
            else
            {
                return _propertyNames;
            }
        }

        public void SetProperty(object target, string propertyName, object propertyValue)
        {
            try
            {
                if (target is IDictionary)
                {
                    ((IDictionary)target)[propertyName] = propertyValue;
                }
                else
                {
                    throw new DynaAccessException(target.GetType().Name + " class is not a IDictionary.");
                }
            }
            catch (Exception e)
            {
                throw new DynaAccessException(e);
            }
        }

        public object GetProperty(object target, string propertyName)
        {
            object value = null;

            try
            {
                if (target is IDictionary)
                {
                    value = ((IDictionary)target)[propertyName];
                }
                else
                {
                    throw new DynaAccessException(target.GetType().Name + " class is not IDictionary.");
                }
            }
            catch (Exception e)
            {
                throw new DynaAccessException(e);
            }
            return value;
        }

        public bool HasProperty(object target, string propertyName)
        {
            if (target is IDictionary)
            {
                try
                {
                    return ((IDictionary)target).Contains(propertyName);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Type GetPropertyType(object target, string propertyName)
        {
            object value = null;

            try
            {
                if (target is IDictionary)
                {
                    value = ((IDictionary)target)[propertyName];
                    if (value == null)
                    {
                        value = new object();
                    }

                    return value.GetType();
                }
                else
                {
                    throw new DynaAccessException(target.GetType().Name + " class is not a IDictionary.");
                }
            }
            catch (Exception e)
            {
                throw new DynaAccessException(e);
            }
        }

        public Type ProxyType
        {
            get
            {
                return typeof(System.Collections.IDictionary);
            }
        }

        #endregion
    }
}
