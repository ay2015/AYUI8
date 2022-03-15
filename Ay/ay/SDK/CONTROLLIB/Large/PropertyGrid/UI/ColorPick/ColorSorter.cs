using System;
using System.Collections;
using System.Drawing;

namespace Xceed.Wpf.Toolkit
{
	internal class ColorSorter : IComparer
	{
		public int Compare(object firstItem, object secondItem)
		{
			if (firstItem == null || secondItem == null)
			{
				return -1;
			}
			ColorItem colorItem = (ColorItem)firstItem;
			ColorItem colorItem2 = (ColorItem)secondItem;
			if (!colorItem.Color.HasValue || !colorItem.Color.HasValue || !colorItem2.Color.HasValue || !colorItem2.Color.HasValue)
			{
				return -1;
			}
			Color color = Color.FromArgb(colorItem.Color.Value.A, colorItem.Color.Value.R, colorItem.Color.Value.G, colorItem.Color.Value.B);
			Color color2 = Color.FromArgb(colorItem2.Color.Value.A, colorItem2.Color.Value.R, colorItem2.Color.Value.G, colorItem2.Color.Value.B);
			double num = Math.Round((double)color.GetHue(), 3);
			double num2 = Math.Round((double)color2.GetHue(), 3);
			if (num > num2)
			{
				return 1;
			}
			if (num < num2)
			{
				return -1;
			}
			double num3 = Math.Round((double)color.GetSaturation(), 3);
			double num4 = Math.Round((double)color2.GetSaturation(), 3);
			if (num3 > num4)
			{
				return 1;
			}
			if (num3 < num4)
			{
				return -1;
			}
			double num5 = Math.Round((double)color.GetBrightness(), 3);
			double num6 = Math.Round((double)color2.GetBrightness(), 3);
			if (num5 > num6)
			{
				return 1;
			}
			if (num5 < num6)
			{
				return -1;
			}
			return 0;
		}
	}
}
