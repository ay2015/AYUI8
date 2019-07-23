using System.Windows;

namespace ay.Controls.Args
{
	/// <summary>
	///   <font size="2">Argument used in the InvalidValueEntered event to obtain the invalid string.</font>
	/// </summary>
	public class InvalidValueEventArgs : RoutedEventArgs
	{
		/// <summary>
		///   <font size="2">Gets the invalid string entered.</font>
		/// </summary>
		public string Text
		{
			get;
			private set;
		}

		public InvalidValueEventArgs(RoutedEvent routedEvent, object source, string text)
			: base(routedEvent, source)
		{
			Text = text;
		}
	}
}
