using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in TextBox editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorTextBox : ay.Controls.AyTextBox
	{
		static PropertyGridEditorTextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorTextBox), new FrameworkPropertyMetadata(typeof(PropertyGridEditorTextBox)));
		}
	}
}
