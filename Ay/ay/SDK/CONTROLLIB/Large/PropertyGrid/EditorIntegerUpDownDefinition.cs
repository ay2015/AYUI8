using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of an integer up-down editor in the PropertyGrid.
	///
	/// <para></para></summary>
	public class EditorIntegerUpDownDefinition : EditorNumericUpDownDefinitionBase<IntegerUpDown, int>
	{
		static EditorIntegerUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<IntegerUpDown, int>.UpdateMetadata(typeof(EditorIntegerUpDownDefinition), 1, -2147483648, 2147483647);
		}

		protected override IntegerUpDown CreateEditor()
		{
			return new PropertyGridEditorIntegerUpDown();
		}
	}
}
