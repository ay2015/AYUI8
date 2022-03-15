using System.Windows;
using Xceed.Wpf.Toolkit.Core;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Provides event information for the ItemAdding event.</summary>
	public class ItemAddingEventArgs : CancelRoutedEventArgs
	{
		/// <summary>Gets the item being added.</summary>
		public object Item
		{
			get;
			set;
		}

		public ItemAddingEventArgs(RoutedEvent itemAddingEvent, object itemAdding)
			: base(itemAddingEvent)
		{
			Item = itemAdding;
		}
	}
}
