using System;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>
	///   <para>Represents a textbox with button spinners that allow incrementing and decrementing short values by using the spinner buttons, keyboard up/down arrows, or
	/// mouse wheel.</para>
	/// </summary>
	public class ShortUpDown : CommonNumericUpDown<short>
	{
		static ShortUpDown()
		{
			CommonNumericUpDown<short>.UpdateMetadata(typeof(ShortUpDown), 1, short.MinValue, short.MaxValue);
		}

		/// <summary>Initializes a new instance of the ShortUpDown class.</summary>
		public ShortUpDown()
			: base((FromText)short.TryParse, (FromDecimal)decimal.ToInt16, (Func<short, short, bool>)((short v1, short v2) => v1 < v2), (Func<short, short, bool>)((short v1, short v2) => v1 > v2))
		{
		}

		protected override short IncrementValue(short value, short increment)
		{
			return (short)(value + increment);
		}

		protected override short DecrementValue(short value, short increment)
		{
			return (short)(value - increment);
		}
	}
}
