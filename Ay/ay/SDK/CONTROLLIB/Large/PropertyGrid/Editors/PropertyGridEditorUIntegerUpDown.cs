using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in UIntegerUpDown editor used in the PropertyGrid.</summary>
	
	public class PropertyGridEditorUIntegerUpDown : UIntegerUpDown
	{
		static PropertyGridEditorUIntegerUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorUIntegerUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorUIntegerUpDown)));
		}
	}
}
