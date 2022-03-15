using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in <strong>FilePicker</strong> editor.</summary>
	public class FileEditor : TypeEditor<FilePicker>
	{
		protected override FilePicker CreateEditor()
		{
			return new PropertyGridEditorFilePicker();
		}

		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = FilePicker.SelectedFileProperty;
		}

		protected override IValueConverter CreateValueConverter()
		{
			return new FileInfoToStringConverter();
		}
	}
}
