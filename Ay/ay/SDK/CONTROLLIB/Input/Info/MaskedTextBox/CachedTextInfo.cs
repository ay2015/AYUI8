using System;
using System.Windows.Controls;

namespace ay.Controls.Info
{
	internal class CachedTextInfo : ICloneable
	{
		public string Text
		{
			get;
			private set;
		}

		public int CaretIndex
		{
			get;
			private set;
		}

		public int SelectionStart
		{
			get;
			private set;
		}

		public int SelectionLength
		{
			get;
			private set;
		}

		private CachedTextInfo(string text, int caretIndex, int selectionStart, int selectionLength)
		{
			Text = text;
			CaretIndex = caretIndex;
			SelectionStart = selectionStart;
			SelectionLength = selectionLength;
		}

		public CachedTextInfo(TextBox textBox)
			: this(textBox.Text, textBox.CaretIndex, textBox.SelectionStart, textBox.SelectionLength)
		{
		}

		public object Clone()
		{
			return new CachedTextInfo(Text, CaretIndex, SelectionStart, SelectionLength);
		}
	}
}
