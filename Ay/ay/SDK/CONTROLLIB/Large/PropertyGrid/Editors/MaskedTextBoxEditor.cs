using ay.Controls;
using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in MaskedTextBox editor.</summary>
	public class MaskedTextBoxEditor : TypeEditor<MaskedTextBox>
	{
		public string Mask
		{
			get;
			set;
		}

		public Type ValueDataType
		{
			get;
			set;
		}

		protected override MaskedTextBox CreateEditor()
		{
			return new PropertyGridEditorMaskedTextBox();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.Editor.BorderThickness = new Thickness(0.0);
			base.Editor.ValueDataType = ValueDataType;
			base.Editor.Mask = Mask;
		}

		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = ValueRangeTextBox.ValueProperty;
		}
	}
}
