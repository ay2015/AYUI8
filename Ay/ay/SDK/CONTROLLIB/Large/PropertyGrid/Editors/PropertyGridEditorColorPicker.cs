using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in ColorPicker editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorColorPicker : ColorPicker
	{
		static PropertyGridEditorColorPicker()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorColorPicker), new FrameworkPropertyMetadata(typeof(PropertyGridEditorColorPicker)));
		}
	}
}
