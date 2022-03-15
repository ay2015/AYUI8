using System.Windows;
using System.Windows.Controls;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in TextBlock editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorTextBlock : TextBlock
	{
		static PropertyGridEditorTextBlock()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorTextBlock), new FrameworkPropertyMetadata(typeof(PropertyGridEditorTextBlock)));
		}
	}
}
