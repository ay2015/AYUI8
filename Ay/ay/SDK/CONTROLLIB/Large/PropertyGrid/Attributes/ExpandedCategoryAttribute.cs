using System;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ExpandedCategoryAttribute : Attribute
	{
		public bool IsExpanded
		{
			get;
			set;
		}

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

		public ExpandedCategoryAttribute()
		{
			
		}

		public ExpandedCategoryAttribute(string categoryName, bool isExpanded)
			: this()
		{
			CategoryValue = categoryName;
			IsExpanded = isExpanded;
		}
	}
}
