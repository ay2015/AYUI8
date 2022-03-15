using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Represents event data for the ClearPropertyItem and
	/// <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid~PreparePropertyItem_EV.html">PreparePropertyItem</see> events.</summary>
	public class PropertyItemEventArgs : RoutedEventArgs
	{
		public PropertyItemBase PropertyItem
		{
			get;
			private set;
		}

		public object Item
		{
			get;
			private set;
		}

		public PropertyItemEventArgs(RoutedEvent routedEvent, object source, PropertyItemBase propertyItem, object item)
			: base(routedEvent, source)
		{
			PropertyItem = propertyItem;
			Item = item;
		}
	}
}
