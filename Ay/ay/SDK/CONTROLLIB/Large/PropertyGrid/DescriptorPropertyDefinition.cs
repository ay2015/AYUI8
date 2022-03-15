using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class DescriptorPropertyDefinition : DescriptorPropertyDefinitionBase
	{
		private object _selectedObject;

		private PropertyDescriptor _propertyDescriptor;

		private DependencyPropertyDescriptor _dpDescriptor;

		private static Dictionary<string, Type> _dictEditorTypeName = new Dictionary<string, Type>();

		internal override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return _propertyDescriptor;
			}
		}

		private object SelectedObject
		{
			get
			{
				return _selectedObject;
			}
		}

		internal DescriptorPropertyDefinition(PropertyDescriptor propertyDescriptor, object selectedObject, IPropertyContainer propertyContainer)
			: base(propertyContainer.IsCategorized, propertyContainer.IsExpandingNonPrimitiveTypes, propertyContainer.CanExpandProperty(propertyDescriptor).HasValue && propertyContainer.CanExpandProperty(propertyDescriptor).Value)
		{
			Init(propertyDescriptor, selectedObject);
		}

		internal override ObjectContainerHelperBase CreateContainerHelper(IPropertyContainer parent)
		{
			return new ObjectContainerHelper(parent, base.Value);
		}

		internal override void OnValueChanged(object oldValue, object newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			RaiseContainerHelperInvalidated();
		}

		protected override BindingBase CreateValueBinding()
		{
			object sourceObject = SelectedObject;
			string path = PropertyDescriptor.Name;
			if (PropertyDescriptor is CollectionPropertyDescriptor)
			{
				sourceObject = ((CollectionPropertyDescriptor)PropertyDescriptor).Item;
				path = ".";
			}
			Binding binding = new Binding(path);
			binding.Source = GetValueInstance(sourceObject);
			binding.Mode = (PropertyDescriptor.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay);
			binding.ValidatesOnDataErrors = true;
			binding.ValidatesOnExceptions = true;
			binding.ConverterCulture = CultureInfo.CurrentCulture;
			return binding;
		}

		protected override bool ComputeIsReadOnly()
		{
			return PropertyDescriptor.IsReadOnly;
		}

		internal override ITypeEditor CreateDefaultEditor(PropertyItem propertyItem)
		{
			return PropertyGridUtilities.CreateDefaultEditor(PropertyDescriptor.PropertyType, PropertyDescriptor.Converter, propertyItem);
		}

		protected override bool ComputeCanResetValue()
		{
			if (!PropertyDescriptor.IsReadOnly)
			{
				object obj = ComputeDefaultValueAttribute();
				if (obj != null)
				{
					return !obj.Equals(base.Value);
				}
				return PropertyDescriptor.CanResetValue(SelectedObject);
			}
			return false;
		}

		protected override object ComputeAdvancedOptionsTooltip()
		{
			object tooltip;
			UpdateAdvanceOptionsForItem(SelectedObject as DependencyObject, _dpDescriptor, out tooltip);
			return tooltip;
		}

		protected override string ComputeCategory()
		{
			return (string)ComputeCategoryForItem(PropertyDescriptor);
		}

		protected override string ComputeCategoryValue()
		{
			return (string)ComputeCategoryValueForItem(PropertyDescriptor);
		}

		protected override bool ComputeExpandableAttribute()
		{
			return (bool)ComputeExpandableAttributeForItem(PropertyDescriptor);
		}

		protected override object ComputeDefaultValueAttribute()
		{
			return ComputeDefaultValueAttributeForItem(PropertyDescriptor);
		}

		protected override bool ComputeIsExpandable()
		{
			if (base.Value == null)
			{
				return PropertyDescriptor is CollectionPropertyDescriptor;
			}
			return true;
		}

		protected override IList<Type> ComputeNewItemTypes()
		{
			return (IList<Type>)ComputeNewItemTypesForItem(PropertyDescriptor);
		}

		protected override object ComputeDefinitionKey()
		{
			return ComputeDefinitionKeyForItem(PropertyDescriptor);
		}

		protected override IList<string> ComputeDependsOnPropertyItems()
		{
			return ComputeDependsOnPropertyItems(PropertyDescriptor);
		}

		protected override string ComputeDescription()
		{
			return (string)ComputeDescriptionForItem(PropertyDescriptor);
		}

		protected override int ComputeDisplayOrder(bool isPropertyGridCategorized)
		{
			base.IsPropertyGridCategorized = isPropertyGridCategorized;
			return (int)ComputeDisplayOrderForItem(PropertyDescriptor);
		}

		protected override void ResetValue()
		{
			PropertyDescriptor.ResetValue(SelectedObject);
			base.ResetValue();
		}

		internal override ITypeEditor CreateAttributeEditor()
		{
			EditorAttribute attribute = GetAttribute<EditorAttribute>();
			if (attribute != null)
			{
				Type value = null;
				if (!_dictEditorTypeName.TryGetValue(attribute.EditorTypeName, out value))
				{
					try
					{
						string[] typeDef = attribute.EditorTypeName.Split(',');
						if (typeDef.Length >= 2)
						{
							Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.FullName.Contains(typeDef[1].Trim()));
							if (assembly != null)
							{
								value = assembly.GetTypes().FirstOrDefault(delegate(Type t)
								{
									if (t != null && t.FullName != null)
									{
										return t.FullName.Contains(typeDef[0]);
									}
									return false;
								});
							}
						}
					}
					catch (Exception)
					{
					}
					if (value == null)
					{
						value = Type.GetType(attribute.EditorTypeName);
					}
					_dictEditorTypeName.Add(attribute.EditorTypeName, value);
				}
				if (typeof(ITypeEditor).IsAssignableFrom(value) && value.GetConstructor(new Type[0]) != null)
				{
					ITypeEditor typeEditor = Activator.CreateInstance(value) as ITypeEditor;
					if (typeEditor != null)
					{
						return typeEditor;
					}
				}
			}
			ItemsSourceAttribute attribute2 = GetAttribute<ItemsSourceAttribute>();
			if (attribute2 != null)
			{
				return new ItemsSourceAttributeEditor(attribute2);
			}
			return null;
		}

		private T GetAttribute<T>() where T : Attribute
		{
			return PropertyGridUtilities.GetAttribute<T>(PropertyDescriptor);
		}

		private void Init(PropertyDescriptor propertyDescriptor, object selectedObject)
		{
			if (propertyDescriptor == null)
			{
				throw new ArgumentNullException("propertyDescriptor");
			}
			if (selectedObject == null)
			{
				throw new ArgumentNullException("selectedObject");
			}
			_propertyDescriptor = propertyDescriptor;
			_selectedObject = selectedObject;
			_dpDescriptor = DependencyPropertyDescriptor.FromProperty(propertyDescriptor);
		}
	}
}
