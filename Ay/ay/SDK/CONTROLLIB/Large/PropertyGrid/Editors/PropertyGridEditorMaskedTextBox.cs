using ay.Controls;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in MaskedTextBox editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorMaskedTextBox : MaskedTextBox
	{
		static PropertyGridEditorMaskedTextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorMaskedTextBox), new FrameworkPropertyMetadata(typeof(PropertyGridEditorMaskedTextBox)));
		}
	}
}
