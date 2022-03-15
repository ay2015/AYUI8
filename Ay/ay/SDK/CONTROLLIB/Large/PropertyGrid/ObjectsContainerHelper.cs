using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class ObjectsContainerHelper : ObjectContainerHelperBase
	{
		private static Guid _currentPropertiesGeneratorID;

		private IEnumerable _selectedObjects;

		private List<DispatcherOperation> _propertiesGenerationDispatcherList;

		private double _initialScrollPosition;

		private IEnumerable<object> SelectedObjects
		{
			get
			{
				return _selectedObjects.Cast<object>();
			}
		}

		public ObjectsContainerHelper(IPropertyContainer propertyContainer, IEnumerable selectedObjects)
			: base(propertyContainer)
		{
			if (selectedObjects == null)
			{
				throw new ArgumentNullException("selectedObjects");
			}
			_selectedObjects = selectedObjects;
			_propertiesGenerationDispatcherList = new List<DispatcherOperation>();
		}

		public override void ClearHelper()
		{
			base.ClearHelper();
			CleanPropertiesGenerationDispatcher();
		}

		protected override string GetDefaultPropertyName()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(SelectedObjects, (object x) => ObjectContainerHelperBase.GetDefaultPropertyName(x), out result);
			if (result == null)
			{
				return null;
			}
			return result as string;
		}

		protected override void GenerateSubPropertiesCore(Action<IEnumerable<PropertyItem>> updatePropertyItemsCallback)
		{
			_currentPropertiesGeneratorID = Guid.NewGuid();
			List<PropertyItem> finalPropertyItemsList = new List<PropertyItem>();
			List<List<PropertyDescriptor>> propertyDescriptorsList = GetPropertyDescriptorsList();
			PropertyGrid propertyGrid = PropertyContainer as PropertyGrid;
			_initialScrollPosition = ((propertyGrid != null && !propertyGrid.IsScrollingToTopAfterRefresh) ? propertyGrid.GetScrollPosition() : 0.0);
			GenerateSubPropertiesCoreWithDispatcher(propertyDescriptorsList, 0, finalPropertyItemsList, updatePropertyItemsCallback, _currentPropertiesGeneratorID);
		}

		private void GenerateSubPropertiesCoreWithDispatcher(List<List<PropertyDescriptor>> commonDescriptorsLists, int index, List<PropertyItem> finalPropertyItemsList, Action<IEnumerable<PropertyItem>> updatePropertyItemsCallback, Guid propertiesGeneratorID)
		{
			if (propertiesGeneratorID != _currentPropertiesGeneratorID)
			{
				CleanPropertiesGenerationDispatcher();
			}
			else
			{
				Control control = PropertyContainer as Control;
				if (index >= commonDescriptorsLists.Count)
				{
					updatePropertyItemsCallback(finalPropertyItemsList);
				}
				else
				{
					List<PropertyDescriptor> list = commonDescriptorsLists[index];
					try
					{
						object result;
						if (PropertyGridUtilities.IsSameForAllObject(list, (object o) => ((PropertyDescriptor)o).PropertyType, out result))
						{
							PropertyDescriptor descriptor = list.First();
							PropertyDefinition propertyDefinition = GetPropertyDefinition(descriptor);
							bool flag = false;
							bool flag2 = false;
							object result2;
							if (PropertyGridUtilities.IsSameForAllObject(list, (object o) => PropertyContainer.IsPropertyVisible((PropertyDescriptor)o), out result2))
							{
								flag2 = (((bool?)result2).HasValue && ((bool?)result2).Value);
							}
							if (!((bool?)result2).HasValue)
							{
								if (PropertyContainer.AutoGenerateProperties)
								{
									bool flag3 = false;
									if (PropertyGridUtilities.IsSameForAllObject(list, (object o) => PropertyGridUtilities.GetAttribute<DisplayAttribute>((PropertyDescriptor)o) != null, out result2))
									{
										flag3 = (bool)result2;
									}
									if (flag3)
									{
										if (PropertyGridUtilities.IsSameForAllObject(list, delegate(object o)
										{
											DisplayAttribute attribute = PropertyGridUtilities.GetAttribute<DisplayAttribute>((PropertyDescriptor)o);
											if (attribute != null)
											{
												bool? autoGenerateField = attribute.GetAutoGenerateField();
												return (autoGenerateField.HasValue && autoGenerateField.Value) || !autoGenerateField.HasValue;
											}
											return true;
										}, out result2))
										{
											flag2 = (bool)result2;
										}
										flag = true;
									}
									if (!flag && PropertyGridUtilities.IsSameForAllObject(list, (object o) => ((PropertyDescriptor)o).IsBrowsable, out result2))
									{
										flag2 = (bool)result2;
									}
								}
								if (propertyDefinition != null)
								{
									flag2 = propertyDefinition.IsBrowsable.GetValueOrDefault(flag2);
								}
							}
							if (flag2)
							{
								PropertyItem propertyItem = CreateCommonPropertyItem(list, propertyDefinition);
								if (propertyItem != null)
								{
									finalPropertyItemsList.Add(propertyItem);
								}
							}
						}
					}
					catch (Exception)
					{
						updatePropertyItemsCallback(finalPropertyItemsList);
					}
					DispatcherOperation item = control.Dispatcher.BeginInvoke((Action)delegate
					{
						GenerateSubPropertiesCoreWithDispatcher(commonDescriptorsLists, index + 1, finalPropertyItemsList, updatePropertyItemsCallback, propertiesGeneratorID);
					}, DispatcherPriority.Input);
					_propertiesGenerationDispatcherList.Add(item);
				}
			}
		}

		private void CleanPropertiesGenerationDispatcher()
		{
			if (_propertiesGenerationDispatcherList != null)
			{
				foreach (DispatcherOperation propertiesGenerationDispatcher in _propertiesGenerationDispatcherList)
				{
					propertiesGenerationDispatcher.Abort();
				}
				_propertiesGenerationDispatcherList.Clear();
			}
		}

		private List<PropertyDescriptor> GetPropertyDescriptors(object instance)
		{
			if (IsExpandingNonPrimitiveTypes() && instance.GetType().IsGenericType)
			{
				List<PropertyDescriptor> list = new List<PropertyDescriptor>();
				{
					foreach (object item in (IEnumerable)instance)
					{
						list.Add(new CollectionPropertyDescriptor((IEnumerable)instance, item));
					}
					return list;
				}
			}
			return ObjectContainerHelperBase.GetPropertyDescriptors(instance, PropertyContainer.HideInheritedProperties);
		}

		private List<List<PropertyDescriptor>> GetPropertyDescriptorsList()
		{
			new List<PropertyDescriptorCollection>();
			List<List<PropertyDescriptor>> list = SelectedObjects.Select(GetPropertyDescriptors).ToList();
			int count = list.Count;
			List<List<PropertyDescriptor>> list2 = new List<List<PropertyDescriptor>>();
			foreach (PropertyDescriptor item in list.First())
			{
				List<List<PropertyDescriptor>> source = list;
				Func<List<PropertyDescriptor>, bool> predicate = (List<PropertyDescriptor> x) => x.Contains(item);
				if (source.All(predicate))
				{
					list2.Add(list.Select(delegate(List<PropertyDescriptor> dList)
					{
						PropertyDescriptor propertyDescriptor = item;
						return dList.Find(((object)propertyDescriptor).Equals);
					}).ToList());
				}
			}
			return list2;
		}

		protected internal override bool ShouldRegenerateProperties()
		{
			if (IsExpandingNonPrimitiveTypes())
			{
				return SelectedObjects.First().GetType().IsGenericType;
			}
			return false;
		}

		protected internal override void UpdatePropertyItemsCallback(IEnumerable<PropertyItem> subProperties)
		{
			base.UpdatePropertyItemsCallback(subProperties);
			PropertyGrid propertyGrid = PropertyContainer as PropertyGrid;
			if (propertyGrid != null && !propertyGrid.IsScrollingToTopAfterRefresh)
			{
				propertyGrid.ScrollToPosition(_initialScrollPosition);
			}
		}

		private PropertyItem CreateCommonPropertyItem(List<PropertyDescriptor> propertyList, PropertyDefinition propertyDef)
		{
			CommonDescriptorPropertyDefinition commonDescriptorPropertyDefinition = new CommonDescriptorPropertyDefinition(propertyList, SelectedObjects.ToList(), PropertyContainer);
			commonDescriptorPropertyDefinition.InitProperties();
			if (!IsCategoryBrowsable(commonDescriptorPropertyDefinition.CategoryValue))
			{
				return null;
			}
			InitializeDescriptorDefinition(commonDescriptorPropertyDefinition, propertyDef);
			PropertyItem propertyItem = new PropertyItem(commonDescriptorPropertyDefinition);
			propertyItem.CategoryOrder = GetCategoryOrder(commonDescriptorPropertyDefinition.CategoryValue);
			propertyItem.IsCategoryExpanded = GetIsCategoryExpanded(commonDescriptorPropertyDefinition.CategoryValue);
			propertyItem.IsExpanded = GetIsExpanded(commonDescriptorPropertyDefinition.PropertyDescriptors);
			propertyItem.WillRefreshPropertyGrid = GetWillRefreshPropertyGrid(commonDescriptorPropertyDefinition.PropertyDescriptors);
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
			object result2;
			if (categoryDefinition != null && categoryDefinition.DisplayOrder.HasValue)
			{
				result = categoryDefinition.DisplayOrder.Value;
			}
			else if (PropertyGridUtilities.IsSameForAllObject(SelectedObjects, delegate(object o)
			{
				CategoryOrderAttribute[] source = (o != null) ? ((CategoryOrderAttribute[])o.GetType().GetCustomAttributes(typeof(CategoryOrderAttribute), true)) : new CategoryOrderAttribute[0];
				CategoryOrderAttribute categoryOrderAttribute = source.FirstOrDefault((CategoryOrderAttribute a) => object.Equals(a.CategoryValue, categoryValue));
				return (categoryOrderAttribute != null) ? categoryOrderAttribute.Order : 2147483647;
			}, out result2))
			{
				result = (int)result2;
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
			object result;
			if (PropertyGridUtilities.IsSameForAllObject(SelectedObjects, delegate(object o)
			{
				ExpandedCategoryAttribute[] source = (o != null) ? ((ExpandedCategoryAttribute[])o.GetType().GetCustomAttributes(typeof(ExpandedCategoryAttribute), true)) : new ExpandedCategoryAttribute[0];
				ExpandedCategoryAttribute expandedCategoryAttribute = source.FirstOrDefault((ExpandedCategoryAttribute a) => object.Equals(a.CategoryValue, categoryValue));
				if (expandedCategoryAttribute != null)
				{
					return expandedCategoryAttribute.IsExpanded;
				}
				return true;
			}, out result))
			{
				return (bool)result;
			}
			return true;
		}

		private bool GetIsExpanded(List<PropertyDescriptor> commonDescriptorsLists)
		{
			object result;
			if (PropertyGridUtilities.IsSameForAllObject(commonDescriptorsLists, (object o) => GetIsExpanded((PropertyDescriptor)o), out result))
			{
				return (bool)result;
			}
			return false;
		}

		private bool GetWillRefreshPropertyGrid(List<PropertyDescriptor> commonDescriptorsLists)
		{
			object result;
			if (PropertyGridUtilities.IsSameForAllObject(commonDescriptorsLists, (object o) => GetWillRefreshPropertyGrid((PropertyDescriptor)o), out result))
			{
				return (bool)result;
			}
			return false;
		}
	}
}
