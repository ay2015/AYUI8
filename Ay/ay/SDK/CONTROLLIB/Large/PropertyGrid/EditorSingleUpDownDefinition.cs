using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a single up-down editor in the PropertyGrid.</summary>
	public class EditorSingleUpDownDefinition : EditorNumericUpDownDefinitionBase<SingleUpDown, float>
	{
		public AllowedSpecialValues AllowInputSpecialValues
		{
			get;
			set;
		}

		static EditorSingleUpDownDefinition()
		{
			EditorNumericUpDownDefinitionBase<SingleUpDown, float>.UpdateMetadata(typeof(EditorSingleUpDownDefinition), 1f, float.NegativeInfinity, float.PositiveInfinity);
		}

		protected override SingleUpDown CreateEditor()
		{
			return new PropertyGridEditorSingleUpDown();
		}

		public EditorSingleUpDownDefinition()
		{
			AllowInputSpecialValues = AllowedSpecialValues.Any;
		}

		internal override void InitializeUpDownEditor(UpDownBase<float?> editor)
		{
			base.InitializeUpDownEditor(editor);
			SingleUpDown singleUpDown = (SingleUpDown)editor;
			singleUpDown.AllowInputSpecialValues = AllowInputSpecialValues;
		}
	}
}
