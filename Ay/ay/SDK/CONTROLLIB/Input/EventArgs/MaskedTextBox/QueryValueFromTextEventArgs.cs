using System;

namespace ay.Controls.Args
{
	public class QueryValueFromTextEventArgs : EventArgs
	{
		private string m_text;

		private object m_value;

		private bool m_hasParsingError;

		public string Text
		{
			get
			{
				return m_text;
			}
		}

		public object Value
		{
			get
			{
				return m_value;
			}
			set
			{
				m_value = value;
			}
		}

		public bool HasParsingError
		{
			get
			{
				return m_hasParsingError;
			}
			set
			{
				m_hasParsingError = value;
			}
		}

		public QueryValueFromTextEventArgs(string text, object value)
		{
			m_text = text;
			m_value = value;
		}
	}
}
