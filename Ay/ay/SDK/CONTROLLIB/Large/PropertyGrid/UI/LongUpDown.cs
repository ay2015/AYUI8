using System;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Represents a textbox with button spinners that allow incrementing and decrementing long values by using the spinner buttons, keyboard up/down arrows, or mouse
	/// wheel.</summary>
	public class LongUpDown : CommonNumericUpDown<long>
	{
		static LongUpDown()
		{
			CommonNumericUpDown<long>.UpdateMetadata(typeof(LongUpDown), 1L, -9223372036854775808L, 9223372036854775807L);
		}

		/// <summary>Initializes a new instance of the LongUpDown class.</summary>
		public LongUpDown()
			: base((FromText)long.TryParse, (FromDecimal)decimal.ToInt64, (Func<long, long, bool>)((long v1, long v2) => v1 < v2), (Func<long, long, bool>)((long v1, long v2) => v1 > v2))
		{
		}

		protected override long IncrementValue(long value, long increment)
		{
			return value + increment;
		}

		protected override long DecrementValue(long value, long increment)
		{
			return value - increment;
		}
	}
}
