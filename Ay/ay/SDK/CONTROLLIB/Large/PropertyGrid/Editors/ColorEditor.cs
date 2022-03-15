using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in <strong>ColorPicker</strong> editor.</summary>
	public class ColorEditor : TypeEditor<ColorPicker>
	{
		protected override ColorPicker CreateEditor()
		{
			return new PropertyGridEditorColorPicker();
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.Editor.BorderThickness = new Thickness(0.0);
			base.Editor.DisplayColorAndName = true;
		}

		/// <summary>Sets the value dependency property.</summary>
		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = ColorPicker.SelectedColorProperty;
		}
	}
}
