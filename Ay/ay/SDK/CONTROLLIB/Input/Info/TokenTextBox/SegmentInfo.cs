using System;

namespace ay.Controls.Info
{
	internal abstract class SegmentInfo
	{
		private string _textValue;

		private object _objectValue;

		public bool IsTextSegment
		{
			get
			{
				return _textValue != null;
			}
		}

		public string Text
		{
			get
			{
				return _textValue;
			}
		}

		public object TokenItem
		{
			get
			{
				return _objectValue;
			}
		}

		protected SegmentInfo(bool isTextSegment, object value)
		{
			if (isTextSegment)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Text token cannot have a null value");
				}
				_textValue = (value as string);
				if (_textValue == null)
				{
					throw new ArgumentNullException("value", "Text token value must provide a string");
				}
			}
			else
			{
				_objectValue = value;
			}
		}
	}
}
