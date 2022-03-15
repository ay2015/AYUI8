using System.Windows;
using System.Windows.Controls;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in TextBlock editor.</summary>
	public class TextBlockEditor : TypeEditor<TextBlock>
	{
		protected override TextBlock CreateEditor()
		{
			return new PropertyGridEditorTextBlock();
		}

		/// <summary>Sets the value dependency property.</summary>
		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = TextBlock.TextProperty;
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.Editor.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
			base.Editor.TextTrimming = TextTrimming.CharacterEllipsis;
		}
	}
}
