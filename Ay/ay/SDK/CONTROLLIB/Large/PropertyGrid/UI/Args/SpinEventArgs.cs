using System.Windows;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Provides information on spin events.</summary>
	public class SpinEventArgs : RoutedEventArgs
	{
		/// <summary>Gets the spin direction.</summary>
		public SpinDirection Direction
		{
			get;
			private set;
		}

		/// <summary>Get or set whether the spin event originated from a mouse wheel event.</summary>
		public bool UsingMouseWheel
		{
			get;
			private set;
		}

		public SpinEventArgs(SpinDirection direction)
		{
			Direction = direction;
		}

		public SpinEventArgs(RoutedEvent routedEvent, SpinDirection direction)
			: base(routedEvent)
		{
			Direction = direction;
		}

		public SpinEventArgs(SpinDirection direction, bool usingMouseWheel)
		{
			Direction = direction;
			UsingMouseWheel = usingMouseWheel;
		}

		public SpinEventArgs(RoutedEvent routedEvent, SpinDirection direction, bool usingMouseWheel)
			: base(routedEvent)
		{
			Direction = direction;
			UsingMouseWheel = usingMouseWheel;
		}
	}
}
