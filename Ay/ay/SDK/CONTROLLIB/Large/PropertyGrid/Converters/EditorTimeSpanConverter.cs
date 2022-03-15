using System;
using System.Globalization;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
	/// <summary>Converts a TimeSpan value to a DateTime value.</summary>
	public sealed class EditorTimeSpanConverter : IValueConverter
	{
		public bool AllowNulls
		{
			get;
			set;
		}

		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (AllowNulls && value == null)
			{
				return null;
			}
			TimeSpan t = (value != null) ? ((TimeSpan)value) : TimeSpan.Zero;
			return DateTime.Today + t;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (AllowNulls && value == null)
			{
				return null;
			}
			return (value != null) ? ((DateTime)value).TimeOfDay : TimeSpan.Zero;
		}
	}
}
