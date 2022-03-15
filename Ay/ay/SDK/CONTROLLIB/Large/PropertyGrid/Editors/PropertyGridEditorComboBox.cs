using System.Windows;
using System.Windows.Controls;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in ComboBox editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorComboBox : ComboBox
	{
		static PropertyGridEditorComboBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorComboBox), new FrameworkPropertyMetadata(typeof(PropertyGridEditorComboBox)));
		}
	}
}
