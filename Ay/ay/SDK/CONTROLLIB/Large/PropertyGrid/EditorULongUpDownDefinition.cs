using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class EditorULongUpDownDefinition : EditorNumericUpDownDefinitionBase<ULongUpDown, ulong>
	{
		static EditorULongUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<ULongUpDown, ulong>.UpdateMetadata(typeof(EditorULongUpDownDefinition), 1uL, 0uL, ulong.MaxValue);
		}

		protected override ULongUpDown CreateEditor()
		{
			return new PropertyGridEditorULongUpDown();
		}
	}
}
