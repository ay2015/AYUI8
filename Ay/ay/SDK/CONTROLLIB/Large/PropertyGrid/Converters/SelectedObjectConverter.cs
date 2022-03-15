using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
	public class SelectedObjectConverter : IValueConverter
	{
		private const string ValidParameterMessage = "parameter must be one of the following strings: 'Type', 'TypeName', 'SelectedObjectName'";

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter");
			}
			if (!(parameter is string))
			{
				throw new ArgumentException("parameter must be one of the following strings: 'Type', 'TypeName', 'SelectedObjectName'");
			}
			if (CompareParam(parameter, "Type"))
			{
				return ConvertToType(value, culture);
			}
			if (CompareParam(parameter, "TypeName"))
			{
				return ConvertToTypeName(value, culture);
			}
			if (CompareParam(parameter, "SelectedObjectName"))
			{
				return ConvertToSelectedObjectName(value, culture);
			}
			throw new ArgumentException("parameter must be one of the following strings: 'Type', 'TypeName', 'SelectedObjectName'");
		}

		private bool CompareParam(object parameter, string parameterValue)
		{
			return string.Compare((string)parameter, parameterValue, true) == 0;
		}

		private object ConvertToType(object value, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			return value.GetType();
		}

		private object ConvertToTypeName(object value, CultureInfo culture)
		{
			if (value == null)
			{
				return string.Empty;
			}
			Type type = value.GetType();
			if (type.GetInterface("ICustomTypeProvider", true) != null)
			{
				MethodInfo method = type.GetMethod("GetCustomType");
				type = (method.Invoke(value, null) as Type);
			}
			DisplayNameAttribute displayNameAttribute = type.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault();
			if (displayNameAttribute != null)
			{
				return displayNameAttribute.DisplayName;
			}
			return type.Name;
		}

		private object ConvertToSelectedObjectName(object value, CultureInfo culture)
		{
			if (value == null)
			{
				return string.Empty;
			}
			Type type = value.GetType();
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if (propertyInfo.Name == "Name")
				{
					return propertyInfo.GetValue(value, null);
				}
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
