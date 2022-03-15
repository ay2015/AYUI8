using System;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>
	///   <para>Represents an attribute that is set on a property to identify the order in which the decorated property will appear relative to the other properties.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public class PropertyOrderAttribute : Attribute
	{
		/// <summary>Gets or sets the order of the property.</summary>
		public int Order
		{
			get;
			set;
		}

		public UsageContextEnum UsageContext
		{
			get;
			set;
		}

		public override object TypeId
		{
			get
			{
				return this;
			}
		}

		public PropertyOrderAttribute(int order)
			: this(order, UsageContextEnum.Both)
		{
		}

		public PropertyOrderAttribute(int order, UsageContextEnum usageContext)
		{
			Order = order;
			UsageContext = usageContext;
		}
	}
}
