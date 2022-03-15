using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in SingleUpDown editor.</summary>
	public class SingleUpDownEditor : NumericUpDownEditor<SingleUpDown, float?>
	{
		protected override SingleUpDown CreateEditor()
		{
			return new PropertyGridEditorSingleUpDown();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			base.Editor.AllowInputSpecialValues = AllowedSpecialValues.Any;
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(float)));
		}
	}
}
