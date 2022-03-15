using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in TextBox editor.</summary>
	public class TextBoxEditor : TypeEditor<ay.Controls.AyTextBox>
	{
		protected override ay.Controls.AyTextBox CreateEditor()
		{
			return new PropertyGridEditorTextBox();
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			DisplayAttribute attribute = PropertyGridUtilities.GetAttribute<DisplayAttribute>(propertyItem.PropertyDescriptor);
			if (attribute != null)
			{
				base.Editor.Placeholder = attribute.GetPrompt();
			}
		}

		/// <summary>Sets the value dependency property.</summary>
		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = TextBox.TextProperty;
		}
	}
}
