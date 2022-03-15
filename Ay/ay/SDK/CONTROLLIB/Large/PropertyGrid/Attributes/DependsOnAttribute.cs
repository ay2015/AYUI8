using System;
using System.Collections.Generic;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>Takes a list of strings in parameter that represent the names of properties that affect the current property. It is placed over any properties of the
	/// PropertyGrid's selectedObject.</summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public class DependsOnAttribute : Attribute
	{
		public List<string> PropertyItemNames
		{
			get;
			set;
		}

		public DependsOnAttribute()
		{
			
		}

		public DependsOnAttribute(params string[] propertyItemsName)
		{
			PropertyItemNames = new List<string>();
			foreach (string item in propertyItemsName)
			{
				PropertyItemNames.Add(item);
			}
		}
	}
}
