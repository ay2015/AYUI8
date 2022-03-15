using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
	internal class ListConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return true;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return null;
			}
			string text = value as string;
			List<object> list = new List<object>();
			if (text == null && value != null)
			{
				list.Add(value);
			}
			else
			{
				if (text == null)
				{
					return null;
				}
				string[] array = text.Split(',');
				foreach (string text2 in array)
				{
					list.Add(text2.Trim());
				}
			}
			return new ReadOnlyCollection<object>(list);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
			{
				throw new InvalidOperationException("Can only convert to string.");
			}
			IList list = (IList)value;
			if (list == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (object item in list)
			{
				if (item == null)
				{
					throw new InvalidOperationException("Property names cannot be null.");
				}
				string text = item as string;
				if (text == null)
				{
					throw new InvalidOperationException("Does not support serialization of non-string property names.");
				}
				if (text.Contains(','))
				{
					throw new InvalidOperationException("Property names cannot contain commas.");
				}
				if (text.Trim().Length != text.Length)
				{
					throw new InvalidOperationException("Property names cannot start or end with whitespace characters.");
				}
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}
	}
}
