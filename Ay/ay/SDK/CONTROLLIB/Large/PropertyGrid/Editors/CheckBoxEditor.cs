using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in CheckBox editor.</summary>
	public class CheckBoxEditor : TypeEditor<CheckBox>
	{
		protected override CheckBox CreateEditor()
		{
			return new PropertyGridEditorCheckBox();
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.Editor.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
		}

		/// <summary>Sets the value dependency property.</summary>
		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = ToggleButton.IsCheckedProperty;
		}
	}
}
