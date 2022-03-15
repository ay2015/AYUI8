using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in ShortUpDown editor.</summary>
	public class ShortUpDownEditor : NumericUpDownEditor<ShortUpDown, short?>
	{
		protected override ShortUpDown CreateEditor()
		{
			return new PropertyGridEditorShortUpDown();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.SetControlProperties(propertyItem);
			SetMinMaxFromRangeAttribute(propertyItem.PropertyDescriptor, TypeDescriptor.GetConverter(typeof(short)));
		}
	}
}
