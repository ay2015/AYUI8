using System.Collections;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in ComboBox editor.</summary>
	public abstract class ComboBoxEditor : TypeEditor<ComboBox>
	{
		/// <summary>Sets the value dependency property.</summary>
		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = Selector.SelectedItemProperty;
		}

		protected override ComboBox CreateEditor()
		{
			return new PropertyGridEditorComboBox();
		}

		/// <summary>Resolves the binding of the value property of the passed PropertyItem. The
		/// PropertyItem whose value property binding will be resolved.</summary>
		protected override void ResolveValueBinding(PropertyItem propertyItem)
		{
			SetItemsSource(propertyItem);
			base.ResolveValueBinding(propertyItem);
		}

		/// <summary>
		///   <span class="st">Creates a collection used to generate a list for the content of the editor using the PropertyType of the passed <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem.html">PropertyItem</see>.</span>
		/// </summary>
		/// <returns>The created list.</returns>
		/// <param name="propertyItem">The PropertyItem whose PropertyType will be used to determine the list that is created.</param>
		protected abstract IEnumerable CreateItemsSource(PropertyItem propertyItem);

		private void SetItemsSource(PropertyItem propertyItem)
		{
			base.Editor.ItemsSource = CreateItemsSource(propertyItem);
		}
	}
}
