using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class ObjectContainerHelper : ObjectContainerHelperBase
	{
		private object _selectedObject;

		private object SelectedObject
		{
			get
			{
				return _selectedObject;
			}
		}

		public ObjectContainerHelper(IPropertyContainer propertyContainer, object selectedObject)
			: base(propertyContainer)
		{
			_selectedObject = selectedObject;
		}

		protected override string GetDefaultPropertyName()
		{
			object selectedObject = SelectedObject;
			if (selectedObject == null)
			{
				return null;
			}
			return ObjectContainerHelperBase.GetDefaultPropertyName(SelectedObject);
		}

		protected override void GenerateSubPropertiesCore(Action<IEnumerable<PropertyItem>> updatePropertyItemsCallback)
		{
			List<PropertyItem> list = new List<PropertyItem>();
			if (SelectedObject != null)
			{
				try
				{
					List<PropertyDescriptor> list2 = new List<PropertyDescriptor>();
					if (IsExpandingNonPrimitiveTypes() && SelectedObject.GetType().IsGenericType && SelectedObject.GetType().GetGenericTypeDefinition() != typeof(KeyValuePair<, >))
					{
						foreach (object item in (IEnumerable)SelectedObject)
						{
							list2.Add(new CollectionPropertyDescriptor((IEnumerable)SelectedObject, item));
						}
					}
					else
					{
						list2 = ObjectContainerHelperBase.GetPropertyDescriptors(SelectedObject, PropertyContainer.HideInheritedProperties);
					}
					foreach (PropertyDescriptor item2 in list2)
					{
						PropertyDefinition propertyDefinition = GetPropertyDefinition(item2);
						bool flag = false;
						bool? flag2 = PropertyContainer.IsPropertyVisible(item2);
						if (flag2.HasValue)
						{
							flag = flag2.Value;
						}
						else
						{
							DisplayAttribute attribute = PropertyGridUtilities.GetAttribute<DisplayAttribute>(item2);
							if (attribute != null)
							{
								bool? autoGenerateField = attribute.GetAutoGenerateField();
								flag = (PropertyContainer.AutoGenerateProperties && ((autoGenerateField.HasValue && autoGenerateField.Value) || !autoGenerateField.HasValue));
							}
							else
							{
								flag = (item2.IsBrowsable && PropertyContainer.AutoGenerateProperties);
							}
							if (propertyDefinition != null)
							{
								flag = propertyDefinition.IsBrowsable.GetValueOrDefault(flag);
							}
						}
						if (flag)
						{
							PropertyItem propertyItem = CreatePropertyItem(item2, propertyDefinition);
							if (propertyItem != null)
							{
								list.Add(propertyItem);
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			updatePropertyItemsCallback(list);
		}

		protected internal override bool ShouldRegenerateProperties()
		{
			if (IsExpandingNonPrimitiveTypes())
			{
				return SelectedObject.GetType().IsGenericType;
			}
			return false;
		}

		private PropertyItem CreatePropertyItem(PropertyDescriptor property, PropertyDefinition propertyDef)
		{
			DescriptorPropertyDefinition descriptorPropertyDefinition = new DescriptorPropertyDefinition(property, SelectedObject, PropertyContainer);
			descriptorPropertyDefinition.InitProperties();
			if (!IsCategoryBrowsable(descriptorPropertyDefinition.CategoryValue))
			{
				return null;
			}
			InitializeDescriptorDefinition(descriptorPropertyDefinition, propertyDef);
			PropertyItem propertyItem = new PropertyItem(descriptorPropertyDefinition);
			propertyItem.Instance = SelectedObject;
			propertyItem.CategoryOrder = GetCategoryOrder(descriptorPropertyDefinition.CategoryValue);
			propertyItem.IsCategoryExpanded = GetIsCategoryExpanded(descriptorPropertyDefinition.CategoryValue);
			propertyItem.IsExpanded = GetIsExpanded(descriptorPropertyDefinition.PropertyDescriptor);
			propertyItem.WillRefreshPropertyGrid = GetWillRefreshPropertyGrid(property);
			return propertyItem;
		}

		private int GetCategoryOrder(object categoryValue)
		{
			if (categoryValue == null)
			{
				return 2147483647;
			}
			int result = 2147483647;
			CategoryDefinition categoryDefinition = GetCategoryDefinition(categoryValue);
			if (categoryDefinition != null && categoryDefinition.DisplayOrder.HasValue)
			{
				result = categoryDefinition.DisplayOrder.Value;
			}
			else
			{
				object selectedObject = SelectedObject;
				CategoryOrderAttribute[] source = (selectedObject != null) ? ((CategoryOrderAttribute[])selectedObject.GetType().GetCustomAttributes(typeof(CategoryOrderAttribute), true)) : new CategoryOrderAttribute[0];
				CategoryOrderAttribute categoryOrderAttribute = source.FirstOrDefault((CategoryOrderAttribute a) => object.Equals(a.CategoryValue, categoryValue));
				if (categoryOrderAttribute != null)
				{
					result = categoryOrderAttribute.Order;
				}
			}
			return result;
		}

		private bool IsCategoryBrowsable(object categoryValue)
		{
			if (categoryValue == null)
			{
				return true;
			}
			CategoryDefinition categoryDefinition = GetCategoryDefinition(categoryValue);
			if (categoryDefinition != null)
			{
				return categoryDefinition.IsBrowsable;
			}
			return true;
		}

		private bool GetIsCategoryExpanded(object categoryValue)
		{
			if (categoryValue == null)
			{
				return true;
			}
			CategoryDefinition categoryDefinition = GetCategoryDefinition(categoryValue);
			if (categoryDefinition != null)
			{
				return categoryDefinition.IsExpanded;
			}
			object selectedObject = SelectedObject;
			ExpandedCategoryAttribute[] source = (selectedObject != null) ? ((ExpandedCategoryAttribute[])selectedObject.GetType().GetCustomAttributes(typeof(ExpandedCategoryAttribute), true)) : new ExpandedCategoryAttribute[0];
			ExpandedCategoryAttribute expandedCategoryAttribute = source.FirstOrDefault((ExpandedCategoryAttribute a) => object.Equals(a.CategoryValue, categoryValue));
			if (expandedCategoryAttribute != null)
			{
				return expandedCategoryAttribute.IsExpanded;
			}
			return true;
		}
	}
}
