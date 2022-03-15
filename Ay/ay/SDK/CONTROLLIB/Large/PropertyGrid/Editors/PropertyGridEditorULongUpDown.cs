using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in ULongUpDown editor used in the PropertyGrid.</summary>
	
	public class PropertyGridEditorULongUpDown : ULongUpDown
	{
		static PropertyGridEditorULongUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorULongUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorULongUpDown)));
		}
	}
}
