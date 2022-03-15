using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal abstract class DescriptorPropertyDefinitionBase : DependencyObject
	{
		private string _category;

		private string _categoryValue;

		private string _description;

		private string _displayName;

		private object _defaultValue;

		private object _definitionKey;

		private IList<string> _dependsOnPropertyItemNames;

		private int _displayOrder;

		private bool _expandableAttribute;

		private bool _expandableProperty;

		private bool _isReadOnly;

		private IList<Type> _newItemTypes;

		private IEnumerable<CommandBinding> _commandBindings;

		public static readonly DependencyProperty AdvancedOptionsIconProperty = DependencyProperty.Register("AdvancedOptionsIcon", typeof(ImageSource), typeof(DescriptorPropertyDefinitionBase), new UIPropertyMetadata(null));

		public static readonly DependencyProperty AdvancedOptionsTooltipProperty = DependencyProperty.Register("AdvancedOptionsTooltip", typeof(object), typeof(DescriptorPropertyDefinitionBase), new UIPropertyMetadata(null));

		public static readonly DependencyProperty IsExpandableProperty = DependencyProperty.Register("IsExpandable", typeof(bool), typeof(DescriptorPropertyDefinitionBase), new UIPropertyMetadata(false));

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(DescriptorPropertyDefinitionBase), new UIPropertyMetadata(null, OnValueChanged));

		internal abstract PropertyDescriptor PropertyDescriptor
		{
			get;
		}

		public ImageSource AdvancedOptionsIcon
		{
			get
			{
				return (ImageSource)GetValue(AdvancedOptionsIconProperty);
			}
			set
			{
				SetValue(AdvancedOptionsIconProperty, value);
			}
		}

		public object AdvancedOptionsTooltip
		{
			get
			{
				return GetValue(AdvancedOptionsTooltipProperty);
			}
			set
			{
				SetValue(AdvancedOptionsTooltipProperty, value);
			}
		}

		public bool IsExpandable
		{
			get
			{
				return (bool)GetValue(IsExpandableProperty);
			}
			set
			{
				SetValue(IsExpandableProperty, value);
			}
		}

		public string Category
		{
			get
			{
				return _category;
			}
			internal set
			{
				_category = value;
			}
		}

		public string CategoryValue
		{
			get
			{
				return _categoryValue;
			}
			internal set
			{
				_categoryValue = value;
			}
		}

		public IEnumerable<CommandBinding> CommandBindings
		{
			get
			{
				return _commandBindings;
			}
		}

		public string DisplayName
		{
			get
			{
				return _displayName;
			}
			internal set
			{
				_displayName = value;
			}
		}

		public object DefaultValue
		{
			get
			{
				return _defaultValue;
			}
			set
			{
				_defaultValue = value;
			}
		}

		public object DefinitionKey
		{
			get
			{
				return _definitionKey;
			}
		}

		public IList<string> DependsOnPropertyItemNames
		{
			get
			{
				return _dependsOnPropertyItemNames;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			internal set
			{
				_description = value;
			}
		}

		public int DisplayOrder
		{
			get
			{
				return _displayOrder;
			}
			internal set
			{
				_displayOrder = value;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return _isReadOnly;
			}
		}

		public IList<Type> NewItemTypes
		{
			get
			{
				return _newItemTypes;
			}
		}

		public string PropertyName
		{
			get
			{
				return PropertyDescriptor.Name;
			}
		}

		public Type PropertyType
		{
			get
			{
				return PropertyDescriptor.PropertyType;
			}
		}

		internal bool ExpandableAttribute
		{
			get
			{
				return _expandableAttribute;
			}
			set
			{
				_expandableAttribute = value;
				UpdateIsExpandable();
			}
		}

		internal bool ExpandableProperty
		{
			get
			{
				return _expandableProperty;
			}
			set
			{
				_expandableProperty = value;
				UpdateIsExpandable();
			}
		}

		internal bool IsPropertyGridCategorized
		{
			get;
			set;
		}

		internal bool IsExpandingNonPrimitiveTypes
		{
			get;
			set;
		}

		internal bool CanExpandProperty
		{
			get;
			set;
		}

		public object Value
		{
			get
			{
				return GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		public event EventHandler ContainerHelperInvalidated;

		internal DescriptorPropertyDefinitionBase(bool isPropertyGridCategorized, bool IsExpandingNonPrimitiveTypes, bool canExpandProperty)
		{
			IsPropertyGridCategorized = isPropertyGridCategorized;
			this.IsExpandingNonPrimitiveTypes = IsExpandingNonPrimitiveTypes;
			CanExpandProperty = canExpandProperty;
		}

		protected virtual string ComputeCategory()
		{
			return null;
		}

		protected virtual string ComputeCategoryValue()
		{
			return null;
		}

		protected virtual string ComputeDescription()
		{
			return null;
		}

		protected virtual object ComputeDefinitionKey()
		{
			return null;
		}

		protected virtual IList<string> ComputeDependsOnPropertyItems()
		{
			return null;
		}

		protected virtual int ComputeDisplayOrder(bool isPropertyGridCategorized)
		{
			return 2147483647;
		}

		protected virtual bool ComputeExpandableAttribute()
		{
			return false;
		}

		protected virtual object ComputeDefaultValueAttribute()
		{
			return null;
		}

		protected abstract bool ComputeIsExpandable();

		protected virtual IList<Type> ComputeNewItemTypes()
		{
			return null;
		}

		protected virtual bool ComputeIsReadOnly()
		{
			return false;
		}

		protected virtual bool ComputeCanResetValue()
		{
			return false;
		}

		protected virtual object ComputeAdvancedOptionsTooltip()
		{
			return null;
		}

		protected virtual void ResetValue()
		{
			BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(this, ValueProperty);
			if (bindingExpressionBase != null)
			{
				bindingExpressionBase.UpdateTarget();
			}
		}

		protected abstract BindingBase CreateValueBinding();

		internal abstract ObjectContainerHelperBase CreateContainerHelper(IPropertyContainer parent);

		internal void RaiseContainerHelperInvalidated()
		{
			if (this.ContainerHelperInvalidated != null)
			{
				this.ContainerHelperInvalidated(this, EventArgs.Empty);
			}
		}

		internal virtual ITypeEditor CreateDefaultEditor(PropertyItem propertyItem)
		{
			return null;
		}

		internal virtual ITypeEditor CreateAttributeEditor()
		{
			return null;
		}

		internal void UpdateAdvanceOptionsForItem(DependencyObject dependencyObject, DependencyPropertyDescriptor dpDescriptor, out object tooltip)
		{
			tooltip = StringConstants.Default;
			bool flag = false;
			bool flag2 = false;
			flag = typeof(Style).IsAssignableFrom(PropertyType);
			flag2 = typeof(DynamicResourceExtension).IsAssignableFrom(PropertyType);
			if (flag || flag2)
			{
				tooltip = StringConstants.Resource;
			}
			else if (dependencyObject != null && dpDescriptor != null)
			{
				if (BindingOperations.GetBindingExpressionBase(dependencyObject, dpDescriptor.DependencyProperty) != null)
				{
					tooltip = StringConstants.Databinding;
				}
				else
				{
					switch (DependencyPropertyHelper.GetValueSource(dependencyObject, dpDescriptor.DependencyProperty).BaseValueSource)
					{
					case BaseValueSource.DefaultStyleTrigger:
					case BaseValueSource.TemplateTrigger:
					case BaseValueSource.StyleTrigger:
					case BaseValueSource.ParentTemplate:
					case BaseValueSource.ParentTemplateTrigger:
						break;
					case BaseValueSource.Inherited:
					case BaseValueSource.DefaultStyle:
					case BaseValueSource.ImplicitStyleReference:
						tooltip = StringConstants.Inheritance;
						break;
					case BaseValueSource.Style:
						tooltip = StringConstants.StyleSetter;
						break;
					case BaseValueSource.Local:
						tooltip = StringConstants.Local;
						break;
					}
				}
			}
			else if (!object.Equals(Value, DefaultValue))
			{
				if (DefaultValue != null)
				{
					tooltip = StringConstants.Local;
				}
				else if (PropertyType.IsValueType)
				{
					object objB = Activator.CreateInstance(PropertyType);
					if (!object.Equals(Value, objB))
					{
						tooltip = StringConstants.Local;
					}
				}
				else if (Value != null)
				{
					tooltip = StringConstants.Local;
				}
			}
		}

		internal void UpdateAdvanceOptions()
		{
			AdvancedOptionsTooltip = ComputeAdvancedOptionsTooltip();
		}

		internal void UpdateIsExpandable()
		{
			IsExpandable = (ComputeIsExpandable() && (ExpandableAttribute || ExpandableProperty));
		}

		internal void UpdateValueFromSource()
		{
			BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(this, ValueProperty);
			if (bindingExpressionBase != null)
			{
				bindingExpressionBase.UpdateTarget();
			}
		}

		internal object ComputeCategoryForItem(object item)
		{
			PropertyDescriptor propertyDescriptor = item as PropertyDescriptor;
			DisplayAttribute attribute = PropertyGridUtilities.GetAttribute<DisplayAttribute>(propertyDescriptor);
			if (attribute != null && attribute.GetGroupName() != null)
			{
				return attribute.GetGroupName();
			}
			LocalizedCategoryAttribute attribute2 = PropertyGridUtilities.GetAttribute<LocalizedCategoryAttribute>(propertyDescriptor);
			if (attribute2 == null)
			{
				return propertyDescriptor.Category;
			}
			return attribute2.LocalizedCategory;
		}

		internal object ComputeCategoryValueForItem(object item)
		{
			PropertyDescriptor propertyDescriptor = item as PropertyDescriptor;
			LocalizedCategoryAttribute attribute = PropertyGridUtilities.GetAttribute<LocalizedCategoryAttribute>(propertyDescriptor);
			if (attribute == null)
			{
				return propertyDescriptor.Category;
			}
			return attribute.CategoryValue;
		}

		internal object ComputeDescriptionForItem(object item)
		{
			PropertyDescriptor propertyDescriptor = item as PropertyDescriptor;
			DisplayAttribute attribute = PropertyGridUtilities.GetAttribute<DisplayAttribute>(propertyDescriptor);
			if (attribute != null)
			{
				return attribute.GetDescription();
			}
			DescriptionAttribute attribute2 = PropertyGridUtilities.GetAttribute<DescriptionAttribute>(propertyDescriptor);
			if (attribute2 == null)
			{
				return propertyDescriptor.Description;
			}
			return attribute2.Description;
		}

		internal object ComputeNewItemTypesForItem(object item)
		{
			PropertyDescriptor property = item as PropertyDescriptor;
			NewItemTypesAttribute attribute = PropertyGridUtilities.GetAttribute<NewItemTypesAttribute>(property);
			if (attribute == null)
			{
				return null;
			}
			return attribute.Types;
		}

		internal object ComputeDefinitionKeyForItem(object item)
		{
			PropertyDescriptor property = item as PropertyDescriptor;
			DefinitionKeyAttribute attribute = PropertyGridUtilities.GetAttribute<DefinitionKeyAttribute>(property);
			if (attribute == null)
			{
				return null;
			}
			return attribute.Key;
		}

		internal IList<string> ComputeDependsOnPropertyItems(object item)
		{
			PropertyDescriptor property = item as PropertyDescriptor;
			DependsOnAttribute attribute = PropertyGridUtilities.GetAttribute<DependsOnAttribute>(property);
			if (attribute == null)
			{
				return null;
			}
			return attribute.PropertyItemNames;
		}

		internal object ComputeDisplayOrderForItem(object item)
		{
			PropertyDescriptor propertyDescriptor = item as PropertyDescriptor;
			DisplayAttribute attribute = PropertyGridUtilities.GetAttribute<DisplayAttribute>(PropertyDescriptor);
			if (attribute != null && attribute.GetOrder().HasValue)
			{
				return attribute.GetOrder();
			}
			List<PropertyOrderAttribute> list = propertyDescriptor.Attributes.OfType<PropertyOrderAttribute>().ToList();
			if (list.Count > 0)
			{
				ValidatePropertyOrderAttributes(list);
				if (IsPropertyGridCategorized)
				{
					PropertyOrderAttribute propertyOrderAttribute = list.FirstOrDefault(delegate(PropertyOrderAttribute x)
					{
						if (x.UsageContext != UsageContextEnum.Categorized)
						{
							return x.UsageContext == UsageContextEnum.Both;
						}
						return true;
					});
					if (propertyOrderAttribute != null)
					{
						return propertyOrderAttribute.Order;
					}
				}
				else
				{
					PropertyOrderAttribute propertyOrderAttribute2 = list.FirstOrDefault(delegate(PropertyOrderAttribute x)
					{
						if (x.UsageContext != 0)
						{
							return x.UsageContext == UsageContextEnum.Both;
						}
						return true;
					});
					if (propertyOrderAttribute2 != null)
					{
						return propertyOrderAttribute2.Order;
					}
				}
			}
			return 2147483647;
		}

		internal object ComputeExpandableAttributeForItem(object item)
		{
			PropertyDescriptor property = (PropertyDescriptor)item;
			ExpandableObjectAttribute attribute = PropertyGridUtilities.GetAttribute<ExpandableObjectAttribute>(property);
			return attribute != null;
		}

		internal int ComputeDisplayOrderInternal(bool isPropertyGridCategorized)
		{
			return ComputeDisplayOrder(isPropertyGridCategorized);
		}

		internal object GetValueInstance(object sourceObject)
		{
			ICustomTypeDescriptor customTypeDescriptor = sourceObject as ICustomTypeDescriptor;
			if (customTypeDescriptor != null)
			{
				sourceObject = customTypeDescriptor.GetPropertyOwner(PropertyDescriptor);
			}
			return sourceObject;
		}

		internal object ComputeDefaultValueAttributeForItem(object item)
		{
			PropertyDescriptor property = (PropertyDescriptor)item;
			DefaultValueAttribute attribute = PropertyGridUtilities.GetAttribute<DefaultValueAttribute>(property);
			if (attribute == null)
			{
				return null;
			}
			return attribute.Value;
		}

		private static void ExecuteResetValueCommand(object sender, ExecutedRoutedEventArgs e)
		{
			PropertyItem propertyItem = e.Parameter as PropertyItem;
			if (propertyItem == null)
			{
				propertyItem = (sender as PropertyItem);
			}
			if (propertyItem != null && propertyItem.DescriptorDefinition != null && propertyItem.DescriptorDefinition.ComputeCanResetValue())
			{
				propertyItem.DescriptorDefinition.ResetValue();
			}
		}

		private static void CanExecuteResetValueCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			PropertyItem propertyItem = e.Parameter as PropertyItem;
			if (propertyItem == null)
			{
				propertyItem = (sender as PropertyItem);
			}
			e.CanExecute = (propertyItem != null && propertyItem.DescriptorDefinition != null && propertyItem.DescriptorDefinition.ComputeCanResetValue());
		}

		private string ComputeDisplayName()
		{
			DisplayAttribute attribute = PropertyGridUtilities.GetAttribute<DisplayAttribute>(PropertyDescriptor);
			string text = (attribute != null) ? attribute.GetName() : PropertyDescriptor.DisplayName;
			ParenthesizePropertyNameAttribute attribute2 = PropertyGridUtilities.GetAttribute<ParenthesizePropertyNameAttribute>(PropertyDescriptor);
			if (attribute2 != null && attribute2.NeedParenthesis)
			{
				text = "(" + text + ")";
			}
			return text;
		}

		private void ValidatePropertyOrderAttributes(List<PropertyOrderAttribute> list)
		{
			if (list.Count > 0)
			{
				PropertyOrderAttribute propertyOrderAttribute = list.FirstOrDefault((PropertyOrderAttribute x) => x.UsageContext == UsageContextEnum.Both);
				if (propertyOrderAttribute != null)
				{
					int count = list.Count;
				}
			}
		}

		private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((DescriptorPropertyDefinitionBase)o).OnValueChanged(e.OldValue, e.NewValue);
		}

		internal virtual void OnValueChanged(object oldValue, object newValue)
		{
			UpdateIsExpandable();
			UpdateAdvanceOptions();
			CommandManager.InvalidateRequerySuggested();
		}

		public virtual void InitProperties()
		{
			_isReadOnly = ComputeIsReadOnly();
			_category = ComputeCategory();
			_categoryValue = ComputeCategoryValue();
			_description = ComputeDescription();
			_displayName = ComputeDisplayName();
			_defaultValue = ComputeDefaultValueAttribute();
			_displayOrder = ComputeDisplayOrder(IsPropertyGridCategorized);
			_expandableAttribute = ComputeExpandableAttribute();
			_definitionKey = ComputeDefinitionKey();
			_dependsOnPropertyItemNames = ComputeDependsOnPropertyItems();
			_expandableProperty = (IsExpandingNonPrimitiveTypes ? CanExpandFromCollection() : CanExpandProperty);
			_newItemTypes = ComputeNewItemTypes();
			_commandBindings = new CommandBinding[1]
			{
				new CommandBinding(PropertyItemCommands.ResetValue, ExecuteResetValueCommand, CanExecuteResetValueCommand)
			};
			BindingBase binding = CreateValueBinding();
			BindingOperations.SetBinding(this, ValueProperty, binding);
		}

		private bool CanExpandFromCollection()
		{
			if (PropertyType.IsGenericType && PropertyType.GetGenericTypeDefinition() != typeof(Dictionary<, >) && PropertyType.GetGenericTypeDefinition() != typeof(KeyValuePair<, >))
			{
				Type type = PropertyType.GetGenericArguments().FirstOrDefault();
				if (type != null && !type.IsPrimitive && !type.Equals(typeof(string)))
				{
					return !type.IsEnum;
				}
				return false;
			}
			if (!PropertyType.IsPrimitive && !PropertyType.Equals(typeof(string)))
			{
				return !PropertyType.IsEnum;
			}
			return false;
		}
	}
}
