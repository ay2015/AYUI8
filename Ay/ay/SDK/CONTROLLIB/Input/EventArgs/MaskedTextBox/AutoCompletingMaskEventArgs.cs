using System.ComponentModel;

namespace ay.Controls.Args
{
	/// <summary>Provides information on input text whose mask is being automatically completed.</summary>
	public class AutoCompletingMaskEventArgs : CancelEventArgs
	{
		private MaskedTextProvider m_maskedTextProvider;

		private int m_startPosition;

		private int m_selectionLength;

		private string m_input;

		private int m_autoCompleteStartPosition;

		private string m_autoCompleteText;

		/// <summary>Gets a copy of the <strong>MaskTextProvider</strong> that was used to mask the input text.</summary>
		public MaskedTextProvider MaskedTextProvider
		{
			get
			{
				return m_maskedTextProvider;
			}
		}

		/// <summary>Gets the position at which the inputted text starts.</summary>
		public int StartPosition
		{
			get
			{
				return m_startPosition;
			}
		}

		/// <summary>
		///   <para>Gets a value indicating the number of selected characters.</para>
		/// </summary>
		public int SelectionLength
		{
			get
			{
				return m_selectionLength;
			}
		}

		/// <summary>Gets the input text.</summary>
		public string Input
		{
			get
			{
				return m_input;
			}
		}

		/// <summary>Gets or sets a value indicating the position at which the automatic completion of the mask starts.</summary>
		public int AutoCompleteStartPosition
		{
			get
			{
				return m_autoCompleteStartPosition;
			}
			set
			{
				m_autoCompleteStartPosition = value;
			}
		}

		/// <summary>Gets or sets the text that will be used to automatically complete the mask.</summary>
		public string AutoCompleteText
		{
			get
			{
				return m_autoCompleteText;
			}
			set
			{
				m_autoCompleteText = value;
			}
		}

		/// <summary>Initializes a new instance of the <strong>AutoCompletingMaskEventArgs</strong> class specifying the input text as well as the information required to
		/// automatically complete the mask.</summary>
		/// <param name="maskedTextProvider">A copy of the <strong>MaskTextProvider</strong> that was used to mask the inputted text.</param>
		/// <param name="startPosition">A zero-based value indicating the position at which the automatic completion of the mask starts.</param>
		/// <param name="selectionLength">A value indicating the number of selected characters.</param>
		/// <param name="input">The input text.</param>
		public AutoCompletingMaskEventArgs(MaskedTextProvider maskedTextProvider, int startPosition, int selectionLength, string input)
		{
			m_autoCompleteStartPosition = -1;
			m_maskedTextProvider = maskedTextProvider;
			m_startPosition = startPosition;
			m_selectionLength = selectionLength;
			m_input = input;
		}
	}
}
