using System.Collections.ObjectModel;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a color picker editor in the PropertyGrid.</summary>
	public class EditorColorPickerDefinition : EditorBoundDefinition
	{
		public static readonly DependencyProperty DisplayColorAndNameProperty = ColorPicker.DisplayColorAndNameProperty.AddOwner(typeof(EditorColorPickerDefinition), new UIPropertyMetadata(true));

		/// <summary>Identifies the ShowTabHeaders dependency
		/// property.</summary>
		public static readonly DependencyProperty ShowTabHeadersProperty = ColorPicker.ShowTabHeadersProperty.AddOwner(typeof(EditorColorPickerDefinition), new UIPropertyMetadata(true));

		public static readonly DependencyProperty ShowAvailableColorsProperty = ColorPicker.ShowAvailableColorsProperty.AddOwner(typeof(EditorColorPickerDefinition));

		public static readonly DependencyProperty ShowRecentColorsProperty = ColorPicker.ShowRecentColorsProperty.AddOwner(typeof(EditorColorPickerDefinition));

		public static readonly DependencyProperty ShowStandardColorsProperty = ColorPicker.ShowStandardColorsProperty.AddOwner(typeof(EditorColorPickerDefinition));

		public static readonly DependencyProperty ShowDropDownButtonProperty = ColorPicker.ShowDropDownButtonProperty.AddOwner(typeof(EditorColorPickerDefinition));

		public ObservableCollection<ColorItem> AvailableColors
		{
			get;
			set;
		}

		public bool DisplayColorAndName
		{
			get
			{
				return (bool)GetValue(DisplayColorAndNameProperty);
			}
			set
			{
				SetValue(DisplayColorAndNameProperty, value);
			}
		}

		public ObservableCollection<ColorItem> RecentColors
		{
			get;
			set;
		}

		/// <summary>Gets or sets a value indicating if the TabItems displaying the standard color palette and the color canvas, are visible in the popup.</summary>
		public bool ShowTabHeaders
		{
			get
			{
				return (bool)GetValue(ShowTabHeadersProperty);
			}
			set
			{
				SetValue(ShowTabHeadersProperty, value);
			}
		}

		public bool ShowAvailableColors
		{
			get
			{
				return (bool)GetValue(ShowAvailableColorsProperty);
			}
			set
			{
				SetValue(ShowAvailableColorsProperty, value);
			}
		}

		public bool ShowRecentColors
		{
			get
			{
				return (bool)GetValue(ShowRecentColorsProperty);
			}
			set
			{
				SetValue(ShowRecentColorsProperty, value);
			}
		}

		public bool ShowStandardColors
		{
			get
			{
				return (bool)GetValue(ShowStandardColorsProperty);
			}
			set
			{
				SetValue(ShowStandardColorsProperty, value);
			}
		}

		public bool ShowDropDownButton
		{
			get
			{
				return (bool)GetValue(ShowDropDownButtonProperty);
			}
			set
			{
				SetValue(ShowDropDownButtonProperty, value);
			}
		}

		public ObservableCollection<ColorItem> StandardColors
		{
			get;
			set;
		}

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			PropertyGridEditorColorPicker propertyGridEditorColorPicker = new PropertyGridEditorColorPicker();
			if (AvailableColors != null)
			{
				propertyGridEditorColorPicker.AvailableColors = AvailableColors;
			}
			if (RecentColors != null)
			{
				propertyGridEditorColorPicker.RecentColors = RecentColors;
			}
			if (StandardColors != null)
			{
				propertyGridEditorColorPicker.StandardColors = StandardColors;
			}
			UpdateProperty(propertyGridEditorColorPicker, ColorPicker.DisplayColorAndNameProperty, DisplayColorAndNameProperty);
			UpdateProperty(propertyGridEditorColorPicker, ColorPicker.ShowTabHeadersProperty, ShowTabHeadersProperty);
			UpdateProperty(propertyGridEditorColorPicker, ColorPicker.ShowAvailableColorsProperty, ShowAvailableColorsProperty);
			UpdateProperty(propertyGridEditorColorPicker, ColorPicker.ShowDropDownButtonProperty, ShowDropDownButtonProperty);
			UpdateProperty(propertyGridEditorColorPicker, ColorPicker.ShowRecentColorsProperty, ShowRecentColorsProperty);
			UpdateProperty(propertyGridEditorColorPicker, ColorPicker.ShowStandardColorsProperty, ShowStandardColorsProperty);
			UpdateStyle(propertyGridEditorColorPicker);
			UpdateBinding(propertyGridEditorColorPicker, ColorPicker.SelectedColorProperty, propertyItem);
			return propertyGridEditorColorPicker;
		}
	}
}
