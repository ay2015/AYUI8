using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Represents a collection of PropertyItem instances.</summary>
	public class PropertyItemCollection : ReadOnlyObservableCollection<PropertyItem>
	{
		internal static readonly string CategoryPropertyName;

		internal static readonly string CategoryOrderPropertyName;

		internal static readonly string PropertyOrderPropertyName;

		internal static readonly string DisplayNamePropertyName;

		private bool _preventNotification;

		internal Predicate<object> FilterPredicate
		{
			get
			{
				return GetDefaultView().Filter;
			}
			set
			{
				GetDefaultView().Filter = value;
			}
		}

		public ObservableCollection<PropertyItem> EditableCollection
		{
			get;
			private set;
		}

		static PropertyItemCollection()
		{
			PropertyItem p = null;
			CategoryPropertyName = ReflectionHelper.GetPropertyOrFieldName(() => p.Category);
			CategoryOrderPropertyName = ReflectionHelper.GetPropertyOrFieldName(() => p.CategoryOrder);
			PropertyOrderPropertyName = ReflectionHelper.GetPropertyOrFieldName(() => p.PropertyOrder);
			DisplayNamePropertyName = ReflectionHelper.GetPropertyOrFieldName(() => p.DisplayName);
		}

		/// <summary>Initializes a new instance of the PropertyItemCollection class.</summary>
		public PropertyItemCollection(ObservableCollection<PropertyItem> editableCollection)
			: base(editableCollection)
		{
			EditableCollection = editableCollection;
		}

		private ICollectionView GetDefaultView()
		{
			return CollectionViewSource.GetDefaultView(this);
		}

		/// <summary>Groups the PropertyItem instances using the passed name.</summary>
		/// <param name="name">A string representing the name of the grouping scheme.</param>
		public void GroupBy(string name)
		{
			GetDefaultView().GroupDescriptions.Add(new PropertyGroupDescription(name));
		}

		/// <summary>Sort the PropertyItem instances using the passed name and sort direction.</summary>
		/// <param name="name">A name representing the sorting scheme.</param>
		/// <param name="sortDirection">A ListSortDirection representing the direction to use.</param>
		public void SortBy(string name, ListSortDirection sortDirection)
		{
			GetDefaultView().SortDescriptions.Add(new SortDescription(name, sortDirection));
		}

		/// <summary>Filters the collection.</summary>
		/// <param name="text">The filter text.</param>
		public void Filter(string text)
		{
			Predicate<object> filter = CreateFilter(text, base.Items, null);
			GetDefaultView().Filter = filter;
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (!_preventNotification)
			{
				base.OnCollectionChanged(args);
			}
		}

		internal void UpdateItems(IEnumerable<PropertyItem> newItems)
		{
			if (newItems == null)
			{
				throw new ArgumentNullException("newItems");
			}
			_preventNotification = true;
			using (GetDefaultView().DeferRefresh())
			{
				EditableCollection.Clear();
				foreach (PropertyItem newItem in newItems)
				{
					EditableCollection.Add(newItem);
				}
			}
			_preventNotification = false;
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		internal void UpdateCategorization(GroupDescription groupDescription, bool isPropertyGridCategorized, bool sortAlphabetically)
		{
			foreach (PropertyItem item in base.Items)
			{
				item.DescriptorDefinition.DisplayOrder = item.DescriptorDefinition.ComputeDisplayOrderInternal(isPropertyGridCategorized);
				item.PropertyOrder = item.DescriptorDefinition.DisplayOrder;
			}
			ICollectionView defaultView = GetDefaultView();
			using (defaultView.DeferRefresh())
			{
				defaultView.GroupDescriptions.Clear();
				defaultView.SortDescriptions.Clear();
				if (groupDescription != null)
				{
					defaultView.GroupDescriptions.Add(groupDescription);
					if (sortAlphabetically)
					{
						SortBy(CategoryOrderPropertyName, ListSortDirection.Ascending);
						SortBy(CategoryPropertyName, ListSortDirection.Ascending);
					}
				}
				if (sortAlphabetically)
				{
					SortBy(PropertyOrderPropertyName, ListSortDirection.Ascending);
					SortBy(DisplayNamePropertyName, ListSortDirection.Ascending);
				}
			}
		}

		internal void RefreshView()
		{
			GetDefaultView().Refresh();
		}

		internal static Predicate<object> CreateFilter(string text, IList<PropertyItem> PropertyItems, IPropertyContainer propertyContainer)
		{
			Predicate<object> result = null;
			if (!string.IsNullOrEmpty(text))
			{
				result = delegate(object item)
				{
					PropertyItem propertyItem = item as PropertyItem;
					if (propertyItem.DisplayName != null)
					{
						DisplayAttribute attribute = PropertyGridUtilities.GetAttribute<DisplayAttribute>(propertyItem.PropertyDescriptor);
						if (attribute != null)
						{
							bool? autoGenerateFilter = attribute.GetAutoGenerateFilter();
							if (autoGenerateFilter.HasValue && !autoGenerateFilter.Value)
							{
								return false;
							}
						}
						propertyItem.HighlightedText = (propertyItem.DisplayName.ToLower().Contains(text.ToLower()) ? text : null);
						if (propertyContainer != null && CreateFilterSubItems(propertyItem, text, propertyContainer.AutoGenerateProperties, propertyContainer.PropertyDefinitions))
						{
							return true;
						}
						return propertyItem.HighlightedText != null;
					}
					return false;
				};
			}
			else
			{
				ClearFilterSubItems(PropertyItems.ToList());
			}
			return result;
		}

		private static bool CreateFilterSubItems(PropertyItem property, string text, bool autoGenerateProperties, PropertyDefinitionCollection propertyDefinitionCol)
		{
			if (property.IsExpandable)
			{
				bool flag = false;
				List<string> textFilter = new List<string>
				{
					text
				};
				PropertyDefinition propertyDefinition = GetSubPropertyDefinition(propertyDefinitionCol, property);
				if (!autoGenerateProperties && propertyDefinition == null)
				{
					return false;
				}
				PropertyInfo[] properties = property.PropertyType.GetProperties();
				if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericArguments().Count() > 0)
				{
					properties = property.PropertyType.GetGenericArguments()[0].GetProperties();
				}
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo in array)
				{
					PropertyDefinition subPropertyDefinition = GetSubPropertyDefinition(propertyDefinition, propertyInfo);
					if ((autoGenerateProperties || subPropertyDefinition != null) && IsSubItemRespectingFilter(propertyInfo, subPropertyDefinition, text, 1))
					{
						flag = true;
						textFilter.Add(propertyInfo.Name);
					}
				}
				if (flag)
				{
					property.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)delegate
					{
						property.IsExpanded = true;
						ICollectionView defaultView = CollectionViewSource.GetDefaultView(property.Properties);
						defaultView.Filter = ((object o) => ExecuteFilter(o, textFilter));
						int j;
						for (j = 1; j < textFilter.Count; j++)
						{
							PropertyItem propertyItem = property.Properties.Cast<PropertyItem>().FirstOrDefault((PropertyItem x) => x.DisplayName == textFilter[j]);
							if (propertyItem != null)
							{
								CreateFilterSubItems(propertyItem, textFilter[0], autoGenerateProperties, (propertyDefinition != null) ? propertyDefinition.PropertyDefinitions : null);
							}
						}
					});
					return true;
				}
				if (property.IsExpanded)
				{
					property.IsExpanded = false;
				}
			}
			return false;
		}

		private static bool IsSubItemRespectingFilter(PropertyInfo propertyInfo, PropertyDefinition propertyDefinition, string text, int currentSubLevel)
		{
			if (currentSubLevel >= 10)
			{
				return false;
			}
			BrowsableAttribute browsableAttribute = propertyInfo.GetCustomAttributes(typeof(BrowsableAttribute), false).FirstOrDefault() as BrowsableAttribute;
			if (browsableAttribute != null && !browsableAttribute.Browsable)
			{
				return false;
			}
			if (propertyInfo.Name.ToLower().Contains(text.ToLower()))
			{
				return true;
			}
			if (propertyInfo.GetCustomAttributes(typeof(ExpandableObjectAttribute), false).FirstOrDefault() != null || (propertyDefinition != null && propertyDefinition.IsExpandable.HasValue && propertyDefinition.IsExpandable.Value))
			{
				PropertyInfo[] properties = propertyInfo.PropertyType.GetProperties();
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo2 in array)
				{
					PropertyDefinition subPropertyDefinition = GetSubPropertyDefinition(propertyDefinition, propertyInfo2);
					if (IsSubItemRespectingFilter(propertyInfo2, subPropertyDefinition, text, currentSubLevel + 1))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static PropertyDefinition GetSubPropertyDefinition(PropertyDefinition propertyDefinition, PropertyInfo subProperty)
		{
			PropertyDefinition propertyDefinition2 = null;
			if (propertyDefinition != null && propertyDefinition.PropertyDefinitions != null && propertyDefinition.PropertyDefinitions.Count > 0)
			{
				propertyDefinition2 = propertyDefinition.PropertyDefinitions[subProperty.Name];
				if (propertyDefinition2 == null)
				{
					propertyDefinition2 = propertyDefinition.PropertyDefinitions.GetRecursiveBaseTypes(subProperty.PropertyType);
				}
			}
			return propertyDefinition2;
		}

		private static PropertyDefinition GetSubPropertyDefinition(PropertyDefinitionCollection propertyDefinitionCollection, PropertyItem subPropertyItem)
		{
			PropertyDefinition propertyDefinition = null;
			if (propertyDefinitionCollection != null && propertyDefinitionCollection.Count > 0)
			{
				propertyDefinition = propertyDefinitionCollection[subPropertyItem.PropertyName];
				if (propertyDefinition == null)
				{
					propertyDefinition = propertyDefinitionCollection.GetRecursiveBaseTypes(subPropertyItem.PropertyType);
				}
			}
			return propertyDefinition;
		}

		private static bool ExecuteFilter(object item, List<string> filters)
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

		private static void ClearFilterSubItems(IList items)
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
	}
}
