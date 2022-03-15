using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	internal class UIntegerUpDownEditor : NumericUpDownEditor<UIntegerUpDown, uint?>
	{
		protected override UIntegerUpDown CreateEditor()
		{
			return new PropertyGridEditorUIntegerUpDown();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(uint)));
		}
	}
}
