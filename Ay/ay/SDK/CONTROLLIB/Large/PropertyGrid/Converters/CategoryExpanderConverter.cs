using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
	public class CategoryExpanderConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Count() != 3 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
			{
				return true;
			}
			bool flag = (bool)values[0];
			CustomPropertyItem objB = values[1] as CustomPropertyItem;
			ReadOnlyObservableCollection<object> readOnlyObservableCollection = values[2] as ReadOnlyObservableCollection<object>;
			if (readOnlyObservableCollection != null)
			{
				foreach (object item in readOnlyObservableCollection)
				{
					CustomPropertyItem customPropertyItem = item as CustomPropertyItem;
					if (customPropertyItem != null && !object.Equals(customPropertyItem, objB))
					{
						customPropertyItem.IsCategoryExpanded = flag;
					}
				}
			}
			return flag;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[1]
			{
				value
			};
		}
	}
}
