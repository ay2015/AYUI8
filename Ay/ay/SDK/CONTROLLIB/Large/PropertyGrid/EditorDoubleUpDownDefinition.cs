using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a double up-down editor in the PropertyGrid.
	///
	/// <para></para></summary>
	public class EditorDoubleUpDownDefinition : EditorNumericUpDownDefinitionBase<DoubleUpDown, double>
	{
		public AllowedSpecialValues AllowInputSpecialValues
		{
			get;
			set;
		}

		static EditorDoubleUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<DoubleUpDown, double>.UpdateMetadata(typeof(EditorDoubleUpDownDefinition), 1.0, double.NegativeInfinity, double.PositiveInfinity);
		}

		protected override DoubleUpDown CreateEditor()
		{
			return new PropertyGridEditorDoubleUpDown();
		}

		public EditorDoubleUpDownDefinition()
		{
			AllowInputSpecialValues = AllowedSpecialValues.Any;
		}

		internal override void InitializeUpDownEditor(UpDownBase<double?> editor)
		{
			base.InitializeUpDownEditor(editor);
			DoubleUpDown doubleUpDown = (DoubleUpDown)editor;
			doubleUpDown.AllowInputSpecialValues = AllowInputSpecialValues;
		}
	}
}
