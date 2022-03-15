using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in FilePicker editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorFilePicker : FilePicker
	{
		static PropertyGridEditorFilePicker()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorFilePicker), new FrameworkPropertyMetadata(typeof(PropertyGridEditorFilePicker)));
		}
	}
}
