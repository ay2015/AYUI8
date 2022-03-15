using System;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal struct FilterInfo
	{
		public string InputString;

		public Predicate<object> Predicate;
	}
}
