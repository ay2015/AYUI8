using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	public class EditorFilePickerDefinition : EditorBoundDefinition
	{
		public static readonly DependencyProperty FilterProperty = FilePicker.FilterProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(""));

		public static readonly DependencyProperty MultiSelectProperty = FilePicker.MultiSelectProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(false));

		public static readonly DependencyProperty SelectedFileProperty = FilePicker.SelectedFileProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(""));

		public static readonly DependencyProperty SelectedFilesProperty = FilePicker.SelectedFilesProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(null));

		public static readonly DependencyProperty InitialDirectoryProperty = FilePicker.InitialDirectoryProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(""));

		public static readonly DependencyProperty IsOpenProperty = FilePicker.IsOpenProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(false));

		public static readonly DependencyProperty TitleProperty = FilePicker.TitleProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(""));

		public static readonly DependencyProperty WatermarkProperty = FilePicker.WatermarkProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(null));

		public static readonly DependencyProperty WatermarkTemplateProperty = FilePicker.WatermarkTemplateProperty.AddOwner(typeof(EditorFilePickerDefinition), new UIPropertyMetadata(null));

		public string Filter
		{
			get
			{
				return (string)GetValue(FilterProperty);
			}
			set
			{
				SetValue(FilterProperty, value);
			}
		}

		public bool MultiSelect
		{
			get
			{
				return (bool)GetValue(MultiSelectProperty);
			}
			set
			{
				SetValue(MultiSelectProperty, value);
			}
		}

		public string SelectedFile
		{
			get
			{
				return (string)GetValue(SelectedFileProperty);
			}
			set
			{
				SetValue(SelectedFileProperty, value);
			}
		}

		public ObservableCollection<string> SelectedFiles
		{
			get
			{
				return (ObservableCollection<string>)GetValue(SelectedFilesProperty);
			}
			set
			{
				SetValue(SelectedFilesProperty, value);
			}
		}

		public string InitialDirectory
		{
			get
			{
				return (string)GetValue(InitialDirectoryProperty);
			}
			set
			{
				SetValue(InitialDirectoryProperty, value);
			}
		}

		public bool IsOpen
		{
			get
			{
				return (bool)GetValue(IsOpenProperty);
			}
			set
			{
				SetValue(IsOpenProperty, value);
			}
		}

		public string Title
		{
			get
			{
				return (string)GetValue(TitleProperty);
			}
			set
			{
				SetValue(TitleProperty, value);
			}
		}

		public object Watermark
		{
			get
			{
				return GetValue(WatermarkProperty);
			}
			set
			{
				SetValue(WatermarkProperty, value);
			}
		}

		public DataTemplate WatermarkTemplate
		{
			get
			{
				return (DataTemplate)GetValue(WatermarkTemplateProperty);
			}
			set
			{
				SetValue(WatermarkTemplateProperty, value);
			}
		}

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			PropertyGridEditorFilePicker propertyGridEditorFilePicker = new PropertyGridEditorFilePicker();
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.FilterProperty, FilterProperty);
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.MultiSelectProperty, MultiSelectProperty);
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.SelectedFileProperty, SelectedFileProperty);
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.SelectedFilesProperty, SelectedFilesProperty);
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.InitialDirectoryProperty, InitialDirectoryProperty);
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.IsOpenProperty, IsOpenProperty);
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.TitleProperty, TitleProperty);
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.WatermarkProperty, WatermarkProperty);
			UpdateProperty(propertyGridEditorFilePicker, FilePicker.WatermarkTemplateProperty, WatermarkTemplateProperty);
			UpdateStyle(propertyGridEditorFilePicker);
			Binding binding = (Binding)PropertyGridUtilities.GetDefaultBinding(propertyItem);
			binding.Converter = new FileInfoToStringConverter();
			base.Binding = binding;
			UpdateBinding(propertyGridEditorFilePicker, FilePicker.SelectedFileProperty, propertyItem);
			return propertyGridEditorFilePicker;
		}
	}
}
