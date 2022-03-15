using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in FontComboBox editor.</summary>
	public class FontComboBoxEditor : ComboBoxEditor
	{
		/// <summary>
		///   <span class="st">Creates a collection used to generate a list for the content of the editor using the PropertyType of the passed <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem.html">PropertyItem</see>.</span>
		/// </summary>
		/// <returns>The created list.</returns>
		/// <param name="propertyItem">The PropertyItem whose PropertyType will be used to determine the list that is created.</param>
		protected override IEnumerable CreateItemsSource(PropertyItem propertyItem)
		{
			if (propertyItem.PropertyType == typeof(FontFamily))
			{
				return from x in FontUtilities.Families
				orderby x.Source
				select x;
			}
			if (propertyItem.PropertyType == typeof(FontWeight))
			{
				return FontUtilities.Weights;
			}
			if (propertyItem.PropertyType == typeof(FontStyle))
			{
				return FontUtilities.Styles;
			}
			if (propertyItem.PropertyType == typeof(FontStretch))
			{
				return FontUtilities.Stretches;
			}
			return null;
		}
	}
}
