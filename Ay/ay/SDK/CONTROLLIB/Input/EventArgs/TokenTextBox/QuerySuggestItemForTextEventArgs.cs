using System;

namespace ay.Controls.Args
{
	/// <summary>Arguments for the TokenizedTextBox.QuerySuggestItemForText event.</summary>
	public class QuerySuggestItemForTextEventArgs : EventArgs
	{
		/// <summary>The item corresponding to the maching text.</summary>
		public object Item
		{
			get;
			private set;
		}

		/// <summary>The text corresponding to the current input text.</summary>
		public string Text
		{
			get;
			private set;
		}

		/// <summary>This value should be set to True to specify if the provided item should be displayed in the dropdown suggestion box, taking into account the actual input
		/// provided by the QuerySuggestItemForTextEventArgs.Text property.</summary>
		public bool? SuggestItem
		{
			get;
			set;
		}

		public QuerySuggestItemForTextEventArgs(object item, string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			Item = item;
		}
	}
}
