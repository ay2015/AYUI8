using System;

namespace ay.Controls.Args
{
	/// <summary>Arguments for the TokenizedTextBox.QueryTextFromItem event.</summary>
	public class QueryTextFromItemEventArgs : EventArgs
	{
		/// <summary>The item corresponding to the maching text.</summary>
		public object Item
		{
			get;
			private set;
		}

		/// <summary>The text corresponding to the maching item. This value should be set by the user to specify the desired text representation of the item.</summary>
		public string Text
		{
			get;
			set;
		}

		public QueryTextFromItemEventArgs(object item)
		{
			Item = item;
		}
	}
}
