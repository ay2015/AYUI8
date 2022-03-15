using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class CommonPropertyExceptionValidationRule : ValidationRule
	{
		private TypeConverter _propertyTypeConverter;

		private Type _type;

		internal CommonPropertyExceptionValidationRule(Type type)
		{
			_propertyTypeConverter = TypeDescriptor.GetConverter(type);
			_type = type;
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			ValidationResult result = new ValidationResult(true, null);
			if (GeneralUtilities.CanConvertValue(value, _type))
			{
				try
				{
					_propertyTypeConverter.ConvertFrom(value);
					return result;
				}
				catch (Exception ex)
				{
					return new ValidationResult(false, ex.Message);
				}
			}
			return result;
		}
	}
}
