using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in DateTimeUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorDateTimeUpDown : DateTimeUpDown
	{
		static PropertyGridEditorDateTimeUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorDateTimeUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorDateTimeUpDown)));
		}
	}
}
