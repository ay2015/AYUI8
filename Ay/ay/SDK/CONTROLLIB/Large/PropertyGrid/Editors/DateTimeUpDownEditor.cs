using System;
using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in DateTimeUpDown editor.</summary>
	public class DateTimeUpDownEditor : UpDownEditor<DateTimeUpDown, DateTime?>
	{
		protected override DateTimeUpDown CreateEditor()
		{
			return new PropertyGridEditorDateTimeUpDown();
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(DateTime)));
		}
	}
}
