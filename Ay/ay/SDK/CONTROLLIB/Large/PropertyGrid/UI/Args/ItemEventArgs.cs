using System.Windows;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Provides event data for the ItemAdded and <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.CollectionControl~ItemDeleted_EV.html">ItemDeleted</see> events.</summary>
	public class ItemEventArgs : RoutedEventArgs
	{
		private object _item;

		/// <summary>Gets the added/deleted item.</summary>
		public object Item
		{
			get
			{
				return _item;
			}
		}

		internal ItemEventArgs(RoutedEvent routedEvent, object newItem)
			: base(routedEvent)
		{
			_item = newItem;
		}
	}
}
