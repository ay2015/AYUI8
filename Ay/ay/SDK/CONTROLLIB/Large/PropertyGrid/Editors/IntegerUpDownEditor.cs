using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in IntegerUpDown editor.</summary>
	public class IntegerUpDownEditor : NumericUpDownEditor<IntegerUpDown, int?>
	{
		protected override IntegerUpDown CreateEditor()
		{
			return new PropertyGridEditorIntegerUpDown();
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(int)));
		}
	}
}
