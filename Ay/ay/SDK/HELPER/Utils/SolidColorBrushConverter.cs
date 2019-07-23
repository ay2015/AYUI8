using System;
using System.Windows.Media;

/// <summary>
/// 用户画刷辅助类
/// </summary>
public class SolidColorBrushConverter
{
    static BrushConverter converter = new BrushConverter();
    public static Brush FreezeSolidBrush(string hex)
    {
        var _1 = SolidColorBrushConverter.From16JinZhi(hex);
        _1.Freeze();
        return _1;
    }

    public static System.Windows.Media.Brush From16JinZhi(string color)
    {
        try
        {

            return (System.Windows.Media.Brush)converter.ConvertFromString(color);
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    public static System.Windows.Media.Color ToColor(string colorName)
    {
        if (colorName.StartsWith("#"))
            colorName = colorName.Replace("#", string.Empty);
        int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
        return new System.Windows.Media.Color()
        {
            A = Convert.ToByte((v >> 24) & 255),
            R = Convert.ToByte((v >> 16) & 255),
            G = Convert.ToByte((v >> 8) & 255),
            B = Convert.ToByte((v >> 0) & 255)
        };
    }
}

