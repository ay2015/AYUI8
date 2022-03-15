using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in SByteUpDown editor used in the PropertyGrid.</summary>
	
	public class PropertyGridEditorSByteUpDown : SByteUpDown
	{
		static PropertyGridEditorSByteUpDown()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorSByteUpDown), new FrameworkPropertyMetadata(typeof(PropertyGridEditorSByteUpDown)));
		}
	}
}
