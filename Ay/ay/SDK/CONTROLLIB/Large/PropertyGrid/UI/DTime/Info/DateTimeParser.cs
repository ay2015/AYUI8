using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Xceed.Wpf.Toolkit
{
	internal class DateTimeParser
	{
		public static bool TryParse(string value, string format, DateTime currentDate, CultureInfo cultureInfo, bool autoClipTimeParts, out DateTime result)
		{
			bool flag = false;
			result = currentDate;
			if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(format))
			{
				return false;
			}
			UpdateValueFormatForQuotes(ref value, ref format);
			string text = ComputeDateTimeString(value, format, currentDate, cultureInfo, autoClipTimeParts).Trim();
			if (!string.IsNullOrEmpty(text))
			{
				flag = DateTime.TryParse(text, cultureInfo.DateTimeFormat, DateTimeStyles.None, out result);
			}
			if (!flag)
			{
				result = currentDate;
			}
			return flag;
		}

		private static void UpdateValueFormatForQuotes(ref string value, ref string format)
		{
			int num = format.IndexOf("'");
			if (num > -1)
			{
				int num2 = format.IndexOf("'", num + 1);
				if (num2 > -1)
				{
					string oldValue = format.Substring(num + 1, num2 - num - 1);
					value = value.Replace(oldValue, "");
					format = format.Remove(num, num2 - num + 1);
					UpdateValueFormatForQuotes(ref value, ref format);
				}
			}
		}

		private static string ComputeDateTimeString(string dateTime, string format, DateTime currentDate, CultureInfo cultureInfo, bool autoClipTimeParts)
		{
			Dictionary<string, string> dateParts = GetDateParts(currentDate, cultureInfo);
			string[] array = new string[3]
			{
				currentDate.Hour.ToString(),
				currentDate.Minute.ToString(),
				currentDate.Second.ToString()
			};
			string str = currentDate.Millisecond.ToString();
			string arg = "";
			string[] array2 = new string[7]
			{
				",",
				" ",
				"-",
				".",
				"/",
				cultureInfo.DateTimeFormat.DateSeparator,
				cultureInfo.DateTimeFormat.TimeSeparator
			};
			bool flag = false;
			UpdateSortableDateTimeString(ref dateTime, ref format, cultureInfo);
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if (array2.Any((string s) => dateTime.Contains(s)))
			{
				list = dateTime.Split(array2, StringSplitOptions.RemoveEmptyEntries).ToList();
				list2 = format.Split(array2, StringSplitOptions.RemoveEmptyEntries).ToList();
			}
			else
			{
				string text = "";
				string text2 = "";
				char[] array3 = format.ToCharArray();
				for (int i = 0; i < array3.Count(); i++)
				{
					char c = array3[i];
					if (!text.Contains(c))
					{
						if (!string.IsNullOrEmpty(text))
						{
							list2.Add(text);
							list.Add(text2);
						}
						text = c.ToString();
						text2 = ((i < dateTime.Length) ? dateTime[i].ToString() : "");
					}
					else
					{
						text += c;
						text2 += ((i < dateTime.Length) ? dateTime[i] : '\0');
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					list2.Add(text);
					list.Add(text2);
				}
			}
			if (list.Count < list2.Count)
			{
				while (list.Count != list2.Count)
				{
					list.Add("0");
				}
			}
			if (list.Count != list2.Count)
			{
				return string.Empty;
			}
			for (int j = 0; j < list2.Count; j++)
			{
				string text3 = list2[j];
				if (!text3.Contains("ddd") && !text3.Contains("GMT"))
				{
					if (text3.Contains("M"))
					{
						dateParts["Month"] = list[j];
					}
					else if (text3.Contains("d"))
					{
						dateParts["Day"] = list[j];
					}
					else if (text3.Contains("y"))
					{
						dateParts["Year"] = ((list[j] != "0") ? list[j] : "0000");
						if (dateParts["Year"].Length == 2)
						{
							int num = int.Parse(dateParts["Year"]);
							int twoDigitYearMax = cultureInfo.Calendar.TwoDigitYearMax;
							int num2 = (num <= twoDigitYearMax % 100) ? (twoDigitYearMax / 100) : (twoDigitYearMax / 100 - 1);
							dateParts["Year"] = string.Format("{0}{1}", num2, dateParts["Year"]);
						}
					}
					else if (text3.Contains("hh") || text3.Contains("HH"))
					{
						int num3 = Convert.ToInt32(list[j]) % 24;
						array[0] = (autoClipTimeParts ? num3.ToString() : list[j]);
					}
					else if (text3.Contains("h") || text3.Contains("H"))
					{
						if (autoClipTimeParts)
						{
							int num4 = Convert.ToInt32(list[j]) % 24;
							if (num4 > 11)
							{
								num4 -= 12;
								flag = true;
							}
							array[0] = num4.ToString();
						}
						else
						{
							array[0] = list[j];
						}
					}
					else if (text3.Contains("m"))
					{
						int num5 = Convert.ToInt32(list[j]) % 60;
						array[1] = (autoClipTimeParts ? num5.ToString() : list[j]);
					}
					else if (text3.Contains("s"))
					{
						int num6 = Convert.ToInt32(list[j]) % 60;
						array[2] = (autoClipTimeParts ? num6.ToString() : list[j]);
					}
					else if (text3.Contains("f"))
					{
						str = list[j];
					}
					else if (text3.Contains("t"))
					{
						arg = (flag ? "PM" : list[j]);
					}
				}
			}
			string arg2 = string.Join(cultureInfo.DateTimeFormat.DateSeparator, (from x in dateParts
			select x.Value).ToArray());
			string str2 = string.Join(cultureInfo.DateTimeFormat.TimeSeparator, array);
			str2 = str2 + "." + str;
			return string.Format("{0} {1} {2}", arg2, str2, arg);
		}

		private static void UpdateSortableDateTimeString(ref string dateTime, ref string format, CultureInfo cultureInfo)
		{
			if (format == cultureInfo.DateTimeFormat.SortableDateTimePattern)
			{
				format = format.Replace("'", "").Replace("T", " ");
				dateTime = dateTime.Replace("'", "").Replace("T", " ");
			}
			else if (format == cultureInfo.DateTimeFormat.UniversalSortableDateTimePattern)
			{
				format = format.Replace("'", "").Replace("Z", "");
				dateTime = dateTime.Replace("'", "").Replace("Z", "");
			}
		}

		private static Dictionary<string, string> GetDateParts(DateTime currentDate, CultureInfo cultureInfo)
		{
			Dictionary<string, string> dateParts = new Dictionary<string, string>();
			string[] separator = new string[7]
			{
				",",
				" ",
				"-",
				".",
				"/",
				cultureInfo.DateTimeFormat.DateSeparator,
				cultureInfo.DateTimeFormat.TimeSeparator
			};
			List<string> list = cultureInfo.DateTimeFormat.ShortDatePattern.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
			list.ForEach(delegate(string item)
			{
				string key = string.Empty;
				string value = string.Empty;
				if (item.Contains("M"))
				{
					key = "Month";
					value = currentDate.Month.ToString();
				}
				else if (item.Contains("d"))
				{
					key = "Day";
					value = currentDate.Day.ToString();
				}
				else if (item.Contains("y"))
				{
					key = "Year";
					value = currentDate.Year.ToString("D4");
				}
				if (!dateParts.ContainsKey(key))
				{
					dateParts.Add(key, value);
				}
			});
			return dateParts;
		}
	}
}
