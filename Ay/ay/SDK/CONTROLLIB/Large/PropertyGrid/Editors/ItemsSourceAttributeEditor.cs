using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in ItemsSourceAttribute editor.</summary>
	public class ItemsSourceAttributeEditor : TypeEditor<ComboBox>
	{
		private readonly ItemsSourceAttribute _attribute;

		/// <summary>Initializes a new instance of the CheckBoxEditor class.</summary>
		public ItemsSourceAttributeEditor(ItemsSourceAttribute attribute)
		{
			_attribute = attribute;
		}

		/// <summary>Sets the value dependency property.</summary>
		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = Selector.SelectedValueProperty;
		}

		protected override ComboBox CreateEditor()
		{
			return new PropertyGridEditorComboBox();
		}

		/// <summary>Resolves the binding of the value property of the passed PropertyItem.</summary>
		/// <param name="propertyItem">The PropertyItem whose value property binding will be resolved.</param>
		protected override void ResolveValueBinding(PropertyItem propertyItem)
		{
			SetItemsSource();
			base.ResolveValueBinding(propertyItem);
		}

		/// <summary>Sets the properties of the control.</summary>
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			base.Editor.DisplayMemberPath = "DisplayName";
			base.Editor.SelectedValuePath = "Value";
			if (propertyItem != null)
			{
				base.Editor.IsEnabled = !propertyItem.IsReadOnly;
			}
		}

		private void SetItemsSource()
		{
			base.Editor.ItemsSource = CreateItemsSource();
		}

		private IEnumerable CreateItemsSource()
		{
			object obj = Activator.CreateInstance(_attribute.Type);
			return (obj as IItemsSource).GetValues();
		}
	}
}
