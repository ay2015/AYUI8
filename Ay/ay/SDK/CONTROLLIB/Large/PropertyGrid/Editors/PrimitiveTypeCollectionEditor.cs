using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in PrimitiveTypeCollection editor.</summary>
	public class PrimitiveTypeCollectionEditor : TypeEditor<PrimitiveTypeCollectionControl>
	{
		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.Editor.BorderThickness = new Thickness(0.0);
			base.Editor.Content = "(Collection)";
		}

		/// <summary>Sets the value dependency property.</summary>
		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = PrimitiveTypeCollectionControl.ItemsSourceProperty;
		}

		protected override PrimitiveTypeCollectionControl CreateEditor()
		{
			return new PropertyGridEditorPrimitiveTypeCollectionControl();
		}

		/// <summary>Resolves the binding of the value property of the passed PropertyItem.</summary>
		/// <param name="propertyItem">The PropertyItem whose value property binding will be resolved.</param>
		protected override void ResolveValueBinding(PropertyItem propertyItem)
		{
			Type propertyType = propertyItem.PropertyType;
			base.Editor.ItemsSourceType = propertyType;
			if (propertyType.BaseType == typeof(Array))
			{
				base.Editor.ItemType = propertyType.GetElementType();
			}
			else
			{
				Type[] genericArguments = propertyType.GetGenericArguments();
				if (genericArguments.Length > 0)
				{
					base.Editor.ItemType = genericArguments[0];
				}
			}
			base.ResolveValueBinding(propertyItem);
		}
	}
}
