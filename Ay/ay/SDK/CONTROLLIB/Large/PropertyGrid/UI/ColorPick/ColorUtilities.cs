using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit.Core.Utilities
{
	internal static class ColorUtilities
	{
		public static readonly Dictionary<string, Color> KnownColors = GetKnownColors();

		public static string GetColorName(this Color color)
		{
			string text = (from kvp in KnownColors
			where kvp.Value.Equals(color)
			select kvp.Key).FirstOrDefault();
			if (string.IsNullOrEmpty(text))
			{
				text = color.ToString();
			}
			return text;
		}

		public static string FormatColorString(string stringToFormat, bool isUsingAlphaChannel)
		{
			if (!isUsingAlphaChannel && stringToFormat.Length == 9)
			{
				return stringToFormat.Remove(1, 2);
			}
			return stringToFormat;
		}

		private static Dictionary<string, Color> GetKnownColors()
		{
			PropertyInfo[] properties = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
			return properties.ToDictionary((PropertyInfo p) => p.Name, (PropertyInfo p) => (Color)p.GetValue(null, null));
		}

		public static HsvColor ConvertRgbToHsv(int r, int g, int b)
		{
			double num = 0.0;
			double num2 = (double)Math.Min(Math.Min(r, g), b);
			double num3 = (double)Math.Max(Math.Max(r, g), b);
			double num4 = num3 - num2;
			double num5 = (num3 != 0.0) ? (num4 / num3) : 0.0;
			if (num5 == 0.0)
			{
				num = 0.0;
			}
			else
			{
				if ((double)r == num3)
				{
					num = (double)(g - b) / num4;
				}
				else if ((double)g == num3)
				{
					num = 2.0 + (double)(b - r) / num4;
				}
				else if ((double)b == num3)
				{
					num = 4.0 + (double)(r - g) / num4;
				}
				num *= 60.0;
				if (num < 0.0)
				{
					num += 360.0;
				}
			}
			HsvColor result = default(HsvColor);
			result.H = num;
			result.S = num5;
			result.V = num3 / 255.0;
			return result;
		}

		public static Color ConvertHsvToRgb(double h, double s, double v)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			if (s == 0.0)
			{
				num = v;
				num2 = v;
				num3 = v;
			}
			else
			{
				h = ((h != 360.0) ? (h / 60.0) : 0.0);
				int num4 = (int)Math.Truncate(h);
				double num5 = h - (double)num4;
				double num6 = v * (1.0 - s);
				double num7 = v * (1.0 - s * num5);
				double num8 = v * (1.0 - s * (1.0 - num5));
				switch (num4)
				{
				case 0:
					num = v;
					num2 = num8;
					num3 = num6;
					break;
				case 1:
					num = num7;
					num2 = v;
					num3 = num6;
					break;
				case 2:
					num = num6;
					num2 = v;
					num3 = num8;
					break;
				case 3:
					num = num6;
					num2 = num7;
					num3 = v;
					break;
				case 4:
					num = num8;
					num2 = num6;
					num3 = v;
					break;
				default:
					num = v;
					num2 = num6;
					num3 = num7;
					break;
				}
			}
			return Color.FromArgb(byte.MaxValue, (byte)Math.Round(num * 255.0), (byte)Math.Round(num2 * 255.0), (byte)Math.Round(num3 * 255.0));
		}

		public static List<Color> GenerateHsvSpectrum()
		{
			List<Color> list = new List<Color>();
			int num = 60;
			for (int i = 0; i < 360; i += num)
			{
				list.Add(ConvertHsvToRgb((double)i, 1.0, 1.0));
			}
			list.Add(ConvertHsvToRgb(0.0, 1.0, 1.0));
			return list;
		}
	}
}
