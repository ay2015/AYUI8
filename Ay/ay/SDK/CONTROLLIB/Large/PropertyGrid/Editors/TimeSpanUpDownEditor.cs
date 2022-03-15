using System;
using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in TimeSpanUpDown editor.</summary>
	public class TimeSpanUpDownEditor : UpDownEditor<TimeSpanUpDown, TimeSpan?>
	{
		protected override TimeSpanUpDown CreateEditor()
		{
			return new PropertyGridEditorTimeSpanUpDown();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(TimeSpan)));
		}
	}
}
