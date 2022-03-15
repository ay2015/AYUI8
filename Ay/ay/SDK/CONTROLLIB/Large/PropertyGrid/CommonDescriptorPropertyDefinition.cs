using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class CommonDescriptorPropertyDefinition : DescriptorPropertyDefinitionBase
	{
		private readonly List<PropertyDescriptor> _propertyDescriptors;

		private readonly List<DependencyPropertyDescriptor> _dpDescriptors = new List<DependencyPropertyDescriptor>();

		private readonly IEnumerable _selectedObjects;

		private static Dictionary<object, object> _dictEditorTypeName = new Dictionary<object, object>();

		private static readonly DependencyProperty MultipleValuesProperty = DependencyProperty.Register("MultipleValues", typeof(IEnumerable), typeof(CommonDescriptorPropertyDefinition), new UIPropertyMetadata(null, OnMultipleValuesPropertyChanged));

		public List<PropertyDescriptor> PropertyDescriptors
		{
			get
			{
				return _propertyDescriptors;
			}
		}

		internal override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return PropertyDescriptors.First();
			}
		}

		private IEnumerable<object> ValueInstances
		{
			get
			{
				return _selectedObjects.Cast<object>();
			}
		}

		private int SelectedObjectsCount
		{
			get
			{
				return PropertyDescriptors.Count();
			}
		}

		private IEnumerable MultipleValues
		{
			get
			{
				return (IEnumerable)GetValue(MultipleValuesProperty);
			}
			set
			{
				SetValue(MultipleValuesProperty, value);
			}
		}

		internal CommonDescriptorPropertyDefinition(List<PropertyDescriptor> propertyDescriptorList, IEnumerable<object> selectedObjects, IPropertyContainer propertyContainer)
			: base(propertyContainer.IsCategorized, propertyContainer.IsExpandingNonPrimitiveTypes, propertyContainer.CanExpandProperty(propertyDescriptorList.FirstOrDefault()).HasValue && propertyContainer.CanExpandProperty(propertyDescriptorList.FirstOrDefault()).Value)
		{
			if (propertyDescriptorList == null)
			{
				throw new ArgumentNullException("propertyDescriptor");
			}
			if (propertyDescriptorList.Count == 0)
			{
				throw new InvalidOperationException("propertyDescriptorList is empty ! There are no common properties.");
			}
			if (selectedObjects == null)
			{
				throw new ArgumentNullException("selectedObjects");
			}
			_selectedObjects = selectedObjects;
			_propertyDescriptors = propertyDescriptorList;
			foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorList)
			{
				_dpDescriptors.Add(DependencyPropertyDescriptor.FromProperty(propertyDescriptor));
			}
		}

		private static void OnMultipleValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			CommonDescriptorPropertyDefinition commonDescriptorPropertyDefinition = (CommonDescriptorPropertyDefinition)sender;
			commonDescriptorPropertyDefinition.UpdateIsExpandable();
			commonDescriptorPropertyDefinition.RaiseContainerHelperInvalidated();
		}

		internal override ObjectContainerHelperBase CreateContainerHelper(IPropertyContainer parent)
		{
			if (!base.IsExpandable)
			{
				return new ObjectContainerHelper(parent, null);
			}
			return new ObjectsContainerHelper(parent, MultipleValues);
		}

		protected override BindingBase CreateValueBinding()
		{
			MultiBinding multiBinding = new MultiBinding();
			multiBinding.Converter = new CommonPropertyConverter(base.PropertyType);
			multiBinding.ValidationRules.Add(new CommonPropertyExceptionValidationRule(base.PropertyType));
			multiBinding.Mode = (base.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay);
			int num = 0;
			foreach (object valueInstance in ValueInstances)
			{
				object sourceObject = valueInstance;
				string path = base.PropertyName;
				if (PropertyDescriptor is CollectionPropertyDescriptor)
				{
					sourceObject = ((CollectionPropertyDescriptor)PropertyDescriptors[num++]).Item;
					path = ".";
				}
				Binding binding = new Binding(path);
				binding.Source = GetValueInstance(sourceObject);
				binding.Mode = (base.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay);
				binding.ValidatesOnDataErrors = true;
				binding.ValidatesOnExceptions = true;
				binding.ConverterCulture = CultureInfo.CurrentCulture;
				Binding item = binding;
				multiBinding.Bindings.Add(item);
			}
			return multiBinding;
		}

		protected override bool ComputeIsReadOnly()
		{
			Type typeFromHandle = typeof(IList);
			if (base.PropertyType.GetInterface(typeFromHandle.Name) != null)
			{
				return true;
			}
			return PropertyDescriptors.FirstOrDefault((PropertyDescriptor x) => x.IsReadOnly) != null;
		}

		internal override ITypeEditor CreateDefaultEditor(PropertyItem propertyItem)
		{
			object result = null;
			if (PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, (object o) => ((PropertyDescriptor)o).Converter.GetType(), out result))
			{
				return PropertyGridUtilities.CreateDefaultEditor(base.PropertyType, PropertyDescriptors.First().Converter, propertyItem);
			}
			return new TextBlockEditor();
		}

		protected override string ComputeCategory()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeCategoryForItem, out result);
			if (result == null)
			{
				return CategoryAttribute.Default.Category;
			}
			return result as string;
		}

		protected override string ComputeCategoryValue()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeCategoryValueForItem, out result);
			if (result == null)
			{
				return CategoryAttribute.Default.Category;
			}
			return result as string;
		}

		protected override object ComputeDefinitionKey()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeDefinitionKeyForItem, out result);
			if (result == null)
			{
				return null;
			}
			return result;
		}

		protected override IList<string> ComputeDependsOnPropertyItems()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeDependsOnPropertyItems, out result);
			if (result == null)
			{
				return null;
			}
			return result as IList<string>;
		}

		protected override string ComputeDescription()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeDescriptionForItem, out result);
			if (result == null)
			{
				return null;
			}
			return result as string;
		}

		protected override int ComputeDisplayOrder(bool isPropertyGridCategorized)
		{
			object result = null;
			base.IsPropertyGridCategorized = isPropertyGridCategorized;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeDisplayOrderForItem, out result);
			if (result == null)
			{
				return 2147483647;
			}
			return (int)result;
		}

		protected override bool ComputeExpandableAttribute()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeExpandableAttributeForItem, out result);
			if (result == null)
			{
				return false;
			}
			return (bool)result;
		}

		protected override object ComputeDefaultValueAttribute()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeDefaultValueAttributeForItem, out result);
			if (result == null)
			{
				return null;
			}
			return result;
		}

		protected override bool ComputeIsExpandable()
		{
			if (MultipleValues != null && MultipleValues.Cast<object>().Any())
			{
				return MultipleValues.Cast<object>().All((object o) => o != null);
			}
			return false;
		}

		public override void InitProperties()
		{
			base.InitProperties();
			MultiBinding multiBinding = new MultiBinding();
			multiBinding.Mode = BindingMode.OneWay;
			multiBinding.Converter = new MultipleValuesConverter();
			int num = 0;
			foreach (object valueInstance in ValueInstances)
			{
				object source = valueInstance;
				string path = base.PropertyName;
				if (PropertyDescriptor is CollectionPropertyDescriptor)
				{
					source = ((CollectionPropertyDescriptor)PropertyDescriptors[num++]).Item;
					path = ".";
				}
				Binding binding = new Binding(path);
				binding.Source = source;
				binding.Mode = BindingMode.OneWay;
				Binding item = binding;
				multiBinding.Bindings.Add(item);
			}
			BindingOperations.SetBinding(this, MultipleValuesProperty, multiBinding);
		}

		protected override IList<Type> ComputeNewItemTypes()
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, base.ComputeNewItemTypesForItem, out result);
			if (result == null)
			{
				return null;
			}
			return (IList<Type>)result;
		}

		protected override object ComputeAdvancedOptionsTooltip()
		{
			object[] tooltips = new object[SelectedObjectsCount];
			object[] array = ValueInstances.ToArray();
			for (int i = 0; i < SelectedObjectsCount; i++)
			{
				object tooltip;
				UpdateAdvanceOptionsForItem(array[i] as DependencyObject, _dpDescriptors[i], out tooltip);
				tooltips[i] = tooltip;
			}
			object result = "Advanced Properties";
			object result2 = null;
			if (PropertyGridUtilities.IsSameForAllObject(tooltips, (object o) => object.Equals(o, tooltips[0]), out result2))
			{
				result = tooltips[0];
			}
			return result;
		}

		protected override void ResetValue()
		{
			List<PropertyDescriptor> list = PropertyDescriptors.ToList();
			List<object> list2 = ValueInstances.ToList();
			for (int i = 0; i < SelectedObjectsCount; i++)
			{
				list[i].ResetValue(list2[i]);
			}
			UpdateAdvanceOptions();
			base.ResetValue();
		}

		protected override bool ComputeCanResetValue()
		{
			object obj = ComputeDefaultValueAttribute();
			if (obj != null)
			{
				return !obj.Equals(base.Value);
			}
			IList<PropertyDescriptor> list = PropertyDescriptors.ToList();
			IList<object> list2 = ValueInstances.ToList();
			for (int i = 0; i < SelectedObjectsCount; i++)
			{
				if (!list[i].CanResetValue(list2[i]))
				{
					return false;
				}
			}
			return !base.IsReadOnly;
		}

		internal override ITypeEditor CreateAttributeEditor()
		{
			if (IsAttributePresentForAllSelectedObjects<EditorAttribute>())
			{
				object value = null;
				string key = null;
				object result;
				PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, (object o) => Type.GetType(GetAttribute<EditorAttribute>((PropertyDescriptor)o).EditorTypeName), out result);
				if (result != null)
				{
					key = GetAttribute<EditorAttribute>(PropertyDescriptors.First()).EditorTypeName;
				}
				if (!_dictEditorTypeName.TryGetValue(key, out value))
				{
					try
					{
						PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, delegate(object o)
						{
							string[] typeDef = GetAttribute<EditorAttribute>((PropertyDescriptor)o).EditorTypeName.Split(',');
							if (typeDef.Length >= 2)
							{
								Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.FullName.Contains(typeDef[1].Trim()));
								if (assembly != null)
								{
									return assembly.GetTypes().FirstOrDefault(delegate(Type t)
									{
										if (t != null && t.FullName != null)
										{
											return t.FullName.Contains(typeDef[0]);
										}
										return false;
									});
								}
							}
							return null;
						}, out value);
					}
					catch (Exception)
					{
					}
					if (value == null)
					{
						PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, (object o) => Type.GetType(GetAttribute<EditorAttribute>((PropertyDescriptor)o).EditorTypeName), out value);
					}
					_dictEditorTypeName.Add(key, value);
				}
				Type type = value as Type;
				if (type != null)
				{
					object obj = Activator.CreateInstance(type);
					if (obj is ITypeEditor)
					{
						return (ITypeEditor)obj;
					}
				}
			}
			if (IsAttributePresentForAllSelectedObjects<ItemsSourceAttribute>())
			{
				object result2 = null;
				PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, (object o) => GetAttribute<ItemsSourceAttribute>((PropertyDescriptor)o), out result2);
				ItemsSourceAttribute itemsSourceAttribute = result2 as ItemsSourceAttribute;
				if (itemsSourceAttribute != null)
				{
					return new ItemsSourceAttributeEditor(itemsSourceAttribute);
				}
			}
			return null;
		}

		private T GetAttribute<T>(PropertyDescriptor pd) where T : Attribute
		{
			return PropertyGridUtilities.GetAttribute<T>(pd);
		}

		private bool IsAttributePresentForAllSelectedObjects<T>() where T : Attribute
		{
			object result = null;
			PropertyGridUtilities.IsSameForAllObject(PropertyDescriptors, (object o) => GetAttribute<T>((PropertyDescriptor)o) != null, out result);
			if (result != null)
			{
				return (bool)result;
			}
			return false;
		}
	}
}
