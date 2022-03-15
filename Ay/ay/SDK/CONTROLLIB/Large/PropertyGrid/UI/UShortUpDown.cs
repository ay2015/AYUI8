using System;

namespace Xceed.Wpf.Toolkit
{
	
	public class UShortUpDown : CommonNumericUpDown<ushort>
	{
		static UShortUpDown()
		{
			CommonNumericUpDown<ushort>.UpdateMetadata(typeof(UShortUpDown), 1, 0, ushort.MaxValue);
		}

		public UShortUpDown()
			: base((FromText)ushort.TryParse, (FromDecimal)decimal.ToUInt16, (Func<ushort, ushort, bool>)((ushort v1, ushort v2) => v1 < v2), (Func<ushort, ushort, bool>)((ushort v1, ushort v2) => v1 > v2))
		{
		}

		protected override ushort IncrementValue(ushort value, ushort increment)
		{
			return (ushort)(value + increment);
		}

		protected override ushort DecrementValue(ushort value, ushort increment)
		{
			return (ushort)(value - increment);
		}
	}
}
