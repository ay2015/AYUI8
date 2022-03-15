using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>
	///   <font size="2">Provides informations related to the visibility of a propertyItem in a PropertyGrid.</font>
	/// </summary>
	public class IsPropertyBrowsableArgs : PropertyArgs
	{
		/// <summary>
		///   <font size="2">Gets or sets if the PropertyItem will be visible or not.</font>
		/// </summary>
		public bool? IsBrowsable
		{
			get;
			set;
		}

		public IsPropertyBrowsableArgs(PropertyDescriptor pd)
			: base(pd)
		{
		}
	}
}
