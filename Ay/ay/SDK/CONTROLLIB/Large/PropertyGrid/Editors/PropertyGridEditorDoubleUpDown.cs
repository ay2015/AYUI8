using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in DoubleUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorDoubleUpDown : DoubleUpDown
	{
		static PropertyGridEditorDoubleUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorDoubleUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorDoubleUpDown)));
		}
	}
}
