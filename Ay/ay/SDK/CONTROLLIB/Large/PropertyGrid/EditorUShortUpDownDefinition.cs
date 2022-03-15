using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class EditorUShortUpDownDefinition : EditorNumericUpDownDefinitionBase<UShortUpDown, ushort>
	{
		static EditorUShortUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<UShortUpDown, ushort>.UpdateMetadata(typeof(EditorUShortUpDownDefinition), 1, 0, ushort.MaxValue);
		}

		protected override UShortUpDown CreateEditor()
		{
			return new PropertyGridEditorUShortUpDown();
		}
	}
}
