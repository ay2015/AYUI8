using System;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	public class EditorPrimitiveTypeCollectionDefinition : EditorBoundDefinition
	{
		public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(EditorPrimitiveTypeCollectionDefinition));

		public static readonly DependencyProperty IsReadOnlyProperty = PrimitiveTypeCollectionControl.IsReadOnlyProperty.AddOwner(typeof(EditorPrimitiveTypeCollectionDefinition));

		public static readonly DependencyProperty ItemsSourceTypeProperty = PrimitiveTypeCollectionControl.ItemsSourceTypeProperty.AddOwner(typeof(EditorPrimitiveTypeCollectionDefinition));

		public string Content
		{
			get
			{
				return (string)GetValue(ContentProperty);
			}
			set
			{
				SetValue(ContentProperty, value);
			}
		}

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

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			PropertyGridEditorPrimitiveTypeCollectionControl propertyGridEditorPrimitiveTypeCollectionControl = new PropertyGridEditorPrimitiveTypeCollectionControl();
			UpdateProperty(propertyGridEditorPrimitiveTypeCollectionControl, PrimitiveTypeCollectionControl.ItemsSourceTypeProperty, ItemsSourceTypeProperty);
			UpdateProperty(propertyGridEditorPrimitiveTypeCollectionControl, ContentControl.ContentProperty, ContentProperty);
			UpdateProperty(propertyGridEditorPrimitiveTypeCollectionControl, PrimitiveTypeCollectionControl.IsReadOnlyProperty, IsReadOnlyProperty);
			UpdateStyle(propertyGridEditorPrimitiveTypeCollectionControl);
			UpdateBinding(propertyGridEditorPrimitiveTypeCollectionControl, PrimitiveTypeCollectionControl.ItemsSourceProperty, propertyItem);
			return propertyGridEditorPrimitiveTypeCollectionControl;
		}
	}
}
