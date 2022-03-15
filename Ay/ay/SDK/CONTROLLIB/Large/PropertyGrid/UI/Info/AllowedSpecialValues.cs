using System;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Values representing the special values the user is allowed to input.</summary>
	[Flags]
	public enum AllowedSpecialValues
	{
		/// <summary>No special values allowed.</summary>
		None = 0x0,
		/// <summary>NaN values allowed.</summary>
		NaN = 0x1,
		/// <summary>Positive infinity allowed</summary>
		PositiveInfinity = 0x2,
		/// <summary>Negative infinity allowed</summary>
		NegativeInfinity = 0x4,
		/// <summary>Either NegativeInfinity or PositiveInfinity.</summary>
		AnyInfinity = 0x6,
		/// <summary>Any special value allowed.</summary>
		Any = 0x7
	}
}
