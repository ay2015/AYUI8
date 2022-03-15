using System;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>Represents an attribute that is set on a property to identify it as a complex, expandable property.</summary>
	public class ExpandableObjectAttribute : Attribute
	{
		public bool IsExpanded
		{
			get;
			set;
		}

		/// <summary>Initializes a new instance of the ExpandableObjectAttribute class.</summary>
		public ExpandableObjectAttribute()
		{
		}

		public ExpandableObjectAttribute(bool isExpanded)
			: this()
		{
			
			IsExpanded = isExpanded;
		}
	}
}
