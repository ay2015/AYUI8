using System.Windows;

namespace Xceed.Wpf.Toolkit.Core
{
	/// <summary>An event data class that allows to inform the sender that the handler wants to cancel the ongoing action. The handler can set the Cancel property to false to cancel the action.</summary>
	public class CancelRoutedEventArgs : RoutedEventArgs
	{
		public bool Cancel
		{
			get;
			set;
		}

		public CancelRoutedEventArgs()
		{
		}

		public CancelRoutedEventArgs(RoutedEvent routedEvent)
			: base(routedEvent)
		{
		}

		public CancelRoutedEventArgs(RoutedEvent routedEvent, object source)
			: base(routedEvent, source)
		{
		}
	}
}
