using System;
using System.Reflection;
namespace Ay.MvcFramework
{
	/// <summary>
	/// 指定属性值提取/设置接口
	/// </summary>
	public interface IGetterSetter
	{
		/// <summary>
		/// 得到指定对象实例的属性值
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		object Get(object target);

		/// <summary>
		/// 设置指定类实例的属性值
		/// </summary>
		/// <param name="target"></param>
		/// <param name="value"></param>
		void Set(object target, object value);

		/// <summary>
		/// 得到属性的类型.
		/// </summary>
		/// <returns></returns>
		System.Type PropertyType{ get; }

		/// <summary>
		/// 得到属性名称
		/// </summary>
		/// <returns></returns>
		string PropertyName { get; } 
	}

	/// <summary>
	/// 类属性提取/设置接口
	/// </summary>
	public interface IPropertyGetterSetter : IGetterSetter
	{
		PropertyInfo Property { get; }
	}

	/// <summary>
	/// 类字段提取/设置接口
	/// </summary>
	public interface IFieldGetterSetter : IGetterSetter
	{
		FieldInfo Field { get; }
	}
}
