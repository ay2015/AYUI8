using System;

namespace Xceed.Wpf.Toolkit.Core.Utilities
{
	internal static class DateTimeUtilities
	{
		public static DateTime GetContextNow(DateTimeKind kind)
		{
			switch (kind)
			{
			case DateTimeKind.Unspecified:
				return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
			default:
				return DateTime.Now;
			case DateTimeKind.Utc:
				return DateTime.UtcNow;
			}
		}

		public static bool IsSameDate(DateTime? date1, DateTime? date2)
		{
			if (!date1.HasValue || !date2.HasValue)
			{
				return false;
			}
			return date1.Value.Date == date2.Value.Date;
		}
	}
}
