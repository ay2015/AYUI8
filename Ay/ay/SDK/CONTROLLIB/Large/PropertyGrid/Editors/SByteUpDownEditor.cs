using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	internal class SByteUpDownEditor : NumericUpDownEditor<SByteUpDown, sbyte?>
	{
		protected override SByteUpDown CreateEditor()
		{
			return new PropertyGridEditorSByteUpDown();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(sbyte)));
		}
	}
}
