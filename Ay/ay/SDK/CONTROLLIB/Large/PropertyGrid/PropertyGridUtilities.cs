using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class PropertyGridUtilities
	{
		private class EditorTypeDescriptorContext : ITypeDescriptorContext, IServiceProvider
		{
			private IContainer _container;

			private object _instance;

			private PropertyDescriptor _propertyDescriptor;

			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return _container;
				}
			}

			object ITypeDescriptorContext.Instance
			{
				get
				{
					return _instance;
				}
			}

			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return _propertyDescriptor;
				}
			}

			internal EditorTypeDescriptorContext(IContainer container, object instance, PropertyDescriptor pd)
			{
				_container = container;
				_instance = instance;
				_propertyDescriptor = pd;
			}

			void ITypeDescriptorContext.OnComponentChanged()
			{
			}

			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}

			object IServiceProvider.GetService(Type serviceType)
			{
				return null;
			}
		}

		internal static T GetAttribute<T>(PropertyDescriptor property) where T : Attribute
		{
			return property.Attributes.OfType<T>().FirstOrDefault();
		}

		internal static bool IsSameForAllObject(IEnumerable objectList, Func<object, object> f, out object result)
		{
			result = null;
			bool flag = false;
			foreach (object @object in objectList)
			{
				object obj = f(@object);
				if (!flag)
				{
					result = obj;
					flag = true;
				}
				else if (!object.Equals(result, obj))
				{
					result = null;
					return false;
				}
			}
			return true;
		}

		internal static ITypeEditor CreateDefaultEditor(Type propertyType, TypeConverter typeConverter, PropertyItem propertyItem)
		{
			ITypeEditor typeEditor = null;
			EditorTypeDescriptorContext context = new EditorTypeDescriptorContext(null, propertyItem.Instance, propertyItem.PropertyDescriptor);
			if (typeConverter != null && typeConverter.GetStandardValuesSupported(context) && typeConverter.GetStandardValuesExclusive(context) && !(typeConverter is ReferenceConverter) && propertyType != typeof(bool) && propertyType != typeof(bool?))
			{
				TypeConverter.StandardValuesCollection standardValues = typeConverter.GetStandardValues(context);
				return new SourceComboBoxEditor(standardValues, typeConverter);
			}
			if (propertyType == typeof(string))
			{
				return new TextBoxEditor();
			}
			if (propertyType == typeof(bool) || propertyType == typeof(bool?))
			{
				return new CheckBoxEditor();
			}
			if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
			{
				return new DecimalUpDownEditor();
			}
			if (propertyType == typeof(double) || propertyType == typeof(double?))
			{
				return new DoubleUpDownEditor();
			}
			if (propertyType == typeof(int) || propertyType == typeof(int?))
			{
				return new IntegerUpDownEditor();
			}
			if (propertyType == typeof(short) || propertyType == typeof(short?))
			{
				return new ShortUpDownEditor();
			}
			if (propertyType == typeof(long) || propertyType == typeof(long?))
			{
				return new LongUpDownEditor();
			}
			if (propertyType == typeof(float) || propertyType == typeof(float?))
			{
				return new SingleUpDownEditor();
			}
			if (propertyType == typeof(byte) || propertyType == typeof(byte?))
			{
				return new ByteUpDownEditor();
			}
			if (propertyType == typeof(sbyte) || propertyType == typeof(sbyte?))
			{
				return new SByteUpDownEditor();
			}
			if (propertyType == typeof(uint) || propertyType == typeof(uint?))
			{
				return new UIntegerUpDownEditor();
			}
			if (propertyType == typeof(ulong) || propertyType == typeof(ulong?))
			{
				return new ULongUpDownEditor();
			}
			if (propertyType == typeof(ushort) || propertyType == typeof(ushort?))
			{
				return new UShortUpDownEditor();
			}
			if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
			{
				return new DateTimeUpDownEditor();
			}
			if (propertyType == typeof(Color) || propertyType == typeof(Color?))
			{
				return new ColorEditor();
			}
			if (propertyType == typeof(FileInfo))
			{
				return new FileEditor();
			}
			if (propertyType.IsEnum)
			{
				return new EnumComboBoxEditor();
			}
			if (propertyType == typeof(TimeSpan) || propertyType == typeof(TimeSpan?))
			{
				return new TimeSpanUpDownEditor();
			}
			if (propertyType == typeof(FontFamily) || propertyType == typeof(FontWeight) || propertyType == typeof(FontStyle) || propertyType == typeof(FontStretch))
			{
				return new FontComboBoxEditor();
			}
			if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
			{
				MaskedTextBoxEditor maskedTextBoxEditor = new MaskedTextBoxEditor();
				maskedTextBoxEditor.ValueDataType = propertyType;
				maskedTextBoxEditor.Mask = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA";
				return maskedTextBoxEditor;
			}
			if (propertyType == typeof(char) || propertyType == typeof(char?))
			{
				MaskedTextBoxEditor maskedTextBoxEditor2 = new MaskedTextBoxEditor();
				maskedTextBoxEditor2.ValueDataType = propertyType;
				maskedTextBoxEditor2.Mask = "&";
				return maskedTextBoxEditor2;
			}
			if (propertyType == typeof(object))
			{
				return new TextBoxEditor();
			}
			Type listItemType = ListUtilities.GetListItemType(propertyType);
			if (listItemType != null)
			{
				if (!listItemType.IsPrimitive && !listItemType.Equals(typeof(string)) && !listItemType.IsEnum)
				{
					return new CollectionEditor();
				}
				return new PrimitiveTypeCollectionEditor();
			}
			Type[] dictionaryItemsType = ListUtilities.GetDictionaryItemsType(propertyType);
			Type collectionItemType = ListUtilities.GetCollectionItemType(propertyType);
			if (dictionaryItemsType != null || collectionItemType != null || typeof(ICollection).IsAssignableFrom(propertyType))
			{
				return new CollectionEditor();
			}
			object result;
			if (typeConverter == null || !typeConverter.CanConvertFrom(typeof(string)))
			{
				ITypeEditor typeEditor2 = new TextBlockEditor();
				result = typeEditor2;
			}
			else
			{
				result = new TextBoxEditor();
			}
			return (ITypeEditor)result;
		}

		internal static BindingBase GetDefaultBinding(PropertyItemBase propertyItem)
		{
			return propertyItem.ParentNode.ContainerHelper.CreateChildrenDefaultBinding(propertyItem);
		}

		internal static FrameworkElement GenerateSystemDefaultEditingElement(Type propertyType, PropertyItemBase propertyItem)
		{
			EditorDefinitionBase editorDefinitionBase = GetDefaultEditorDefinition(propertyType);
			if (editorDefinitionBase == null)
			{
				IEnumerable defaultComboBoxDefinitionItems = GetDefaultComboBoxDefinitionItems(propertyType);
				if (defaultComboBoxDefinitionItems != null)
				{
					EditorComboBoxDefinition editorComboBoxDefinition = new EditorComboBoxDefinition();
					editorComboBoxDefinition.ItemsSource = defaultComboBoxDefinitionItems;
					editorComboBoxDefinition.SelectedItemBinding = GetDefaultBinding(propertyItem);
					editorDefinitionBase = editorComboBoxDefinition;
				}
				else
				{
					Type listItemType = ListUtilities.GetListItemType(propertyType);
					if (listItemType != null)
					{
						if (!listItemType.IsPrimitive && !listItemType.Equals(typeof(string)) && !listItemType.IsEnum)
						{
							EditorCollectionDefinition editorCollectionDefinition = new EditorCollectionDefinition();
							editorCollectionDefinition.NewItemTypes = new List<Type>
							{
								listItemType
							};
							editorDefinitionBase = editorCollectionDefinition;
						}
						else
						{
							editorDefinitionBase = new EditorPrimitiveTypeCollectionDefinition();
						}
					}
				}
			}
			if (editorDefinitionBase == null)
			{
				return null;
			}
			return editorDefinitionBase.GenerateEditingElementInternal(propertyItem);
		}

		internal static FrameworkElement GenerateSystemDefaultEditingElement(PropertyItemBase propertyItem)
		{
			PropertyGridEditorTextBlock propertyGridEditorTextBlock = new PropertyGridEditorTextBlock();
			propertyGridEditorTextBlock.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
			BindingOperations.SetBinding(propertyGridEditorTextBlock, TextBlock.TextProperty, GetDefaultBinding(propertyItem));
			return propertyGridEditorTextBlock;
		}

		internal static EditorDefinitionBase GetDefaultEditorDefinition(Type propertyType)
		{
			Func<Type, object> func = delegate(Type t)
			{
				if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
				{
					return Activator.CreateInstance(t);
				}
				return null;
			};
			if (propertyType == typeof(string))
			{
				return new EditorTextDefinition();
			}
			if (propertyType == typeof(bool) || propertyType == typeof(bool?))
			{
				EditorCheckBoxDefinition editorCheckBoxDefinition = new EditorCheckBoxDefinition();
				editorCheckBoxDefinition.IsThreeState = (propertyType == typeof(bool?));
				return editorCheckBoxDefinition;
			}
			if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
			{
				EditorDateTimeUpDownDefinition editorDateTimeUpDownDefinition = new EditorDateTimeUpDownDefinition();
				editorDateTimeUpDownDefinition.DefaultValue = (DateTime?)func(propertyType);
				return editorDateTimeUpDownDefinition;
			}
			if (propertyType == typeof(Color))
			{
				return new EditorColorPickerDefinition();
			}
			if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
			{
				EditorDecimalUpDownDefinition editorDecimalUpDownDefinition = new EditorDecimalUpDownDefinition();
				editorDecimalUpDownDefinition.DefaultValue = (decimal?)func(propertyType);
				return editorDecimalUpDownDefinition;
			}
			if (propertyType == typeof(double) || propertyType == typeof(double?))
			{
				EditorDoubleUpDownDefinition editorDoubleUpDownDefinition = new EditorDoubleUpDownDefinition();
				editorDoubleUpDownDefinition.DefaultValue = (double?)func(propertyType);
				return editorDoubleUpDownDefinition;
			}
			if (propertyType == typeof(float) || propertyType == typeof(float?))
			{
				EditorSingleUpDownDefinition editorSingleUpDownDefinition = new EditorSingleUpDownDefinition();
				editorSingleUpDownDefinition.DefaultValue = (float?)func(propertyType);
				return editorSingleUpDownDefinition;
			}
			if (propertyType == typeof(byte) || propertyType == typeof(byte?))
			{
				EditorByteUpDownDefinition editorByteUpDownDefinition = new EditorByteUpDownDefinition();
				editorByteUpDownDefinition.DefaultValue = (byte?)func(propertyType);
				return editorByteUpDownDefinition;
			}
			if (propertyType == typeof(sbyte) || propertyType == typeof(sbyte?))
			{
				EditorSByteUpDownDefinition editorSByteUpDownDefinition = new EditorSByteUpDownDefinition();
				editorSByteUpDownDefinition.DefaultValue = (sbyte?)func(propertyType);
				return editorSByteUpDownDefinition;
			}
			if (propertyType == typeof(short) || propertyType == typeof(short?))
			{
				EditorShortUpDownDefinition editorShortUpDownDefinition = new EditorShortUpDownDefinition();
				editorShortUpDownDefinition.DefaultValue = (short?)func(propertyType);
				return editorShortUpDownDefinition;
			}
			if (propertyType == typeof(ushort) || propertyType == typeof(ushort?))
			{
				EditorUShortUpDownDefinition editorUShortUpDownDefinition = new EditorUShortUpDownDefinition();
				editorUShortUpDownDefinition.DefaultValue = (ushort?)func(propertyType);
				return editorUShortUpDownDefinition;
			}
			if (propertyType == typeof(int) || propertyType == typeof(int?))
			{
				EditorIntegerUpDownDefinition editorIntegerUpDownDefinition = new EditorIntegerUpDownDefinition();
				editorIntegerUpDownDefinition.DefaultValue = (int?)func(propertyType);
				return editorIntegerUpDownDefinition;
			}
			if (propertyType == typeof(uint) || propertyType == typeof(uint?))
			{
				EditorUIntegerUpDownDefinition editorUIntegerUpDownDefinition = new EditorUIntegerUpDownDefinition();
				editorUIntegerUpDownDefinition.DefaultValue = (uint?)func(propertyType);
				return editorUIntegerUpDownDefinition;
			}
			if (propertyType == typeof(long) || propertyType == typeof(long?))
			{
				EditorLongUpDownDefinition editorLongUpDownDefinition = new EditorLongUpDownDefinition();
				editorLongUpDownDefinition.DefaultValue = (long?)func(propertyType);
				return editorLongUpDownDefinition;
			}
			if (propertyType == typeof(ulong) || propertyType == typeof(ulong?))
			{
				EditorULongUpDownDefinition editorULongUpDownDefinition = new EditorULongUpDownDefinition();
				editorULongUpDownDefinition.DefaultValue = (ulong?)func(propertyType);
				return editorULongUpDownDefinition;
			}
			return null;
		}

		internal static IEnumerable GetDefaultComboBoxDefinitionItems(Type propertyType)
		{
			if (propertyType == typeof(FontFamily))
			{
				return FontUtilities.Families;
			}
			if (propertyType == typeof(FontWeight))
			{
				return FontUtilities.Weights;
			}
			if (propertyType == typeof(FontStyle))
			{
				return FontUtilities.Styles;
			}
			if (propertyType == typeof(FontStretch))
			{
				return FontUtilities.Stretches;
			}
			if (propertyType != null && propertyType.IsEnum)
			{
				return Enum.GetValues(propertyType);
			}
			return null;
		}
	}
}
