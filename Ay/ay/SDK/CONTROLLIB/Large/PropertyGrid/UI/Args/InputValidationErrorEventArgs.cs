using System;

namespace Xceed.Wpf.Toolkit.Core.Input
{
    /// <summary>Represents the method that will handle the InputValidationError event.</summary>
    public delegate void InputValidationErrorEventHandler(object sender, InputValidationErrorEventArgs e);
    /// <summary>Provides information used in the InputValidationError
    /// event.</summary>
    public class InputValidationErrorEventArgs : EventArgs
	{
		private Exception exception;

		private bool _throwException;

		public Exception Exception
		{
			get
			{
				return exception;
			}
			private set
			{
				exception = value;
			}
		}

		public bool ThrowException
		{
			get
			{
				return _throwException;
			}
			set
			{
				_throwException = value;
			}
		}

		public InputValidationErrorEventArgs(Exception e)
		{
			Exception = e;
		}
	}
}
