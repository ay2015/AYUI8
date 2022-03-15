using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Xceed.Wpf.Toolkit.Core.Utilities
{
	internal static class ReflectionHelper
	{
		[Conditional("DEBUG")]
		internal static void ValidatePublicPropertyName(object sourceObject, string propertyName)
		{
			if (sourceObject == null)
			{
				throw new ArgumentNullException("sourceObject");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
		}

		[Conditional("DEBUG")]
		internal static void ValidatePropertyName(object sourceObject, string propertyName)
		{
			if (sourceObject == null)
			{
				throw new ArgumentNullException("sourceObject");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
		}

		internal static bool TryGetEnumDescriptionAttributeValue(Enum enumeration, out string description)
		{
			try
			{
				FieldInfo field = enumeration.GetType().GetField(enumeration.ToString());
				DescriptionAttribute[] array = field.GetCustomAttributes(typeof(DescriptionAttribute), true) as DescriptionAttribute[];
				if (array != null && array.Length > 0)
				{
					description = array[0].Description;
					return true;
				}
			}
			catch
			{
			}
			description = string.Empty;
			return false;
		}

		[DebuggerStepThrough]
		internal static string GetPropertyOrFieldName(MemberExpression expression)
		{
			string propertyOrFieldName;
			if (!TryGetPropertyOrFieldName(expression, out propertyOrFieldName))
			{
				throw new InvalidOperationException("Unable to retrieve the property or field name.");
			}
			return propertyOrFieldName;
		}

		[DebuggerStepThrough]
		internal static string GetPropertyOrFieldName<TMember>(Expression<Func<TMember>> expression)
		{
			string propertyOrFieldName;
			if (!TryGetPropertyOrFieldName(expression, out propertyOrFieldName))
			{
				throw new InvalidOperationException("Unable to retrieve the property or field name.");
			}
			return propertyOrFieldName;
		}

		[DebuggerStepThrough]
		internal static bool TryGetPropertyOrFieldName(MemberExpression expression, out string propertyOrFieldName)
		{
			propertyOrFieldName = null;
			if (expression == null)
			{
				return false;
			}
			propertyOrFieldName = expression.Member.Name;
			return true;
		}

		[DebuggerStepThrough]
		internal static bool TryGetPropertyOrFieldName<TMember>(Expression<Func<TMember>> expression, out string propertyOrFieldName)
		{
			propertyOrFieldName = null;
			if (expression == null)
			{
				return false;
			}
			return TryGetPropertyOrFieldName(expression.Body as MemberExpression, out propertyOrFieldName);
		}

		public static bool IsPublicInstanceProperty(Type type, string propertyName)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
			return type.GetProperty(propertyName, bindingAttr) != null;
		}
	}
}
