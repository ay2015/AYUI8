using System;
using System.Collections.Generic;
using System.Windows;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in Collection editor.</summary>
	public class CollectionEditor : TypeEditor<CollectionControlButton>
	{
		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = CollectionControlButton.ItemsSourceProperty;
		}

		protected override CollectionControlButton CreateEditor()
		{
			return new PropertyGridEditorCollectionControl();
		}

		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			if (propertyItem != null && ((IPropertyContainer)propertyItem).IsExpandingNonPrimitiveTypes && propertyItem.PropertyType.IsGenericType)
			{
				base.Editor.Click += Editor_Click;
			}
			PropertyGrid propertyGrid = propertyItem.ParentElement as PropertyGrid;
			if (propertyGrid != null)
			{
				base.Editor.EditorDefinitions = propertyGrid.EditorDefinitions;
			}
		}

		protected override void ResolveValueBinding(PropertyItem propertyItem)
		{
			Type propertyType = propertyItem.PropertyType;
			base.Editor.ItemsSourceType = propertyType;
			if (propertyType.BaseType == typeof(Array))
			{
				base.Editor.NewItemTypes = new List<Type>
				{
					propertyType.GetElementType()
				};
			}
			else if (propertyItem.DescriptorDefinition != null && propertyItem.DescriptorDefinition.NewItemTypes != null && propertyItem.DescriptorDefinition.NewItemTypes.Count > 0)
			{
				base.Editor.NewItemTypes = propertyItem.DescriptorDefinition.NewItemTypes;
			}
			else
			{
				Type[] dictionaryItemsType = ListUtilities.GetDictionaryItemsType(propertyType);
				if (dictionaryItemsType != null && dictionaryItemsType.Length == 2)
				{
					Type item = ListUtilities.CreateEditableKeyValuePairType(dictionaryItemsType[0], dictionaryItemsType[1]);
					base.Editor.NewItemTypes = new List<Type>
					{
						item
					};
				}
				else
				{
					Type listItemType = ListUtilities.GetListItemType(propertyType);
					if (listItemType != null)
					{
						base.Editor.NewItemTypes = new List<Type>
						{
							listItemType
						};
					}
					else
					{
						Type collectionItemType = ListUtilities.GetCollectionItemType(propertyType);
						if (collectionItemType != null)
						{
							base.Editor.NewItemTypes = new List<Type>
							{
								collectionItemType
							};
						}
					}
				}
			}
			base.ResolveValueBinding(propertyItem);
		}

		private void Editor_Click(object sender, RoutedEventArgs e)
		{
			CollectionControlButton collectionControlButton = sender as CollectionControlButton;
			if (collectionControlButton != null)
			{
				PropertyItemBase propertyItemBase = collectionControlButton.DataContext as PropertyItemBase;
				if (propertyItemBase != null)
				{
					propertyItemBase.IsExpanded = false;
				}
			}
		}
	}
}
