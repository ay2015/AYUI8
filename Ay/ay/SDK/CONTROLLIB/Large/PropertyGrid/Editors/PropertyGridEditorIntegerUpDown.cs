using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in IntegerUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorIntegerUpDown : IntegerUpDown
	{
		static PropertyGridEditorIntegerUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorIntegerUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorIntegerUpDown)));
		}
	}
}
