using System;
using System.Collections.Generic;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	public class EditorCollectionDefinition : EditorBoundDefinition
	{
		public static readonly DependencyProperty IsReadOnlyProperty = CollectionControlButton.IsReadOnlyProperty.AddOwner(typeof(EditorCollectionDefinition));

		public static readonly DependencyProperty ItemsSourceTypeProperty = CollectionControlButton.ItemsSourceTypeProperty.AddOwner(typeof(EditorCollectionDefinition));

		public static readonly DependencyProperty NewItemTypesProperty = CollectionControlButton.NewItemTypesProperty.AddOwner(typeof(EditorCollectionDefinition));

		public bool IsReadOnly
		{
			get
			{
				return (bool)GetValue(IsReadOnlyProperty);
			}
			set
			{
				SetValue(IsReadOnlyProperty, value);
			}
		}

		public Type ItemsSourceType
		{
			get
			{
				return (Type)GetValue(ItemsSourceTypeProperty);
			}
			set
			{
				SetValue(ItemsSourceTypeProperty, value);
			}
		}

		public IList<Type> NewItemTypes
		{
			get
			{
				return (IList<Type>)GetValue(NewItemTypesProperty);
			}
			set
			{
				SetValue(NewItemTypesProperty, value);
			}
		}

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			PropertyGridEditorCollectionControl propertyGridEditorCollectionControl = new PropertyGridEditorCollectionControl();
			UpdateProperty(propertyGridEditorCollectionControl, CollectionControlButton.ItemsSourceTypeProperty, ItemsSourceTypeProperty);
			UpdateProperty(propertyGridEditorCollectionControl, CollectionControlButton.NewItemTypesProperty, NewItemTypesProperty);
			UpdateProperty(propertyGridEditorCollectionControl, CollectionControlButton.IsReadOnlyProperty, IsReadOnlyProperty);
			UpdateStyle(propertyGridEditorCollectionControl);
			UpdateBinding(propertyGridEditorCollectionControl, CollectionControlButton.ItemsSourceProperty, propertyItem);
			return propertyGridEditorCollectionControl;
		}
	}
}
