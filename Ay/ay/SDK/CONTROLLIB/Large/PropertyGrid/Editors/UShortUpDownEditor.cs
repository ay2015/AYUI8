using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	internal class UShortUpDownEditor : NumericUpDownEditor<UShortUpDown, ushort?>
	{
		protected override UShortUpDown CreateEditor()
		{
			return new PropertyGridEditorUShortUpDown();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(ushort)));
		}
	}
}
