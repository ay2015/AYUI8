using System;
using System.Collections.Generic;

namespace ay.Wpf.Theme.Element
{
	internal class ExplicitStyles : ElementStylesBase
	{
		internal static readonly List<Type> MergedDataList = new List<Type>
		{
			typeof(AllControlsResourceDictionary)
		};

		public ExplicitStyles()
			: base(MergedDataList)
		{
		}

		public ExplicitStyles(ColorModeEnum colorMode)
			: base(MergedDataList)
		{
		}
	}
}
