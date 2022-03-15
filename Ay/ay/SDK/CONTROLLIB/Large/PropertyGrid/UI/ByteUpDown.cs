using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Represents a textbox with button spinners that allow incrementing and decrementing byte values by using the spinner buttons, keyboard up/down arrows, or mouse
	/// wheel.</summary>
	public class ByteUpDown : CommonNumericUpDown<byte>
	{
		static ByteUpDown()
		{
			CommonNumericUpDown<byte>.UpdateMetadata(typeof(ByteUpDown), 1, 0, byte.MaxValue);
			NumericUpDown<byte?>.MaxLengthProperty.OverrideMetadata(typeof(ByteUpDown), new FrameworkPropertyMetadata(3));
		}

		/// <summary>Initializes a new instance of the ByteUpDown class.</summary>
		public ByteUpDown()
			: base((FromText)byte.TryParse, (FromDecimal)decimal.ToByte, (Func<byte, byte, bool>)((byte v1, byte v2) => v1 < v2), (Func<byte, byte, bool>)((byte v1, byte v2) => v1 > v2))
		{
		}

		protected override byte IncrementValue(byte value, byte increment)
		{
			return (byte)(value + increment);
		}

		protected override byte DecrementValue(byte value, byte increment)
		{
			return (byte)(value - increment);
		}
	}
}
