using System.Windows;
using Xceed.Wpf.Toolkit.Core;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Provides event information for the ItemDeleting event.</summary>
	public class ItemDeletingEventArgs : CancelRoutedEventArgs
	{
		private object _item;

		/// <summary>Gets the item being deleted.</summary>
		public object Item
		{
			get
			{
				return _item;
			}
		}

		public ItemDeletingEventArgs(RoutedEvent itemDeletingEvent, object itemDeleting)
			: base(itemDeletingEvent)
		{
			_item = itemDeleting;
		}
	}
}
