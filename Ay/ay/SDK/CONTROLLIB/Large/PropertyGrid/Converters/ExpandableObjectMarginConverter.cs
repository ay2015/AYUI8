using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
	/// <summary>Converts the level of a given child in an expandable object to a Thickness struct to accomodate that child.</summary>
	public class ExpandableObjectMarginConverter : IValueConverter
	{
		/// <summary>Converts a value. The data binding engine calls this method when it propagates a value from the binding source to the binding target.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="value">The value produced by the <span class="clsGlossary" onmouseover="showDef()" onmouseout="clearDef()" g_rid="binding_source#82edbb63-5cbf-46f8-bf38-164eb00a1ec1"><!--In data binding, the object from which the value is obtained.-->binding source (a child
		/// level).</span></param>
		/// <param name="targetType">The type of the <span class="clsGlossary" onmouseover="showDef()" onmouseout="clearDef()" g_rid="binding_target#2b16294b-64eb-4210-83a4-c215cf4c140a"><!--In data binding, the object that consumes the value of the binding. A target property must be a dependency property on a DependencyObject type.-->binding
		/// target</span> property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = (int)value;
			return new Thickness((double)(num * 15), 0.0, 0.0, 0.0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
