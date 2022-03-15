using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	internal class ULongUpDownEditor : NumericUpDownEditor<ULongUpDown, ulong?>
	{
		protected override ULongUpDown CreateEditor()
		{
			return new PropertyGridEditorULongUpDown();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(ulong)));
		}
	}
}
