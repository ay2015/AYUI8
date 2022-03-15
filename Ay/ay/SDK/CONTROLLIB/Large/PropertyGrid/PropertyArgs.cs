using System.ComponentModel;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>A RoutedEventArgs with a PropertyDescriptor property.</summary>
	public class PropertyArgs : RoutedEventArgs
	{
		/// <summary>Gets the PropertyDescriptor of the PropertyItem to obtain informations in order to set the propertyItem as browsable or expandable.</summary>
		public PropertyDescriptor PropertyDescriptor
		{
			get;
			private set;
		}

		public PropertyArgs(PropertyDescriptor pd)
		{
			PropertyDescriptor = pd;
		}
	}
}
