using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Provides informations related to the expandable status of a propertyItem in a PropertyGrid.</summary>
	public class IsPropertyExpandableArgs : PropertyArgs
	{
		/// <summary>Gets or sets if the PropertyItem will be expandable or not.</summary>
		public bool? IsExpandable
		{
			get;
			set;
		}

		public IsPropertyExpandableArgs(PropertyDescriptor pd)
			: base(pd)
		{
		}
	}
}
