using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal abstract class PropertiesContainerHelperBase : ContainerHelperBase
	{
		private PropertiesCollectionView _propertyItems;

		internal static readonly string CategoryPropertyName;

		internal static readonly string CategoryOrderPropertyName;

		internal static readonly string PropertyOrderPropertyName;

		protected PropertiesCollectionView CollectionView
		{
			get
			{
				return _propertyItems;
			}
			set
			{
				_propertyItems = value;
				UpdateCategorization();
			}
		}

		public override IList Properties
		{
			get
			{
				return _propertyItems;
			}
		}

		static PropertiesContainerHelperBase()
		{
			PropertyItem p = null;
			CategoryPropertyName = ReflectionHelper.GetPropertyOrFieldName(() => p.Category);
			CategoryOrderPropertyName = ReflectionHelper.GetPropertyOrFieldName(() => p.CategoryOrder);
			PropertyOrderPropertyName = ReflectionHelper.GetPropertyOrFieldName(() => p.PropertyOrder);
		}

		protected PropertiesContainerHelperBase(IPropertyContainer propertyContainer)
			: base(propertyContainer)
		{
		}

		public override void PrepareChildrenPropertyItem(PropertyItemBase propertyItem, object item)
		{
			base.PrepareChildrenPropertyItem(propertyItem, item);
			CustomPropertyItem customPropertyItem = propertyItem as CustomPropertyItem;
			if (customPropertyItem != null)
			{
				PrepareCustomPropertyItemCore(customPropertyItem);
			}
			else
			{
				PrepareChildrenPropertyItemCore(propertyItem, item);
			}
		}

		public override void ClearChildrenPropertyItem(PropertyItemBase propertyItem, object item)
		{
			Binding binding = BindingOperations.GetBinding(propertyItem, PropertyItemBase.DisplayNameProperty);
			if (binding != null && binding == PropertyContainer.PropertyNameBinding)
			{
				BindingOperations.ClearBinding(propertyItem, PropertyItemBase.DisplayNameProperty);
			}
			if (propertyItem.Editor != null && ContainerHelperBase.GetIsGenerated(propertyItem.Editor))
			{
				propertyItem.ClearValue(PropertyItemBase.EditorProperty);
			}
			base.ClearChildrenPropertyItem(propertyItem, item);
		}

		public override Binding CreateChildrenDefaultBinding(PropertyItemBase propertyItem)
		{
			CustomPropertyItem customPropertyItem = propertyItem as CustomPropertyItem;
			if (customPropertyItem != null)
			{
				return CreateCustomPropertyBinding(customPropertyItem);
			}
			return PropertyContainer.PropertyValueBinding;
		}

		protected override void OnFilterChanged()
		{
			UpdateFilter();
		}

		protected override void OnCategorizationChanged()
		{
			UpdateCategorization();
		}

		protected override void OnEditorDefinitionsChanged()
		{
			ReprepareItems();
		}

		protected override void OnPropertyDefinitionsChanged()
		{
			ReprepareItems();
		}

		public override void NotifyEditorDefinitionsCollectionChanged()
		{
			ReprepareItems();
		}

		public override void NotifyPropertyDefinitionsCollectionChanged()
		{
		}

		public override void UpdateValuesFromSource()
		{
			throw new InvalidOperationException("This operation is not supported when using a list property source (e.g., PropertiesSource).");
		}

		private void ReprepareItems()
		{
			CollectionView.Refresh();
		}

		public override PropertyItemBase ContainerFromItem(object item)
		{
			if (base.ChildrenItemsControl == null)
			{
				return null;
			}
			return (PropertyItemBase)base.ChildrenItemsControl.ItemContainerGenerator.ContainerFromItem(item);
		}

		public override object ItemFromContainer(PropertyItemBase container)
		{
			if (base.ChildrenItemsControl == null)
			{
				return null;
			}
			if (container == null)
			{
				return null;
			}
			return base.ChildrenItemsControl.ItemContainerGenerator.ItemFromContainer(container);
		}

		private void PrepareChildrenPropertyItemCore(PropertyItemBase propertyItem, object item)
		{
			object obj = null;
			if (propertyItem.DisplayName == null && PropertyContainer.PropertyNameBinding != null)
			{
				BindingOperations.SetBinding(propertyItem, PropertyItemBase.DisplayNameProperty, PropertyContainer.PropertyNameBinding);
				obj = propertyItem.DisplayName;
			}
			FilterInfo filterInfo = PropertyContainer.FilterInfo;
			propertyItem.HighlightedText = filterInfo.InputString;
			if (propertyItem.Editor == null)
			{
				object definitionKey = propertyItem.DefinitionKey;
				Type type = definitionKey as Type;
				if (propertyItem.Editor == null && definitionKey != null)
				{
					propertyItem.Editor = GenerateCustomEditingElement(definitionKey, propertyItem);
				}
				if (propertyItem.Editor == null && type != null)
				{
					propertyItem.Editor = (GenerateCustomEditingElement(type, propertyItem) ?? GenerateSystemDefaultEditingElement(type, propertyItem));
				}
				if (propertyItem.Editor == null && definitionKey == null)
				{
					if (obj == null && PropertyContainer.PropertyNameBinding != null && item != null)
					{
						obj = GeneralUtilities.GetBindingValue(item, PropertyContainer.PropertyNameBinding);
					}
					if (obj != null)
					{
						propertyItem.Editor = GenerateCustomEditingElement(obj, propertyItem);
					}
				}
				if (propertyItem.Editor == null && type == null)
				{
					if (item != null && PropertyContainer.PropertyValueBinding != null)
					{
						object bindingValue = GeneralUtilities.GetBindingValue(item, CreateChildrenDefaultBinding(propertyItem));
						if (bindingValue != null)
						{
							Type type2 = bindingValue.GetType();
							propertyItem.Editor = (GenerateCustomEditingElement(type2, propertyItem) ?? GenerateSystemDefaultEditingElement(type2, propertyItem));
						}
					}
					else
					{
						propertyItem.Editor = GenerateDefaultEditingElement(propertyItem);
					}
				}
				if (propertyItem.Editor == null)
				{
					propertyItem.Editor = GenerateDefaultEditingElement(propertyItem);
				}
				if (propertyItem.Editor == null)
				{
					propertyItem.Editor = GenerateSystemDefaultEditingElement(propertyItem);
				}
				ContainerHelperBase.SetIsGenerated(propertyItem.Editor, true);
			}
		}

		private void PrepareCustomPropertyItemCore(CustomPropertyItem customProperty)
		{
			if (customProperty.Editor == null)
			{
				object obj = customProperty.DefinitionKey;
				Type type = obj as Type;
				if (obj == null && customProperty.Value != null)
				{
					type = customProperty.Value.GetType();
					obj = type;
				}
				if (obj != null)
				{
					customProperty.Editor = GenerateCustomEditingElement(obj, customProperty);
				}
				if (customProperty.Editor == null && type != null)
				{
					customProperty.Editor = GenerateCustomEditingElement(type, customProperty);
				}
				if (customProperty.Editor == null && type != null)
				{
					customProperty.Editor = GenerateSystemDefaultEditingElement(type, customProperty);
				}
				if (customProperty.Editor == null)
				{
					customProperty.Editor = GenerateDefaultEditingElement(customProperty);
				}
				if (customProperty.Editor == null)
				{
					customProperty.Editor = GenerateSystemDefaultEditingElement(customProperty);
				}
				if (customProperty.Editor != null)
				{
					ContainerHelperBase.SetIsGenerated(customProperty.Editor, true);
				}
			}
		}

		private Binding CreateCustomPropertyBinding(CustomPropertyItem customProperty)
		{
			Binding binding = new Binding();
			binding.Source = customProperty;
			binding.Path = new PropertyPath(CustomPropertyItem.ValueProperty);
			binding.Mode = BindingMode.TwoWay;
			return binding;
		}

		private void UpdateFilter()
		{
			if (CollectionView.CanFilter)
			{
				FilterInfo filterInfo = PropertyContainer.FilterInfo;
				if (filterInfo.Predicate != null)
				{
					CollectionView.Filter = filterInfo.Predicate;
				}
				else
				{
					Predicate<object> filter = null;
					Binding nameBinding = PropertyContainer.PropertyNameBinding;
					if (!string.IsNullOrEmpty(filterInfo.InputString))
					{
						filter = ((nameBinding == null) ? ((Predicate<object>)delegate(object item)
						{
							PropertyItemBase propertyItemBase = item as PropertyItemBase;
							if (propertyItemBase != null && propertyItemBase.DisplayName != null)
							{
								propertyItemBase.HighlightedText = (propertyItemBase.DisplayName.ToLower().Contains(filterInfo.InputString.ToLower()) ? filterInfo.InputString : null);
								if (CreateFilterSubItems(propertyItemBase, filterInfo.InputString))
								{
									return true;
								}
								return propertyItemBase.HighlightedText != null;
							}
							return false;
						}) : ((Predicate<object>)delegate(object item)
						{
							string text = GeneralUtilities.GetBindingValue(item, nameBinding) as string;
							if (text != null)
							{
								return text.ToLower().Contains(filterInfo.InputString.ToLower());
							}
							return false;
						}));
					}
					else
					{
						ClearFilterSubItems(CollectionView.SourceCollection);
					}
					CollectionView.Filter = filter;
				}
			}
		}

		private bool CreateFilterSubItems(PropertyItemBase property, string text)
		{
			if (property.IsExpandable)
			{
				bool flag = false;
				List<string> textFilter = new List<string>
				{
					text
				};
				IList properties = property.Properties;
				foreach (object item in properties)
				{
					PropertyItemBase propertyItemBase = item as PropertyItemBase;
					if (propertyItemBase != null && IsSubItemRespectingFilter(propertyItemBase, text, 1))
					{
						flag = true;
						textFilter.Add(propertyItemBase.DisplayName);
					}
				}
				if (flag)
				{
					property.IsExpanded = true;
					ICollectionView defaultView = CollectionViewSource.GetDefaultView(property.Properties);
					defaultView.Filter = ((object o) => ExecuteFilter(o, textFilter));
					int i;
					for (i = 1; i < textFilter.Count; i++)
					{
						PropertyItemBase propertyItemBase2 = property.Properties.Cast<PropertyItemBase>().FirstOrDefault((PropertyItemBase x) => x.DisplayName == textFilter[i]);
						if (propertyItemBase2 != null)
						{
							CreateFilterSubItems(propertyItemBase2, textFilter[0]);
						}
					}
					return true;
				}
				if (property.IsExpanded)
				{
					property.IsExpanded = false;
				}
			}
			return false;
		}

		private bool IsSubItemRespectingFilter(PropertyItemBase property, string text, int currentSubLevel)
		{
			if (currentSubLevel >= 10)
			{
				return false;
			}
			if (property.DisplayName.ToLower().Contains(text.ToLower()))
			{
				return true;
			}
			if (property.IsExpandable)
			{
				IList properties = property.Properties;
				foreach (object item in properties)
				{
					PropertyItemBase propertyItemBase = item as PropertyItemBase;
					if (propertyItemBase != null && IsSubItemRespectingFilter(propertyItemBase, text, currentSubLevel + 1))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool ExecuteFilter(object item, List<string> filters)
		{
			PropertyItemBase propertyItemBase = item as PropertyItemBase;
			if (propertyItemBase == null || propertyItemBase.DisplayName == null)
			{
				return false;
			}
			if (filters.Count > 0)
			{
				propertyItemBase.HighlightedText = (propertyItemBase.DisplayName.ToLower().Contains(filters[0].ToLower()) ? filters[0] : null);
			}
			for (int i = 0; i < filters.Count; i++)
			{
				if (propertyItemBase.DisplayName.ToLower().Contains(filters[i].ToLower()))
				{
					return true;
				}
			}
			return false;
		}

		private void ClearFilterSubItems(IEnumerable items)
		{
			foreach (object item in items)
			{
				PropertyItemBase propertyItemBase = item as PropertyItemBase;
				if (propertyItemBase != null)
				{
					propertyItemBase.HighlightedText = null;
					if (propertyItemBase.IsExpandable && propertyItemBase.IsExpanded)
					{
						ICollectionView defaultView = CollectionViewSource.GetDefaultView(propertyItemBase.Properties);
						defaultView.Filter = null;
						ClearFilterSubItems(propertyItemBase.Properties);
						propertyItemBase.IsExpanded = false;
					}
				}
			}
		}

		private void UpdateCategorization()
		{
			if (CollectionView.CanGroup)
			{
				if (CollectionView.GroupDescriptions.Count > 0)
				{
					CollectionView.GroupDescriptions.Clear();
				}
				if (CollectionView.SortDescriptions.Count > 0)
				{
					CollectionView.SortDescriptions.Clear();
				}
				GroupDescription groupDescription = ComputeCategoryGroupDescription();
				if (groupDescription != null)
				{
					CollectionView.GroupDescriptions.Add(groupDescription);
					if (!IsUsingPropertiesOfTypePropertyItemBase())
					{
						PropertyGroupDescription propertyGroupDescription = groupDescription as PropertyGroupDescription;
						if (propertyGroupDescription != null)
						{
							SortBy(propertyGroupDescription.PropertyName, ListSortDirection.Ascending);
						}
					}
				}
				if (IsUsingPropertiesOfTypePropertyItemBase())
				{
					SortBy(CategoryOrderPropertyName, ListSortDirection.Ascending);
					SortBy(CategoryPropertyName, ListSortDirection.Ascending);
					SortBy(PropertyOrderPropertyName, ListSortDirection.Ascending);
				}
				if (PropertyContainer.PropertyNameBinding != null)
				{
					string path = PropertyContainer.PropertyNameBinding.Path.Path;
					SortBy(path, ListSortDirection.Ascending);
				}
				else
				{
					SortBy(PropertyItemBase.DisplayNameProperty.Name, ListSortDirection.Ascending);
				}
			}
		}

		private bool IsUsingPropertiesOfTypePropertyItemBase()
		{
			IEnumerable sourceCollection = CollectionView.SourceCollection;
			if (sourceCollection is IList)
			{
				IList list = (IList)sourceCollection;
				if (list.Count > 0)
				{
					return list[0] is PropertyItemBase;
				}
				return false;
			}
			return false;
		}

		private GroupDescription ComputeCategoryGroupDescription()
		{
			if (!PropertyContainer.IsCategorized)
			{
				return null;
			}
			return PropertyContainer.CategoryGroupDescription;
		}

		private void SortBy(string name, ListSortDirection sortDirection)
		{
			CollectionView.SortDescriptions.Add(new SortDescription(name, sortDirection));
		}

		protected FrameworkElement GenerateDefaultEditingElement(PropertyItemBase propertyItem)
		{
			if (PropertyContainer.DefaultEditorDefinition == null)
			{
				return null;
			}
			return CreateCustomEditor(PropertyContainer.DefaultEditorDefinition, propertyItem);
		}

		protected FrameworkElement GenerateSystemDefaultEditingElement(Type type, PropertyItemBase propertyItem)
		{
			return PropertyGridUtilities.GenerateSystemDefaultEditingElement(type, propertyItem);
		}

		protected FrameworkElement GenerateSystemDefaultEditingElement(PropertyItemBase propertyItem)
		{
			return PropertyGridUtilities.GenerateSystemDefaultEditingElement(propertyItem);
		}
	}
}
