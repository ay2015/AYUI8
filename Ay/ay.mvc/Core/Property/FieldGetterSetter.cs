using System;
using System.Reflection;

namespace Ay.MvcFramework
{
	/// <summary>
	/// 类字段提取/设置接口实现
	/// </summary>
	internal sealed class FieldGetterSetter : IFieldGetterSetter
	{
		private readonly FieldInfo _field;

		public FieldGetterSetter(FieldInfo _field) 
		{
			this._field = _field;
		}

		#region IFieldGetterSetter Members

		public object Get(object target)
		{
			return _field.GetValue(target);
		}

		public void Set(object target, object value)
		{
			_field.SetValue( target, value);
		}

		public System.Type PropertyType
		{
			get { return _field.FieldType; }
		}

		public string PropertyName
		{
			get { return _field.Name; }
		}

		public FieldInfo Field
		{
			get { return _field; }
		}

		#endregion
	}
}
