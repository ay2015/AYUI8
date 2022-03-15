using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal interface IPropertyContainer
	{
		Binding PropertyNameBinding
		{
			get;
		}

		Binding PropertyValueBinding
		{
			get;
		}

		EditorDefinitionBase DefaultEditorDefinition
		{
			get;
		}

		GroupDescription CategoryGroupDescription
		{
			get;
		}

		CategoryDefinitionCollection CategoryDefinitions
		{
			get;
		}

		List<KeyValuePair<string, PropertyItem>> DependsOnPropertyItemsList
		{
			get;
		}

		bool IsExpandingNonPrimitiveTypes
		{
			get;
		}

		ContainerHelperBase ContainerHelper
		{
			get;
		}

		Style PropertyContainerStyle
		{
			get;
		}

		EditorDefinitionCollection EditorDefinitions
		{
			get;
		}

		PropertyDefinitionCollection PropertyDefinitions
		{
			get;
		}

		bool IsCategorized
		{
			get;
		}

		bool IsSortedAlphabetically
		{
			get;
		}

		bool AutoGenerateProperties
		{
			get;
		}

		bool HideInheritedProperties
		{
			get;
		}

		FilterInfo FilterInfo
		{
			get;
		}

		bool? IsPropertyVisible(PropertyDescriptor pd);

		bool? CanExpandProperty(PropertyDescriptor pd);
	}
}
