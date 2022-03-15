using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a text editor in the PropertyGrid.</summary>
	public class EditorTextDefinition : EditorBoundDefinition
	{
		public static readonly DependencyProperty FontFamilyProperty = Control.FontFamilyProperty.AddOwner(typeof(EditorTextDefinition));

		public static readonly DependencyProperty FontSizeProperty = Control.FontSizeProperty.AddOwner(typeof(EditorTextDefinition));

		public static readonly DependencyProperty FontStyleProperty = Control.FontStyleProperty.AddOwner(typeof(EditorTextDefinition));

		public static readonly DependencyProperty FontWeightProperty = Control.FontWeightProperty.AddOwner(typeof(EditorTextDefinition));

		public static readonly DependencyProperty ForegroundProperty = Control.ForegroundProperty.AddOwner(typeof(EditorTextDefinition));

		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)GetValue(FontFamilyProperty);
			}
			set
			{
				SetValue(FontFamilyProperty, value);
			}
		}

		public double FontSize
		{
			get
			{
				return (double)GetValue(FontSizeProperty);
			}
			set
			{
				SetValue(FontSizeProperty, value);
			}
		}

		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)GetValue(FontStyleProperty);
			}
			set
			{
				SetValue(FontStyleProperty, value);
			}
		}

		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)GetValue(FontWeightProperty);
			}
			set
			{
				SetValue(FontWeightProperty, value);
			}
		}

		public Brush Foreground
		{
			get
			{
				return (Brush)GetValue(ForegroundProperty);
			}
			set
			{
				SetValue(ForegroundProperty, value);
			}
		}

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			PropertyGridEditorTextBox propertyGridEditorTextBox = new PropertyGridEditorTextBox();
			UpdateProperty(propertyGridEditorTextBox, Control.FontFamilyProperty, FontFamilyProperty);
			UpdateProperty(propertyGridEditorTextBox, Control.FontSizeProperty, FontSizeProperty);
			UpdateProperty(propertyGridEditorTextBox, Control.FontStyleProperty, FontStyleProperty);
			UpdateProperty(propertyGridEditorTextBox, Control.FontWeightProperty, FontWeightProperty);
			UpdateProperty(propertyGridEditorTextBox, Control.ForegroundProperty, ForegroundProperty);
			UpdateStyle(propertyGridEditorTextBox);
			UpdateBinding(propertyGridEditorTextBox, TextBox.TextProperty, propertyItem);
			return propertyGridEditorTextBox;
		}
	}
}
