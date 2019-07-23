
using System.Windows;

namespace ay.contentcore
{
    public static class FontConvertExt
    {
        public static FontWeight ToFontWeight(this string fontFamilyWeight)
        {
            FontWeight Weight = FontWeights.Normal;
            switch (fontFamilyWeight)
            {
                case "Normal":
                    Weight = FontWeights.Normal;
                    break;
                case "Light":
                    Weight = FontWeights.Light;
                    break;
                case "Medium":
                    Weight = FontWeights.Medium;
                    break;
                case "Regular":
                    Weight = FontWeights.Regular;
                    break;
                case "Heavy":
                    Weight = FontWeights.Heavy;
                    break;
                case "Black":
                    Weight = FontWeights.Black;
                    break;
                case "Bold":
                    Weight = FontWeights.Bold;
                    break;
                case "DemiBold":
                    Weight = FontWeights.DemiBold;
                    break;
                case "ExtraBlack":
                    Weight = FontWeights.ExtraBlack;
                    break;
                case "ExtraBold":
                    Weight = FontWeights.ExtraBold;
                    break;
                case "ExtraLight":
                    Weight = FontWeights.ExtraLight;
                    break;
                case "SemiBold":
                    Weight = FontWeights.SemiBold;
                    break;
                case "Thin":
                    Weight = FontWeights.Thin;
                    break;
                case "UltraBlack":
                    Weight = FontWeights.UltraBlack;
                    break;
                case "UltraBold":
                    Weight = FontWeights.UltraBold;
                    break;
                case "UltraLight":
                    Weight = FontWeights.UltraLight;
                    break;
                default:
                    Weight = FontWeights.Normal;
                    break;
            }
            return Weight;
        }
        public static FontStretch ToFontStretch(this string fontFamilyStretch)
        {
            FontStretch Stretch = FontStretches.Normal;
            switch (fontFamilyStretch)
            {
                case "Normal":
                    Stretch = FontStretches.Normal;
                    break;
                case "Condensed":
                    Stretch = FontStretches.Condensed;
                    break;
                case "ExtraCondensed":
                    Stretch = FontStretches.ExtraCondensed;
                    break;
                case "UltraCondensed":
                    Stretch = FontStretches.UltraCondensed;
                    break;
                case "SemiCondensed":
                    Stretch = FontStretches.SemiCondensed;
                    break;
                case "Oblique":
                    Stretch = FontStretches.Condensed;
                    break;
                case "Medium":
                    Stretch = FontStretches.Medium;
                    break;
                case "SemiExpanded":
                    Stretch = FontStretches.SemiExpanded;
                    break;
                case "Expanded":
                    Stretch = FontStretches.Expanded;
                    break;
                case "ExtraExpanded":
                    Stretch = FontStretches.ExtraExpanded;
                    break;
                case "UltraExpanded":
                    Stretch = FontStretches.UltraExpanded;
                    break;
                default:
                    Stretch = FontStretches.Normal;
                    break;
            }
            return Stretch;
        }
        public static FontStyle ToFontStyle(this string fontFamilyStyle)
        {
            FontStyle Style = FontStyles.Normal;
            switch (fontFamilyStyle)
            {
                case "Italic":
                    Style = FontStyles.Italic;
                    break;
                case "Normal":
                    Style = FontStyles.Normal;
                    break;
                case "Oblique":
                    Style = FontStyles.Oblique;
                    break;
                default:
                    Style = FontStyles.Normal;
                    break;
            }
            return Style;
        }

    }
}


