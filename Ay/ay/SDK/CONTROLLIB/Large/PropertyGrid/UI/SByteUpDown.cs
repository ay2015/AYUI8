using System;

namespace Xceed.Wpf.Toolkit
{
	
	public class SByteUpDown : CommonNumericUpDown<sbyte>
	{
		static SByteUpDown()
		{
			CommonNumericUpDown<sbyte>.UpdateMetadata(typeof(SByteUpDown), 1, sbyte.MinValue, sbyte.MaxValue);
		}

		public SByteUpDown()
			: base((FromText)sbyte.TryParse, (FromDecimal)decimal.ToSByte, (Func<sbyte, sbyte, bool>)((sbyte v1, sbyte v2) => v1 < v2), (Func<sbyte, sbyte, bool>)((sbyte v1, sbyte v2) => v1 > v2))
		{
		}

		protected override sbyte IncrementValue(sbyte value, sbyte increment)
		{
			return (sbyte)(value + increment);
		}

		protected override sbyte DecrementValue(sbyte value, sbyte increment)
		{
			return (sbyte)(value - increment);
		}
	}
}
