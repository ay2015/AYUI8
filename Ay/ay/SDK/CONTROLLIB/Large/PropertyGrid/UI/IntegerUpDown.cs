using System;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Represents a textbox with button spinners that allow incrementing and decrementing integer values by using the spinner buttons, keyboard up/down arrows, or
	/// mouse wheel.</summary>
	public class IntegerUpDown : CommonNumericUpDown<int>
	{
		static IntegerUpDown()
		{
			CommonNumericUpDown<int>.UpdateMetadata(typeof(IntegerUpDown), 1, -2147483648, 2147483647);
		}

		/// <summary>Initializes a new instance of the IntegerUpDown class.</summary>
		public IntegerUpDown()
			: base((FromText)int.TryParse, (FromDecimal)decimal.ToInt32, (Func<int, int, bool>)((int v1, int v2) => v1 < v2), (Func<int, int, bool>)((int v1, int v2) => v1 > v2))
		{
		}

		protected override int IncrementValue(int value, int increment)
		{
			return value + increment;
		}

		protected override int DecrementValue(int value, int increment)
		{
			return value - increment;
		}
	}
}
