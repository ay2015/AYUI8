using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal abstract class ObjectContainerHelperBase : ContainerHelperBase
	{
		private bool _isPreparingItemFlag;

		private PropertyItemCollection _propertyItemCollection;

		public override IList Properties
		{
			get
			{
				return _propertyItemCollection;
			}
		}

		private PropertyItem DefaultProperty
		{
			get
			{
				PropertyItem result = null;
				string defaultName = GetDefaultPropertyName();
				if (defaultName != null)
				{
					result = _propertyItemCollection.FirstOrDefault((PropertyItem prop) => object.Equals(defaultName, prop.PropertyDescriptor.Name));
				}
				return result;
			}
		}

		protected PropertyItemCollection PropertyItems
		{
			get
			{
				return _propertyItemCollection;
			}
		}

		internal event EventHandler ObjectsGenerated;

		public ObjectContainerHelperBase(IPropertyContainer propertyContainer)
			: base(propertyContainer)
		{
			_propertyItemCollection = new PropertyItemCollection(new ObservableCollection<PropertyItem>());
			UpdateFilter();
			UpdateCategorization(false);
		}

		public override PropertyItemBase ContainerFromItem(object item)
		{
			if (item == null)
			{
				return null;
			}
			PropertyItem propertyItem = item as PropertyItem;
			if (propertyItem != null)
			{
				return propertyItem;
			}
			string propertyStr = item as string;
			if (propertyStr != null)
			{
				return PropertyItems.FirstOrDefault((PropertyItem prop) => propertyStr == prop.PropertyDescriptor.Name);
			}
			return null;
		}

		public override object ItemFromContainer(PropertyItemBase container)
		{
			PropertyItem propertyItem = container as PropertyItem;
			if (propertyItem == null)
			{
				return null;
			}
			return propertyItem.PropertyDescriptor.Name;
		}

		public override void UpdateValuesFromSource()
		{
			foreach (PropertyItem propertyItem in PropertyItems)
			{
				propertyItem.DescriptorDefinition.UpdateValueFromSource();
				propertyItem.ContainerHelper.UpdateValuesFromSource();
			}
		}

		public void GenerateProperties()
		{
			if (PropertyItems.Count == 0 || ShouldRegenerateProperties())
			{
				RegenerateProperties();
			}
		}

		protected override void OnFilterChanged()
		{
			UpdateFilter();
		}

		protected override void OnCategorizationChanged()
		{
			UpdateCategorization(true);
		}

		protected override void OnAutoGeneratePropertiesChanged()
		{
			RegenerateProperties();
		}

		protected override void OnHideInheritedPropertiesChanged()
		{
			RegenerateProperties();
		}

		protected override void OnEditorDefinitionsChanged()
		{
			RegenerateProperties();
		}

		protected override void OnPropertyDefinitionsChanged()
		{
			RegenerateProperties();
		}

		protected override void OnCategoryDefinitionsChanged()
		{
			RegenerateProperties();
		}

		public override void NotifyCategoryDefinitionsCollectionChanged()
		{
			RegenerateProperties();
		}

		protected internal virtual bool ShouldRegenerateProperties()
		{
			return false;
		}

		internal bool IsExpandingNonPrimitiveTypes()
		{
			if (PropertyContainer == null)
			{
				return false;
			}
			return PropertyContainer.IsExpandingNonPrimitiveTypes;
		}

		protected internal override void SetPropertiesExpansion(bool isExpanded)
		{
			if (Properties.Count == 0)
			{
				GenerateProperties();
			}
			base.SetPropertiesExpansion(isExpanded);
		}

		protected internal override void SetPropertiesExpansion(string propertyName, bool isExpanded)
		{
			if (Properties.Count == 0)
			{
				GenerateProperties();
			}
			base.SetPropertiesExpansion(propertyName, isExpanded);
		}

		private void UpdateFilter()
		{
			FilterInfo filterInfo = PropertyContainer.FilterInfo;
			PropertyItems.FilterPredicate = (filterInfo.Predicate ?? PropertyItemCollection.CreateFilter(filterInfo.InputString, PropertyItems, PropertyContainer));
		}

		private void UpdateCategorization(bool updateSubPropertiesCategorization)
		{
			_propertyItemCollection.UpdateCategorization(ComputeCategoryGroupDescription(), PropertyContainer.IsCategorized, PropertyContainer.IsSortedAlphabetically);
			if (updateSubPropertiesCategorization && _propertyItemCollection.Count > 0)
			{
				foreach (PropertyItem item in _propertyItemCollection)
				{
					PropertyItemCollection propertyItemCollection = item.Properties as PropertyItemCollection;
					if (propertyItemCollection != null)
					{
						propertyItemCollection.UpdateCategorization(ComputeCategoryGroupDescription(), PropertyContainer.IsCategorized, PropertyContainer.IsSortedAlphabetically);
					}
				}
			}
		}

		private GroupDescription ComputeCategoryGroupDescription()
		{
			if (!PropertyContainer.IsCategorized)
			{
				return null;
			}
			return PropertyContainer.CategoryGroupDescription ?? new PropertyGroupDescription(PropertyItemCollection.CategoryPropertyName);
		}

		private string GetCategoryGroupingPropertyName()
		{
			PropertyGroupDescription propertyGroupDescription = ComputeCategoryGroupDescription() as PropertyGroupDescription;
			if (propertyGroupDescription == null)
			{
				return null;
			}
			return propertyGroupDescription.PropertyName;
		}

		private void OnChildrenPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if ((IsItemOrderingProperty(e.PropertyName) || GetCategoryGroupingPropertyName() == e.PropertyName) && base.ChildrenItemsControl.ItemContainerGenerator.Status != GeneratorStatus.GeneratingContainers && !_isPreparingItemFlag)
			{
				PropertyItems.RefreshView();
			}
		}

		protected abstract string GetDefaultPropertyName();

		protected abstract void GenerateSubPropertiesCore(Action<IEnumerable<PropertyItem>> updatePropertyItemsCallback);

		private void RegenerateProperties()
		{
			GenerateSubPropertiesCore(UpdatePropertyItemsCallback);
		}

		protected internal virtual void UpdatePropertyItemsCallback(IEnumerable<PropertyItem> subProperties)
		{
			foreach (PropertyItem subProperty in subProperties)
			{
				InitializePropertyItem(subProperty);
			}
			foreach (PropertyItem propertyItem in PropertyItems)
			{
				propertyItem.PropertyChanged -= OnChildrenPropertyChanged;
			}
			PropertyItems.UpdateItems(subProperties);
			foreach (PropertyItem propertyItem2 in PropertyItems)
			{
				propertyItem2.PropertyChanged += OnChildrenPropertyChanged;
			}
			PropertyGrid propertyGrid = PropertyContainer as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.SelectedPropertyItem = DefaultProperty;
			}
			if (this.ObjectsGenerated != null)
			{
				this.ObjectsGenerated(this, EventArgs.Empty);
			}
		}

		protected static List<PropertyDescriptor> GetPropertyDescriptors(object instance, bool hideInheritedProperties)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = null;
			TypeConverter converter = TypeDescriptor.GetConverter(instance);
			if (converter == null || !converter.GetPropertiesSupported())
			{
				if (instance is ICustomTypeDescriptor)
				{
					propertyDescriptorCollection = ((ICustomTypeDescriptor)instance).GetProperties();
				}
				else if (instance.GetType().GetInterface("ICustomTypeProvider", true) != null)
				{
					MethodInfo method = instance.GetType().GetMethod("GetCustomType");
					Type componentType = method.Invoke(instance, null) as Type;
					propertyDescriptorCollection = TypeDescriptor.GetProperties(componentType);
				}
				else
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(instance.GetType());
				}
			}
			else
			{
				try
				{
					propertyDescriptorCollection = converter.GetProperties(instance);
				}
				catch (Exception)
				{
				}
			}
			if (propertyDescriptorCollection != null)
			{
				IEnumerable<PropertyDescriptor> source = propertyDescriptorCollection.Cast<PropertyDescriptor>();
				if (hideInheritedProperties)
				{
					IEnumerable<PropertyDescriptor> source2 = from p in source
					where p.ComponentType == instance.GetType()
					select p;
					return source2.ToList();
				}
				return source.ToList();
			}
			return null;
		}

		protected bool GetIsExpanded(PropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor == null)
			{
				return false;
			}
			ExpandableObjectAttribute attribute = PropertyGridUtilities.GetAttribute<ExpandableObjectAttribute>(propertyDescriptor);
			if (attribute != null)
			{
				return attribute.IsExpanded;
			}
			return false;
		}

		protected bool GetWillRefreshPropertyGrid(PropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor == null)
			{
				return false;
			}
			RefreshPropertiesAttribute attribute = PropertyGridUtilities.GetAttribute<RefreshPropertiesAttribute>(propertyDescriptor);
			if (attribute != null)
			{
				return attribute.RefreshProperties != RefreshProperties.None;
			}
			return false;
		}

		internal void InitializeDescriptorDefinition(DescriptorPropertyDefinitionBase descriptorDef, PropertyDefinition propertyDefinition)
		{
			if (descriptorDef == null)
			{
				throw new ArgumentNullException("descriptorDef");
			}
			if (propertyDefinition != null && propertyDefinition != null)
			{
				if (propertyDefinition.Category != null)
				{
					descriptorDef.Category = propertyDefinition.Category;
					descriptorDef.CategoryValue = propertyDefinition.Category;
				}
				if (propertyDefinition.Description != null)
				{
					descriptorDef.Description = propertyDefinition.Description;
				}
				if (propertyDefinition.DisplayName != null)
				{
					descriptorDef.DisplayName = propertyDefinition.DisplayName;
				}
				if (propertyDefinition.DisplayOrder.HasValue)
				{
					descriptorDef.DisplayOrder = propertyDefinition.DisplayOrder.Value;
				}
				if (propertyDefinition.IsExpandable.HasValue)
				{
					descriptorDef.ExpandableAttribute = propertyDefinition.IsExpandable.Value;
				}
			}
		}

		private void InitializePropertyItem(PropertyItem propertyItem)
		{
			DescriptorPropertyDefinitionBase pd = propertyItem.DescriptorDefinition;
			propertyItem.PropertyDescriptor = pd.PropertyDescriptor;
			propertyItem.IsReadOnly = pd.IsReadOnly;
			propertyItem.DisplayName = pd.DisplayName;
			propertyItem.Description = pd.Description;
			propertyItem.DefinitionKey = pd.DefinitionKey;
			if (pd.DependsOnPropertyItemNames != null)
			{
				foreach (string dependsOnPropertyItemName in pd.DependsOnPropertyItemNames)
				{
					PropertyContainer.DependsOnPropertyItemsList.Add(new KeyValuePair<string, PropertyItem>(dependsOnPropertyItemName, propertyItem));
				}
			}
			propertyItem.Category = pd.Category;
			propertyItem.PropertyOrder = pd.DisplayOrder;
			if (pd.PropertyDescriptor.Converter is ExpandableObjectConverter)
			{
				propertyItem.IsExpandable = true;
			}
			else
			{
				SetupDefinitionBinding(propertyItem, PropertyItemBase.IsExpandableProperty, pd, () => pd.IsExpandable, BindingMode.OneWay);
			}
			SetupDefinitionBinding(propertyItem, PropertyItemBase.AdvancedOptionsIconProperty, pd, () => pd.AdvancedOptionsIcon, BindingMode.OneWay);
			SetupDefinitionBinding(propertyItem, PropertyItemBase.AdvancedOptionsTooltipProperty, pd, () => pd.AdvancedOptionsTooltip, BindingMode.OneWay);
			SetupDefinitionBinding(propertyItem, CustomPropertyItem.ValueProperty, pd, () => pd.Value, BindingMode.TwoWay);
			if (pd.CommandBindings != null)
			{
				foreach (CommandBinding commandBinding in pd.CommandBindings)
				{
					propertyItem.CommandBindings.Add(commandBinding);
				}
			}
		}

		private object GetTypeDefaultValue(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetProperty("Value").PropertyType;
			}
			if (!type.IsValueType)
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		private void SetupDefinitionBinding<T>(PropertyItem propertyItem, DependencyProperty itemProperty, DescriptorPropertyDefinitionBase pd, Expression<Func<T>> definitionProperty, BindingMode bindingMode)
		{
			string propertyOrFieldName = ReflectionHelper.GetPropertyOrFieldName(definitionProperty);
			Binding binding = new Binding(propertyOrFieldName);
			binding.Source = pd;
			binding.Mode = bindingMode;
			Binding binding2 = binding;
			propertyItem.SetBinding(itemProperty, binding2);
		}

		internal FrameworkElement GenerateChildrenEditorElement(PropertyItem propertyItem)
		{
			FrameworkElement frameworkElement = null;
			DescriptorPropertyDefinitionBase descriptorDefinition = propertyItem.DescriptorDefinition;
			object definitionKey = propertyItem.DefinitionKey;
			Type type = definitionKey as Type;
			ITypeEditor typeEditor = null;
			if (typeEditor == null)
			{
				typeEditor = descriptorDefinition.CreateAttributeEditor();
			}
			if (typeEditor != null)
			{
				frameworkElement = typeEditor.ResolveEditor(propertyItem);
			}
			if (frameworkElement == null && definitionKey != null)
			{
				frameworkElement = GenerateCustomEditingElement(definitionKey, propertyItem);
			}
			if (frameworkElement == null && type != null)
			{
				frameworkElement = GenerateCustomEditingElement(type, propertyItem);
			}
			if (frameworkElement == null && definitionKey == null && propertyItem.PropertyDescriptor != null)
			{
				frameworkElement = GenerateCustomEditingElement(propertyItem.PropertyDescriptor.Name, propertyItem);
			}
			if (frameworkElement == null && type == null)
			{
				frameworkElement = GenerateCustomEditingElement(propertyItem.PropertyType, propertyItem);
			}
			if (frameworkElement == null)
			{
				if (propertyItem.IsReadOnly)
				{
					typeEditor = new TextBlockEditor();
				}
				if (typeEditor == null)
				{
					typeEditor = ((type != null) ? PropertyGridUtilities.CreateDefaultEditor(type, null, propertyItem) : descriptorDefinition.CreateDefaultEditor(propertyItem));
				}
				frameworkElement = typeEditor.ResolveEditor(propertyItem);
			}
			return frameworkElement;
		}

		internal PropertyDefinition GetPropertyDefinition(PropertyDescriptor descriptor)
		{
			PropertyDefinition propertyDefinition = null;
			PropertyDefinitionCollection propertyDefinitions = PropertyContainer.PropertyDefinitions;
			if (propertyDefinitions != null)
			{
				propertyDefinition = propertyDefinitions[descriptor.Name];
				if (propertyDefinition == null)
				{
					propertyDefinition = propertyDefinitions.GetRecursiveBaseTypes(descriptor.PropertyType);
				}
			}
			return propertyDefinition;
		}

		internal CategoryDefinition GetCategoryDefinition(object categoryValue)
		{
			CategoryDefinitionCollection categoryDefinitions = PropertyContainer.CategoryDefinitions;
			if (categoryDefinitions == null)
			{
				return null;
			}
			return categoryDefinitions[categoryValue];
		}

		public override void PrepareChildrenPropertyItem(PropertyItemBase propertyItem, object item)
		{
			_isPreparingItemFlag = true;
			base.PrepareChildrenPropertyItem(propertyItem, item);
			if (propertyItem.Editor == null)
			{
				FrameworkElement frameworkElement = GenerateChildrenEditorElement((PropertyItem)propertyItem);
				if (frameworkElement != null)
				{
					ContainerHelperBase.SetIsGenerated(frameworkElement, true);
					propertyItem.Editor = frameworkElement;
				}
			}
			_isPreparingItemFlag = false;
		}

		public override void ClearChildrenPropertyItem(PropertyItemBase propertyItem, object item)
		{
			if (propertyItem.Editor != null && ContainerHelperBase.GetIsGenerated(propertyItem.Editor))
			{
				propertyItem.Editor = null;
			}
			base.ClearChildrenPropertyItem(propertyItem, item);
		}

		public override Binding CreateChildrenDefaultBinding(PropertyItemBase propertyItem)
		{
			Binding binding = new Binding("Value");
			binding.Mode = (((PropertyItem)propertyItem).IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay);
			return binding;
		}

		protected static string GetDefaultPropertyName(object instance)
		{
			AttributeCollection attributes = TypeDescriptor.GetAttributes(instance);
			DefaultPropertyAttribute defaultPropertyAttribute = (DefaultPropertyAttribute)attributes[typeof(DefaultPropertyAttribute)];
			if (defaultPropertyAttribute == null)
			{
				return null;
			}
			return defaultPropertyAttribute.Name;
		}

		private static bool IsItemOrderingProperty(string propertyName)
		{
			if (!string.Equals(propertyName, PropertyItemCollection.DisplayNamePropertyName) && !string.Equals(propertyName, PropertyItemCollection.CategoryOrderPropertyName))
			{
				return string.Equals(propertyName, PropertyItemCollection.PropertyOrderPropertyName);
			}
			return true;
		}
	}
}
