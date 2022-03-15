using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class EditorSByteUpDownDefinition : EditorNumericUpDownDefinitionBase<SByteUpDown, sbyte>
	{
		static EditorSByteUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<SByteUpDown, sbyte>.UpdateMetadata(typeof(EditorSByteUpDownDefinition), 1, sbyte.MinValue, sbyte.MaxValue);
		}

		protected override SByteUpDown CreateEditor()
		{
			return new PropertyGridEditorSByteUpDown();
		}
	}
}
