using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in LongUpDown editor.</summary>
	public class LongUpDownEditor : NumericUpDownEditor<LongUpDown, long?>
	{
		protected override LongUpDown CreateEditor()
		{
			return new PropertyGridEditorLongUpDown();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(long)));
		}
	}
}
