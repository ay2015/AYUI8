using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in PrimitiveTypeCollectionControl editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorPrimitiveTypeCollectionControl : PrimitiveTypeCollectionControl
	{
		static PropertyGridEditorPrimitiveTypeCollectionControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorPrimitiveTypeCollectionControl), new FrameworkPropertyMetadata(typeof(PropertyGridEditorPrimitiveTypeCollectionControl)));
		}
	}
}
