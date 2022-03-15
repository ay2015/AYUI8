using System;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>
	///   <para>Represents an attribute that is set on a property to identify the order in which the decorated property's category appears in the PropertyGrid when the
	/// latter is set to Categorized mode.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class CategoryOrderAttribute : Attribute
	{
		/// <summary>Gets or sets a value representing the category's relative order.</summary>
		public int Order
		{
			get;
			set;
		}

		/// <summary>Gets or sets the name of the property's category.</summary>
		public virtual string Category
		{
			get
			{
				return CategoryValue;
			}
		}

		public string CategoryValue
		{
			get;
			private set;
		}

		/// <summary>
		///   <para>Initializes a new instance of the CategoryOrderAttribute class, specifying the order of the decorated property's category.</para>
		/// </summary>
		public CategoryOrderAttribute()
		{
		}

		/// <summary>
		///   <para>Initializes a new instance of the CategoryOrderAttribute class, specifying the order of the decorated property's category.</para>
		/// </summary>
		/// <param name="categoryName">The name of the property's category.</param>
		/// <param name="order">The order of the property.</param>
		public CategoryOrderAttribute(string categoryName, int order)
			: this()
		{
			CategoryValue = categoryName;
			Order = order;
		}
	}
}
