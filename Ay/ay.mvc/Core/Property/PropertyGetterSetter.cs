using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ay.MvcFramework
{

	/// <summary>
	/// 类属性提取/设置接口实现
	/// </summary>
	internal sealed class PropertyGetterSetter : IPropertyGetterSetter
	{
		private PropertyInfo _property;

		public PropertyGetterSetter(PropertyInfo _property) 
		{
			this._property = _property;
		}

		#region IPropertyGetterSetter Members

		public object Get(object target)
		{
			return _property.GetValue(target, null);
		}

		public void Set(object target, object value) 
		{
			_property.SetValue(target,value,null);
		}

		public System.Type PropertyType
		{
			get { return _property.PropertyType; }
		}

		public string PropertyName
		{
			get { return _property.Name; }
		}

		public PropertyInfo Property
		{
			get{ return _property; }
		}

		#endregion
	}

}

