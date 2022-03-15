using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Base class of the "editor definitions."</summary>
	public abstract class EditorDefinitionBase : PropertyDefinitionBase
	{
		internal FrameworkElement GenerateEditingElementInternal(PropertyItemBase propertyItem)
		{
			return GenerateEditingElement(propertyItem);
		}

		protected virtual FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			return null;
		}

		internal void UpdateProperty(FrameworkElement element, DependencyProperty elementProp, DependencyProperty definitionProperty)
		{
			object value = GetValue(definitionProperty);
			object obj = ReadLocalValue(definitionProperty);
			object value2 = element.GetValue(elementProp);
			bool flag = false;
			if (obj != DependencyProperty.UnsetValue)
			{
				if (value2 != null && value != null)
				{
					flag = ((value2.GetType().IsValueType && value.GetType().IsValueType) ? value2.Equals(value) : (value == element.GetValue(elementProp)));
				}
				if (!flag)
				{
					element.SetValue(elementProp, value);
				}
				else
				{
					element.ClearValue(elementProp);
				}
			}
		}
	}
}
