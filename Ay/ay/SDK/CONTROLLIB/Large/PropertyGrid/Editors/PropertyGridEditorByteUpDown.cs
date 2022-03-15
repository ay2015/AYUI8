using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in ByteUpDown editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorByteUpDown : ByteUpDown
	{
		static PropertyGridEditorByteUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorByteUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorByteUpDown)));
		}
	}
}
