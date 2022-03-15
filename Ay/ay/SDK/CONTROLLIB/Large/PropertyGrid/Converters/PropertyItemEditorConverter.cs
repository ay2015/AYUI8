using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
	public class PropertyItemEditorConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values.Length != 2)
			{
				return null;
			}
			object obj = values[0];
			bool? flag = values[1] as bool?;
			if (obj == null || !flag.HasValue)
			{
				return obj;
			}
			Type type = obj.GetType();
			PropertyInfo property = type.GetProperty("IsReadOnly");
			if (property != null)
			{
				if (!IsPropertySetLocally(obj, TextBoxBase.IsReadOnlyProperty))
				{
					property.SetValue(obj, flag, null);
				}
			}
			else
			{
				PropertyInfo property2 = type.GetProperty("IsEnabled");
				if (property2 != null && !IsPropertySetLocally(obj, UIElement.IsEnabledProperty))
				{
					property2.SetValue(obj, !flag, null);
				}
			}
			return obj;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private bool IsPropertySetLocally(object editor, DependencyProperty dp)
		{
			if (dp == null)
			{
				return false;
			}
			DependencyObject dependencyObject = editor as DependencyObject;
			if (dependencyObject == null)
			{
				return false;
			}
			return DependencyPropertyHelper.GetValueSource(dependencyObject, dp).BaseValueSource == BaseValueSource.Local;
		}
	}
}
