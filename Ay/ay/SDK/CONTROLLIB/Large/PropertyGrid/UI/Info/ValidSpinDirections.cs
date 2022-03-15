using System;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Value representing the allowed directions of a spinner.</summary>
	[Flags]
	public enum ValidSpinDirections
	{
		/// <summary>No operation.</summary>
		None = 0x0,
		/// <summary>The increase operation.</summary>
		Increase = 0x1,
		/// <summary>The decrease operation.</summary>
		Decrease = 0x2
	}
}
