using System;

namespace ay.Controls.Args
{
	/// <summary>Arguments for the TokenizedTextBox.QueryItemFromText event.</summary>
	public class QueryItemFromTextEventArgs : EventArgs
	{
		/// <summary>The item corresponding to the maching text. This value should be set by the user if there is a matching item for the specified text.</summary>
		public object Item
		{
			get;
			set;
		}

		/// <summary>The text corresponding to the maching item.</summary>
		public string Text
		{
			get;
			private set;
		}

		public QueryItemFromTextEventArgs(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			Text = text;
		}
	}
}
