using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
	/// <summary>Ensures that the same value is assigned to a common property shared by each selected object and displays, in the PropertyGrid, whether the property is
	/// identical for each of those objects.</summary>
	public class CommonPropertyConverter : IMultiValueConverter
	{
		private TypeConverter _propertyTypeConverter;

		internal CommonPropertyConverter(Type type)
		{
			_propertyTypeConverter = TypeDescriptor.GetConverter(type);
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Distinct().Count() > 1)
			{
				return null;
			}
			return values[0];
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			object element = value;
			if (GeneralUtilities.CanConvertValue(value, targetTypes[0]))
			{
				if (!_propertyTypeConverter.CanConvertFrom(value.GetType()))
				{
					throw new InvalidDataException("Cannot convert from targetType.");
				}
				element = _propertyTypeConverter.ConvertFrom(value);
			}
			return Enumerable.Repeat(element, targetTypes.Count()).ToArray();
		}
	}
}
