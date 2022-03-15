using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;

namespace Xceed.Wpf.Toolkit.Core.Attributes
{
	internal class LocalizationHelper
	{
		private struct ResourcesGetters
		{
			public Func<ResourceManager> ResourceManager;

			public Func<CultureInfo> Culture;
		}

		private Type _resourceClassType;

		private static Dictionary<Type, ResourcesGetters> _getters;

		static LocalizationHelper()
		{
			_getters = new Dictionary<Type, ResourcesGetters>();
		}

		public LocalizationHelper(Type resourceClassType)
		{
			if (resourceClassType == null)
			{
				throw new ArgumentNullException("resourceClassType");
			}
			_resourceClassType = resourceClassType;
			if (!_getters.ContainsKey(_resourceClassType))
			{
				Func<ResourceManager> resourceManager = CreateGetter<ResourceManager>("ResourceManager");
				Func<CultureInfo> culture = CreateGetter<CultureInfo>("Culture");
				ResourcesGetters value = new ResourcesGetters
				{
					ResourceManager = resourceManager,
					Culture = culture
				};
				_getters.Add(_resourceClassType, value);
			}
		}

		public string GetString(string resourceKey)
		{
			ResourcesGetters resourcesGetters = _getters[_resourceClassType];
			return resourcesGetters.ResourceManager().GetString(resourceKey, resourcesGetters.Culture());
		}

		private Func<T> CreateGetter<T>(string propertyName)
		{
			try
			{
				PropertyInfo property = _resourceClassType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				MemberExpression body = Expression.Property(null, property);
				return (Func<T>)Expression.Lambda(body).Compile();
			}
			catch (Exception innerException)
			{
				string message = string.Format("Type must implement a static property named '{0}' that return a {1} instance.", propertyName, typeof(T));
				throw new ArgumentException(message, "resourceClassType", innerException);
			}
		}
	}
}
