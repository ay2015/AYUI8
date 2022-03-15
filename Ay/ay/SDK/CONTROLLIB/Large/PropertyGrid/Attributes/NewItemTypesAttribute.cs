using System;
using System.Collections.Generic;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>This attribute can decorate the collection properties (i.e., IList) of your selected object in order to control the types that will be allowedto be
	/// instantiated in the CollectionControl.</summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NewItemTypesAttribute : Attribute
	{
		public IList<Type> Types
		{
			get;
			set;
		}

		public NewItemTypesAttribute(params Type[] types)
		{
			Types = new List<Type>(types);
		}

		public NewItemTypesAttribute()
		{
		}
	}
}
