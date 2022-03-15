using System.Windows;
using System.Windows.Controls;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in CheckBox editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorCheckBox : CheckBox
	{
		static PropertyGridEditorCheckBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorCheckBox), new FrameworkPropertyMetadata(typeof(PropertyGridEditorCheckBox)));
		}
	}
}
