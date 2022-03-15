using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a decimal up-down editor in the PropertyGrid.</summary>
	public class EditorDecimalUpDownDefinition : EditorNumericUpDownDefinitionBase<DecimalUpDown, decimal>
	{
		static EditorDecimalUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<DecimalUpDown, decimal>.UpdateMetadata(typeof(EditorDecimalUpDownDefinition), 1m, decimal.MinValue, decimal.MaxValue);
		}

		protected override DecimalUpDown CreateEditor()
		{
			return new PropertyGridEditorDecimalUpDown();
		}
	}
}
