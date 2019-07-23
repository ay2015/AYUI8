using System;
using System.Reflection;

namespace Dapper
{
	internal static class TypeExtensions
	{
		public static string Name(this Type type)
		{
			return type.Name;
		}

		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		public static TypeCode GetTypeCode(Type type)
		{
			return Type.GetTypeCode(type);
		}

		public static MethodInfo GetPublicInstanceMethod(this Type type, string name, Type[] types)
		{
			return type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public, null, types, null);
		}
	}
}
