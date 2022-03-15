using System.Collections.Generic;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>Represents a collection of Item instances.</summary>
	public class ItemCollection : List<Item>
	{
		/// <summary>Adds an object to the ItemCollection.</summary>
		/// <param name="value">The value to add.</param>
		public void Add(object value)
		{
			Item item = new Item();
			item.DisplayName = value.ToString();
			item.Value = value;
			base.Add(item);
		}

		/// <summary>Adds an object to the ItemCollection, specifying it display name.</summary>
		/// <param name="value">The value to add.</param>
		/// <param name="displayName">The display name of the item to use.</param>
		public void Add(object value, string displayName)
		{
			Item item = new Item();
			item.DisplayName = displayName;
			item.Value = value;
			base.Add(item);
		}
	}
}
