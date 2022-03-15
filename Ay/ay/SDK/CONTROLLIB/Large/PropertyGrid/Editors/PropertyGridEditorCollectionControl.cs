using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in CollectionControl editor used in the PropertyGrid.</summary>
	public class PropertyGridEditorCollectionControl : CollectionControlButton
	{
		static PropertyGridEditorCollectionControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridEditorCollectionControl), new FrameworkPropertyMetadata(typeof(PropertyGridEditorCollectionControl)));
		}
	}
}
