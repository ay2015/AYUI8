using System;
using System.Collections.Generic;
using System.Linq;

namespace Xceed.Wpf.Toolkit.Core.Utilities
{
	internal class ListUtilities
	{
		internal static Type GetListItemType(Type listType)
		{
			Type type = listType.GetInterfaces().FirstOrDefault(delegate(Type i)
			{
				if (i.IsGenericType)
				{
					return i.GetGenericTypeDefinition() == typeof(IList<>);
				}
				return false;
			});
			if (!(type != null))
			{
				return null;
			}
			return type.GetGenericArguments()[0];
		}

		internal static Type GetCollectionItemType(Type colType)
		{
			Type type = null;
			type = ((!colType.IsGenericType || !(colType.GetGenericTypeDefinition() == typeof(ICollection<>))) ? colType.GetInterfaces().FirstOrDefault(delegate(Type i)
			{
				if (i.IsGenericType)
				{
					return i.GetGenericTypeDefinition() == typeof(ICollection<>);
				}
				return false;
			}) : colType);
			if (!(type != null))
			{
				return null;
			}
			return type.GetGenericArguments()[0];
		}

		internal static Type[] GetDictionaryItemsType(Type dictType)
		{
			if (!dictType.IsGenericType || (!(dictType.GetGenericTypeDefinition() == typeof(Dictionary<, >)) && !(dictType.GetGenericTypeDefinition() == typeof(IDictionary<, >))))
			{
				return null;
			}
			return new Type[2]
			{
				dictType.GetGenericArguments()[0],
				dictType.GetGenericArguments()[1]
			};
		}

		internal static object CreateEditableKeyValuePair(object key, Type keyType, object value, Type valueType)
		{
			Type type = CreateEditableKeyValuePairType(keyType, valueType);
			return Activator.CreateInstance(type, key, value);
		}

		internal static Type CreateEditableKeyValuePairType(Type keyType, Type valueType)
		{
			Type typeFromHandle = typeof(EditableKeyValuePair<, >);
			Type[] typeArguments = new Type[2]
			{
				keyType,
				valueType
			};
			return typeFromHandle.MakeGenericType(typeArguments);
		}
	}
}
