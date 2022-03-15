using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class EditorUIntegerUpDownDefinition : EditorNumericUpDownDefinitionBase<UIntegerUpDown, uint>
	{
		static EditorUIntegerUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<UIntegerUpDown, uint>.UpdateMetadata(typeof(EditorUIntegerUpDownDefinition), 1u, 0u, uint.MaxValue);
		}

		protected override UIntegerUpDown CreateEditor()
		{
			return new PropertyGridEditorUIntegerUpDown();
		}
	}
}
