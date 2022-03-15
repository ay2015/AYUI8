using System;
using System.Windows;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a date-time up-down editor in the PropertyGrid.</summary>
	public class EditorDateTimeUpDownDefinition : EditorUpDownDefinitionBase<DateTimeUpDown, DateTime?>
	{
		public static readonly DependencyProperty FormatProperty = DateTimeUpDown.FormatProperty.AddOwner(typeof(EditorDateTimeUpDownDefinition));

		public static readonly DependencyProperty FormatStringProperty = DateTimeUpDown.FormatStringProperty.AddOwner(typeof(EditorDateTimeUpDownDefinition));

		public DateTimeFormat Format
		{
			get
			{
				return (DateTimeFormat)GetValue(FormatProperty);
			}
			set
			{
				SetValue(FormatProperty, value);
			}
		}

		public string FormatString
		{
			get
			{
				return (string)GetValue(FormatStringProperty);
			}
			set
			{
				SetValue(FormatStringProperty, value);
			}
		}

		protected override DateTimeUpDown CreateEditor()
		{
			return new PropertyGridEditorDateTimeUpDown();
		}

		internal override void InitializeUpDownEditor(UpDownBase<DateTime?> editor)
		{
			base.InitializeUpDownEditor(editor);
			DateTimeUpDown element = (DateTimeUpDown)editor;
			UpdateProperty(element, DateTimeUpDown.FormatStringProperty, FormatStringProperty);
			UpdateProperty(element, DateTimeUpDown.FormatProperty, FormatProperty);
		}
	}
}
