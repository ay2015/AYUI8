using System;

namespace Xceed.Wpf.Toolkit
{
	
	public class UIntegerUpDown : CommonNumericUpDown<uint>
	{
		static UIntegerUpDown()
		{
			CommonNumericUpDown<uint>.UpdateMetadata(typeof(UIntegerUpDown), 1u, 0u, uint.MaxValue);
		}

		public UIntegerUpDown()
			: base((FromText)uint.TryParse, (FromDecimal)decimal.ToUInt32, (Func<uint, uint, bool>)((uint v1, uint v2) => v1 < v2), (Func<uint, uint, bool>)((uint v1, uint v2) => v1 > v2))
		{
		}

		protected override uint IncrementValue(uint value, uint increment)
		{
			return value + increment;
		}

		protected override uint DecrementValue(uint value, uint increment)
		{
			return value - increment;
		}
	}
}
