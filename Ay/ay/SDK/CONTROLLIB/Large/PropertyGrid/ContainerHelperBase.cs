using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal abstract class ContainerHelperBase
	{
		protected readonly IPropertyContainer PropertyContainer;

		internal static readonly DependencyProperty IsGeneratedProperty = DependencyProperty.RegisterAttached("IsGenerated", typeof(bool), typeof(ContainerHelperBase), new PropertyMetadata(false));

		public abstract IList Properties
		{
			get;
		}

		internal ItemsControl ChildrenItemsControl
		{
			get;
			set;
		}

		internal bool IsCleaning
		{
			get;
			private set;
		}

		public ContainerHelperBase(IPropertyContainer propertyContainer)
		{
			if (propertyContainer == null)
			{
				throw new ArgumentNullException("propertyContainer");
			}
			PropertyContainer = propertyContainer;
			INotifyPropertyChanged notifyPropertyChanged = propertyContainer as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged += OnPropertyContainerPropertyChanged;
			}
		}

		internal static bool GetIsGenerated(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsGeneratedProperty);
		}

		internal static void SetIsGenerated(DependencyObject obj, bool value)
		{
			obj.SetValue(IsGeneratedProperty, value);
		}

		public virtual void ClearHelper()
		{
			IsCleaning = true;
			INotifyPropertyChanged notifyPropertyChanged = PropertyContainer as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged -= OnPropertyContainerPropertyChanged;
			}
			if (ChildrenItemsControl != null)
			{
				((IItemContainerGenerator)ChildrenItemsControl.ItemContainerGenerator).RemoveAll();
			}
			IsCleaning = false;
		}

		public virtual void PrepareChildrenPropertyItem(PropertyItemBase propertyItem, object item)
		{
			propertyItem.ParentNode = PropertyContainer;
			PropertyGrid.RaisePreparePropertyItemEvent((UIElement)PropertyContainer, propertyItem, item);
		}

		public virtual void ClearChildrenPropertyItem(PropertyItemBase propertyItem, object item)
		{
			propertyItem.ParentNode = null;
			PropertyGrid.RaiseClearPropertyItemEvent((UIElement)PropertyContainer, propertyItem, item);
		}

		protected FrameworkElement GenerateCustomEditingElement(Type definitionKey, PropertyItemBase propertyItem)
		{
			if (PropertyContainer.EditorDefinitions == null)
			{
				return null;
			}
			return CreateCustomEditor(PropertyContainer.EditorDefinitions.GetRecursiveBaseTypes(definitionKey), propertyItem);
		}

		protected FrameworkElement GenerateCustomEditingElement(object definitionKey, PropertyItemBase propertyItem)
		{
			if (PropertyContainer.EditorDefinitions == null)
			{
				return null;
			}
			return CreateCustomEditor(PropertyContainer.EditorDefinitions[definitionKey], propertyItem);
		}

		protected FrameworkElement CreateCustomEditor(EditorDefinitionBase customEditor, PropertyItemBase propertyItem)
		{
			if (customEditor == null)
			{
				return null;
			}
			return customEditor.GenerateEditingElementInternal(propertyItem);
		}

		protected virtual void OnPropertyContainerPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			IPropertyContainer ps = null;
			if (propertyName == ReflectionHelper.GetPropertyOrFieldName(() => ps.FilterInfo))
			{
				OnFilterChanged();
			}
			else if (propertyName == ReflectionHelper.GetPropertyOrFieldName(() => ps.IsCategorized))
			{
				OnCategorizationChanged();
			}
			else if (PropertyContainer.IsCategorized && propertyName == ReflectionHelper.GetPropertyOrFieldName(() => ps.CategoryGroupDescription))
			{
				OnCategorizationChanged();
			}
			else if (propertyName == ReflectionHelper.GetPropertyOrFieldName(() => ps.CategoryDefinitions))
			{
				OnCategoryDefinitionsChanged();
			}
			else if (propertyName == ReflectionHelper.GetPropertyOrFieldName(() => ps.AutoGenerateProperties))
			{
				OnAutoGeneratePropertiesChanged();
			}
			else if (propertyName == ReflectionHelper.GetPropertyOrFieldName(() => ps.HideInheritedProperties))
			{
				OnHideInheritedPropertiesChanged();
			}
			else if (propertyName == ReflectionHelper.GetPropertyOrFieldName(() => ps.EditorDefinitions))
			{
				OnEditorDefinitionsChanged();
			}
			else if (propertyName == ReflectionHelper.GetPropertyOrFieldName(() => ps.PropertyDefinitions))
			{
				OnPropertyDefinitionsChanged();
			}
		}

		protected virtual void OnCategorizationChanged()
		{
		}

		protected virtual void OnFilterChanged()
		{
		}

		protected virtual void OnAutoGeneratePropertiesChanged()
		{
		}

		protected virtual void OnHideInheritedPropertiesChanged()
		{
		}

		protected virtual void OnEditorDefinitionsChanged()
		{
		}

		protected virtual void OnPropertyDefinitionsChanged()
		{
		}

		protected virtual void OnCategoryDefinitionsChanged()
		{
		}

		public virtual void OnEndInit()
		{
		}

		public abstract PropertyItemBase ContainerFromItem(object item);

		public abstract object ItemFromContainer(PropertyItemBase container);

		public abstract Binding CreateChildrenDefaultBinding(PropertyItemBase propertyItem);

		public virtual void NotifyEditorDefinitionsCollectionChanged()
		{
		}

		public virtual void NotifyPropertyDefinitionsCollectionChanged()
		{
		}

		public virtual void NotifyCategoryDefinitionsCollectionChanged()
		{
		}

		public abstract void UpdateValuesFromSource();

		protected internal virtual void SetPropertiesExpansion(bool isExpanded)
		{
			foreach (object property in Properties)
			{
				PropertyItemBase propertyItemBase = property as PropertyItemBase;
				if (propertyItemBase != null && propertyItemBase.IsExpandable)
				{
					if (propertyItemBase.ContainerHelper != null)
					{
						propertyItemBase.ContainerHelper.SetPropertiesExpansion(isExpanded);
					}
					propertyItemBase.IsExpanded = isExpanded;
				}
			}
		}

		protected internal virtual void SetPropertiesExpansion(string propertyName, bool isExpanded)
		{
			foreach (object property in Properties)
			{
				PropertyItemBase propertyItemBase = property as PropertyItemBase;
				if (propertyItemBase != null && propertyItemBase.IsExpandable)
				{
					if (propertyItemBase.DisplayName == propertyName)
					{
						propertyItemBase.IsExpanded = isExpanded;
						break;
					}
					if (propertyItemBase.ContainerHelper != null)
					{
						propertyItemBase.ContainerHelper.SetPropertiesExpansion(propertyName, isExpanded);
					}
				}
			}
		}
	}
}
