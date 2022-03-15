using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a short up-down editor in the PropertyGrid.
	///
	/// <para>See EditorDefinitionBase for details.</para></summary>
	public class EditorShortUpDownDefinition : EditorNumericUpDownDefinitionBase<ShortUpDown, short>
	{
		static EditorShortUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<ShortUpDown, short>.UpdateMetadata(typeof(EditorShortUpDownDefinition), 1, short.MinValue, short.MaxValue);
		}

		protected override ShortUpDown CreateEditor()
		{
			return new PropertyGridEditorShortUpDown();
		}
	}
}
