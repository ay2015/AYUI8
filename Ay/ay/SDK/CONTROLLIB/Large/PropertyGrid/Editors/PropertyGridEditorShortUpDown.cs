using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in ShortUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorShortUpDown : ShortUpDown
	{
		static PropertyGridEditorShortUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorShortUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorShortUpDown)));
		}
	}
}
