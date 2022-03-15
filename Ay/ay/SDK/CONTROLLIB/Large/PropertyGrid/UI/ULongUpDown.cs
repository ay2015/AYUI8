using System;

namespace Xceed.Wpf.Toolkit
{
	
	public class ULongUpDown : CommonNumericUpDown<ulong>
	{
		static ULongUpDown()
		{
			CommonNumericUpDown<ulong>.UpdateMetadata(typeof(ULongUpDown), 1uL, 0uL, ulong.MaxValue);
		}

		public ULongUpDown()
			: base((FromText)ulong.TryParse, (FromDecimal)decimal.ToUInt64, (Func<ulong, ulong, bool>)((ulong v1, ulong v2) => v1 < v2), (Func<ulong, ulong, bool>)((ulong v1, ulong v2) => v1 > v2))
		{
		}

		protected override ulong IncrementValue(ulong value, ulong increment)
		{
			return value + increment;
		}

		protected override ulong DecrementValue(ulong value, ulong increment)
		{
			return value - increment;
		}
	}
}
