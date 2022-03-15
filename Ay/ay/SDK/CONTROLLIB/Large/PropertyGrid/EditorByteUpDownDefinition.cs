using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>
	///   <para>Allows use of a byte up-down editor in the PropertyGrid.</para>
	/// </summary>
	public class EditorByteUpDownDefinition : EditorNumericUpDownDefinitionBase<ByteUpDown, byte>
	{
		static EditorByteUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<ByteUpDown, byte>.UpdateMetadata(typeof(EditorByteUpDownDefinition), 1, 0, byte.MaxValue);
		}

		protected override ByteUpDown CreateEditor()
		{
			return new PropertyGridEditorByteUpDown();
		}
	}
}
