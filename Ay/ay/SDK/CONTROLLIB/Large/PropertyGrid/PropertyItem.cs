using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Represents a property in PropertyItemCollection.</summary>
	[TemplatePart(Name = "content", Type = typeof(ContentControl))]
	public class PropertyItem : CustomPropertyItem
	{
		private class InvalidValueValidationRule : ValidationRule
		{
			public override ValidationResult Validate(object value, CultureInfo cultureInfo)
			{
				return new ValidationResult(false, null);
			}
		}

		/// <summary>Identifies the IsReadOnly dependency property.</summary>
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(PropertyItem), new UIPropertyMetadata(false, OnIsReadOnlyChanged));

		public static readonly DependencyProperty IsInvalidProperty = DependencyProperty.Register("IsInvalid", typeof(bool), typeof(PropertyItem), new UIPropertyMetadata(false, OnIsInvalidChanged));

		/// <summary>Gets a value indicating whether the property is read-only.</summary>
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

		/// <summary>
		///   <span id="BugEvents">Gets if the PropertyItem is Invalid.</span>
		/// </summary>
		public bool IsInvalid
		{
			get
			{
				return (bool)GetValue(IsInvalidProperty);
			}
			internal set
			{
				SetValue(IsInvalidProperty, value);
			}
		}

		/// <summary>Gets or sets the property descriptor.</summary>
		public PropertyDescriptor PropertyDescriptor
		{
			get;
			internal set;
		}

		/// <summary>Gets the name of the property references by the <strong>PropertyItem</strong>.</summary>
		public string PropertyName
		{
			get
			{
				if (DescriptorDefinition == null)
				{
					return null;
				}
				return DescriptorDefinition.PropertyName;
			}
		}

		/// <summary>Gets or sets the type of the property.</summary>
		public Type PropertyType
		{
			get
			{
				if (PropertyDescriptor == null)
				{
					return null;
				}
				return PropertyDescriptor.PropertyType;
			}
		}

		internal DescriptorPropertyDefinitionBase DescriptorDefinition
		{
			get;
			private set;
		}

		/// <summary>Gets this PropertyItem's instance.</summary>
		public object Instance
		{
			get;
			internal set;
		}

		private static void OnIsReadOnlyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyItem propertyItem = o as PropertyItem;
			if (propertyItem != null)
			{
				propertyItem.OnIsReadOnlyChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsReadOnlyChanged(bool oldValue, bool newValue)
		{
			if (base.IsLoaded)
			{
				RebuildEditor();
			}
		}

		private static void OnIsInvalidChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyItem propertyItem = o as PropertyItem;
			if (propertyItem != null)
			{
				propertyItem.OnIsInvalidChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsInvalidChanged(bool oldValue, bool newValue)
		{
			BindingExpression bindingExpression = GetBindingExpression(CustomPropertyItem.ValueProperty);
			if (newValue)
			{
				ValidationError validationError = new ValidationError(new InvalidValueValidationRule(), bindingExpression);
				validationError.ErrorContent = "Value could not be converted.";
				Validation.MarkInvalid(bindingExpression, validationError);
			}
			else
			{
				Validation.ClearInvalid(bindingExpression);
			}
		}

		protected override string GetPropertyItemName()
		{
			return PropertyName;
		}

		protected override Type GetPropertyItemType()
		{
			return PropertyType;
		}

		protected override void OnIsExpandedChanged(bool oldValue, bool newValue)
		{
			if (newValue && base.IsLoaded)
			{
				GenerateExpandedPropertyItems();
			}
		}

		protected override object OnCoerceValueChanged(object baseValue)
		{
			BindingExpression bindingExpression = GetBindingExpression(CustomPropertyItem.ValueProperty);
			SetRedInvalidBorder(bindingExpression);
			return baseValue;
		}

		protected override void OnValueChanged(object oldValue, object newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			if (newValue == null && DescriptorDefinition != null && DescriptorDefinition.DefaultValue != null)
			{
				SetCurrentValue(CustomPropertyItem.ValueProperty, DescriptorDefinition.DefaultValue);
			}
		}

		internal void SetRedInvalidBorder(BindingExpression be)
		{
			if (be != null && be.DataItem is DescriptorPropertyDefinitionBase)
			{
				DescriptorPropertyDefinitionBase element = be.DataItem as DescriptorPropertyDefinitionBase;
				if (Validation.GetHasError(element))
				{
					ReadOnlyObservableCollection<ValidationError> errors = Validation.GetErrors(element);
					Validation.MarkInvalid(be, errors[0]);
				}
			}
		}

		internal void RebuildEditor()
		{
			ObjectContainerHelperBase objectContainerHelperBase = base.ContainerHelper as ObjectContainerHelperBase;
			FrameworkElement frameworkElement = objectContainerHelperBase.GenerateChildrenEditorElement(this);
			if (frameworkElement != null)
			{
				ContainerHelperBase.SetIsGenerated(frameworkElement, true);
				base.Editor = frameworkElement;
				BindingExpression bindingExpression = GetBindingExpression(CustomPropertyItem.ValueProperty);
				if (bindingExpression != null)
				{
					bindingExpression.UpdateSource();
					SetRedInvalidBorder(bindingExpression);
				}
			}
		}

		private void OnDefinitionContainerHelperInvalidated(object sender, EventArgs e)
		{
			if (base.ContainerHelper != null)
			{
				base.ContainerHelper.ClearHelper();
			}
			ObjectContainerHelperBase objectContainerHelperBase = (ObjectContainerHelperBase)(base.ContainerHelper = DescriptorDefinition.CreateContainerHelper(this));
			if (base.IsExpanded)
			{
				objectContainerHelperBase.GenerateProperties();
			}
		}

		private void Init(DescriptorPropertyDefinitionBase definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException("definition");
			}
			if (base.ContainerHelper != null)
			{
				base.ContainerHelper.ClearHelper();
			}
			DescriptorDefinition = definition;
			base.ContainerHelper = definition.CreateContainerHelper(this);
			definition.ContainerHelperInvalidated += OnDefinitionContainerHelperInvalidated;
			base.Loaded += PropertyItem_Loaded;
		}

		private void GenerateExpandedPropertyItems()
		{
			if (base.IsExpanded)
			{
				ObjectContainerHelperBase objectContainerHelperBase = base.ContainerHelper as ObjectContainerHelperBase;
				if (objectContainerHelperBase != null)
				{
					objectContainerHelperBase.GenerateProperties();
				}
			}
		}

		private void PropertyItem_Loaded(object sender, RoutedEventArgs e)
		{
			GenerateExpandedPropertyItems();
		}

		internal PropertyItem(DescriptorPropertyDefinitionBase definition)
			: base(definition.IsPropertyGridCategorized, !definition.PropertyType.IsArray, definition.IsExpandingNonPrimitiveTypes)
		{
			Init(definition);
		}
	}
}
