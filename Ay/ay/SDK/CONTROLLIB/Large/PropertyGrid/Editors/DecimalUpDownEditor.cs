using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in DecimalUpDown editor.</summary>
	public class DecimalUpDownEditor : NumericUpDownEditor<DecimalUpDown, decimal?>
	{
		protected override DecimalUpDown CreateEditor()
		{
			return new PropertyGridEditorDecimalUpDown();
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(decimal)));
		}
	}
}
