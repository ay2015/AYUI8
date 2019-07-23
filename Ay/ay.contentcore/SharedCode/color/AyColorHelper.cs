using ay.contentcore;
using System;
using System.Collections.Generic;
using System.Windows.Media;


public static class AyColorHelper
{
    public static Color ConvertHslToRgb(HlsColor hlsColor)
    {
        // Initialize result
        var rgbColor = new Color();

        /* If S = 0, that means we are dealing with a shade 
         * of gray. So, we set R, G, and B to L and exit. */

        // Special case: Gray
        if (hlsColor.S == 0)
        {
            rgbColor.R = (byte)(hlsColor.L * 255);
            rgbColor.G = (byte)(hlsColor.L * 255);
            rgbColor.B = (byte)(hlsColor.L * 255);
            rgbColor.A = (byte)(hlsColor.A * 255);
            return rgbColor;
        }

        double t1;
        if (hlsColor.L < 0.5)
        {
            t1 = hlsColor.L * (1.0 + hlsColor.S);
        }
        else
        {
            t1 = hlsColor.L + hlsColor.S - (hlsColor.L * hlsColor.S);
        }

        var t2 = 2.0 * hlsColor.L - t1;

        // Convert H from degrees to a percentage
        var h = hlsColor.H / 360;

        // Set colors as percentage values
        var tR = h + (1.0 / 3.0);
        var r = SetColor(t1, t2, tR);

        var tG = h;
        var g = SetColor(t1, t2, tG);

        var tB = h - (1.0 / 3.0);
        var b = SetColor(t1, t2, tB);

        // Assign colors to Color object
        rgbColor.R = (byte)(r * 255);
        rgbColor.G = (byte)(g * 255);
        rgbColor.B = (byte)(b * 255);
        rgbColor.A = (byte)(hlsColor.A * 255);

        // Set return value
        return rgbColor;
    }
    private static double SetColor(double t1, double t2, double t3)
    {
        if (t3 < 0) t3 += 1.0;
        if (t3 > 1) t3 -= 1.0;

        double color;
        if (6.0 * t3 < 1)
        {
            color = t2 + (t1 - t2) * 6.0 * t3;
        }
        else if (2.0 * t3 < 1)
        {
            color = t1;
        }
        else if (3.0 * t3 < 2)
        {
            color = t2 + (t1 - t2) * ((2.0 / 3.0) - t3) * 6.0;
        }
        else
        {
            color = t2;
        }

        // Set return value
        return color;
    }

    public static HlsColor ConvertRgbToHsl(Color rgbColor)
    {
        // Initialize result
        var hlsColor = new HlsColor();

        // Convert RGB values to percentages
        double r = (double)rgbColor.R / 255;
        var g = (double)rgbColor.G / 255;
        var b = (double)rgbColor.B / 255;
        var a = (double)rgbColor.A / 255;

        // Find min and max RGB values
        var min = Math.Min(r, Math.Min(g, b));
        var max = Math.Max(r, Math.Max(g, b));
        var delta = max - min;

        /* If max and min are equal, that means we are dealing with 
         * a shade of gray. So we set H and S to zero, and L to either
         * max or min (it doesn't matter which), and  then we exit. */

        //Special case: Gray
        if (max == min)
        {
            hlsColor.H = 0;
            hlsColor.S = 0;
            hlsColor.L = max;
            return hlsColor;
        }

        /* If we get to this point, we know we don't have a shade of gray. */

        // Set L
        hlsColor.L = (min + max) / 2;

        // Set S
        if (hlsColor.L < 0.5)
        {
            hlsColor.S = delta / (max + min);
        }
        else
        {
            hlsColor.S = delta / (2.0 - max - min);
        }

        // Set H
        if (r == max) hlsColor.H = (g - b) / delta;
        if (g == max) hlsColor.H = 2.0 + (b - r) / delta;
        if (b == max) hlsColor.H = 4.0 + (r - g) / delta;
        hlsColor.H *= 60;
        if (hlsColor.H < 0) hlsColor.H += 360;

        // Set A
        hlsColor.A = a;

        // Set return value
        return hlsColor;
    }
    public static Color ConvertHsvToRgb(double h, double v, double s)
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
            if (h == 360.0)
            {
                h = 0.0;
            }
            else
            {
                h /= 60.0;
            }
            int num4 = (int)Math.Truncate(h);
            double num5 = h - num4;
            double num6 = v * (1.0 - s);
            double num7 = v * (1.0 - (s * num5));
            double num8 = v * (1.0 - (s * (1.0 - num5)));
            switch (num4)
            {
                case 0:
                    num = v;
                    num2 = num8;
                    num3 = num6;
                    goto Label_0119;

                case 1:
                    num = num7;
                    num2 = v;
                    num3 = num6;
                    goto Label_0119;

                case 2:
                    num = num6;
                    num2 = v;
                    num3 = num8;
                    goto Label_0119;

                case 3:
                    num = num6;
                    num2 = num7;
                    num3 = v;
                    goto Label_0119;

                case 4:
                    num = num8;
                    num2 = num6;
                    num3 = v;
                    goto Label_0119;
            }
            num = v;
            num2 = num6;
            num3 = num7;
        }
    Label_0119:
        return Color.FromArgb(0xff, (byte)(num * 255.0), (byte)(num2 * 255.0), (byte)(num3 * 255.0));
    }

    public static HsvColor ConvertRgbToHsv(int r, int b, int g)
    {
        double num4;
        double num3 = 0.0;
        double num2 = Math.Min(Math.Min(r, g), b);
        double num5 = Math.Max(Math.Max(r, g), b);
        double num = num5 - num2;
        if (num5 == 0.0)
        {
            num4 = 0.0;
        }
        else
        {
            num4 = num / num5;
        }
        if (num4 == 0.0)
        {
            num3 = 0.0;
        }
        else
        {
            if (r == num5)
            {
                num3 = ((double)(g - b)) / num;
            }
            else if (g == num5)
            {
                num3 = 2.0 + (((double)(b - r)) / num);
            }
            else if (b == num5)
            {
                num3 = 4.0 + (((double)(r - g)) / num);
            }
            num3 *= 60.0;
            if (num3 < 0.0)
            {
                num3 += 360.0;
            }
        }
        return new HsvColor { H = num3, S = num4, V = num5 / 255.0 };
    }

    public static List<Color> GenerateHsvSpectrum()
    {
        List<Color> list = new List<Color>(8);
        for (int i = 0; i < 0x1d; i++)
        {
            list.Add(ConvertHsvToRgb((double)(i * 12), 1.0, 1.0));
        }
        list.Add(ConvertHsvToRgb(0.0, 1.0, 1.0));
        return list;
    }
}


