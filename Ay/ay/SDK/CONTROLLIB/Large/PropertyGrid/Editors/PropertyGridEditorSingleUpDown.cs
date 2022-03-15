using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in SingleUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorSingleUpDown : SingleUpDown
	{
		static PropertyGridEditorSingleUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorSingleUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorSingleUpDown)));
		}
	}
}
