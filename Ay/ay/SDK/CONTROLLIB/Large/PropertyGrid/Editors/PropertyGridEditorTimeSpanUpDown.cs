using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in TimeSpanUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorTimeSpanUpDown : TimeSpanUpDown
	{
		static PropertyGridEditorTimeSpanUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorTimeSpanUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorTimeSpanUpDown)));
		}
	}
}
