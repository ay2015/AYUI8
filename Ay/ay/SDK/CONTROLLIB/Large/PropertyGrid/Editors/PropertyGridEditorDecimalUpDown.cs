using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in DecimalUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorDecimalUpDown : DecimalUpDown
	{
		static PropertyGridEditorDecimalUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorDecimalUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorDecimalUpDown)));
		}
	}
}
