using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in DoubleUpDown editor.</summary>
	public class DoubleUpDownEditor : NumericUpDownEditor<DoubleUpDown, double?>
	{
		protected override DoubleUpDown CreateEditor()
		{
			return new PropertyGridEditorDoubleUpDown();
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			base.Editor.AllowInputSpecialValues = AllowedSpecialValues.Any;
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(double)));
		}
	}
}
