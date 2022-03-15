using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in EnumComboBox editor.</summary>
	public class EnumComboBoxEditor : ComboBoxEditor
	{
		/// <summary>
		///   <span class="st">Creates a collection used to generate a list for the content of the editor using the PropertyType of the passed <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem.html">PropertyItem</see>.</span>
		/// </summary>
		/// <returns>The created list.</returns>
		/// <param name="propertyItem">The PropertyItem whose PropertyType will be used to determine the list that is created.</param>
		protected override IEnumerable CreateItemsSource(PropertyItem propertyItem)
		{
			return GetValues(propertyItem.PropertyType);
		}

		private static object[] GetValues(Type enumType)
		{
			List<object> list = new List<object>();
			if (enumType != null)
			{
				IEnumerable<FieldInfo> enumerable = from x in enumType.GetFields()
				where x.IsLiteral
				select x;
				foreach (FieldInfo item in enumerable)
				{
					object[] customAttributes = item.GetCustomAttributes(typeof(BrowsableAttribute), false);
					if (customAttributes.Length == 1)
					{
						BrowsableAttribute browsableAttribute = (BrowsableAttribute)customAttributes[0];
						if (!browsableAttribute.Browsable)
						{
							continue;
						}
					}
					list.Add(item.GetValue(enumType));
				}
			}
			return list.ToArray();
		}
	}
}
