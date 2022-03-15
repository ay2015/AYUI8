using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>
	///   <font size="2">This class will return the style to use for Category groups in the PropertyGrid. The possible returned styles are identified by the class
	/// properties.</font>
	/// </summary>
	public class CategoryGroupStyleSelector : StyleSelector
	{
		/// <summary>
		///   <font size="2">Gets/Sets the style to use when only the default category ("Misc") exists.</font>
		/// </summary>
		public Style SingleDefaultCategoryItemGroupStyle
		{
			get;
			set;
		}

		/// <summary>
		///   <font size="2">Gets/Sets the style to use when the default Category ("Misc") doesn't exists or when there is more than one category.</font>
		/// </summary>
		public Style ItemGroupStyle
		{
			get;
			set;
		}

		public override Style SelectStyle(object item, DependencyObject container)
		{
			CollectionViewGroup collectionViewGroup = item as CollectionViewGroup;
			if (collectionViewGroup.Name != null && !collectionViewGroup.Name.Equals(CategoryAttribute.Default.Category))
			{
				return ItemGroupStyle;
			}
			while (container != null)
			{
				container = VisualTreeHelper.GetParent(container);
				if (container is ItemsControl)
				{
					break;
				}
			}
			ItemsControl itemsControl = container as ItemsControl;
			if (itemsControl != null && itemsControl.Items.Count > 0 && itemsControl.Items.Groups.Count == 1)
			{
				return SingleDefaultCategoryItemGroupStyle;
			}
			return ItemGroupStyle;
		}
	}
}
