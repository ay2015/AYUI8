using System;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>Represents an attribute that is set on a property to identify the IItemsSource-derived class that will be used.</summary>
	public class ItemsSourceAttribute : Attribute
	{
		/// <summary>Gets or sets the type to use.</summary>
		public Type Type
		{
			get;
			set;
		}

		/// <summary>Initializes a new instance of the ItemsSourceAttribute class using the specified Type.</summary>
		/// <param name="type">The Type to use.</param>
		public ItemsSourceAttribute(Type type)
		{
			Type @interface = type.GetInterface(typeof(IItemsSource).FullName);
			if (@interface == null)
			{
				throw new ArgumentException("Type must implement the IItemsSource interface.", "type");
			}
			Type = type;
		}
	}
}
