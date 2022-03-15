using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
	public class FileInfoToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			FileInfo fileInfo = value as FileInfo;
			if (fileInfo == null)
			{
				throw new InvalidDataException("FileInfoToStringConverter needs to receive a FileInfo.");
			}
			return fileInfo.Name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			string text = value as string;
			if (text == null)
			{
				throw new InvalidDataException("FileInfoToStringConverter needs to receive a string.");
			}
			return new FileInfo(text);
		}
	}
}
