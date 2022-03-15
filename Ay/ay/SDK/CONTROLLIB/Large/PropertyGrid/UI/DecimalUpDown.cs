using System;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Represents a textbox with button spinners that allow incrementing and decrementing decimal values by using the spinner buttons, keyboard up/down arrows, or
	/// mouse wheel.</summary>
	public class DecimalUpDown : CommonNumericUpDown<decimal>
	{
		static DecimalUpDown()
		{
			CommonNumericUpDown<decimal>.UpdateMetadata(typeof(DecimalUpDown), 1m, decimal.MinValue, decimal.MaxValue);
		}

		/// <summary>Initializes a new instance of the DecimalUpDown class.</summary>
		public DecimalUpDown()
			: base((FromText)decimal.TryParse, (FromDecimal)((decimal d) => d), (Func<decimal, decimal, bool>)((decimal v1, decimal v2) => v1 < v2), (Func<decimal, decimal, bool>)((decimal v1, decimal v2) => v1 > v2))
		{
		}

		protected override decimal IncrementValue(decimal value, decimal increment)
		{
			return value + increment;
		}

		protected override decimal DecrementValue(decimal value, decimal increment)
		{
			return value - increment;
		}
	}
}
