using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a long up-down editor in the PropertyGrid.</summary>
	public class EditorLongUpDownDefinition : EditorNumericUpDownDefinitionBase<LongUpDown, long>
	{
		static EditorLongUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<LongUpDown, long>.UpdateMetadata(typeof(EditorLongUpDownDefinition), 1L, -9223372036854775808L, 9223372036854775807L);
		}

		protected override LongUpDown CreateEditor()
		{
			return new PropertyGridEditorLongUpDown();
		}
	}
}
