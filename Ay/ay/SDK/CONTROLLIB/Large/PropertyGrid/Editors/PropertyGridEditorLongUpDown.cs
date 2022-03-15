using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in LongUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorLongUpDown : LongUpDown
	{
		static PropertyGridEditorLongUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorLongUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorLongUpDown)));
		}
	}
}
