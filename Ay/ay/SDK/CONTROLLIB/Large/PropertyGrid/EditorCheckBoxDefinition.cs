using System.Windows;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a checkbox editor in the PropertyGrid.</summary>
	public class EditorCheckBoxDefinition : EditorBoundDefinition
	{
		public bool IsThreeState
		{
			get;
			set;
		}

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			PropertyGridEditorCheckBox propertyGridEditorCheckBox = new PropertyGridEditorCheckBox();
			propertyGridEditorCheckBox.IsThreeState = IsThreeState;
			UpdateStyle(propertyGridEditorCheckBox);
			UpdateBinding(propertyGridEditorCheckBox, ToggleButton.IsCheckedProperty, propertyItem);
			return propertyGridEditorCheckBox;
		}
	}
}
