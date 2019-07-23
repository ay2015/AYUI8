using ay.contentcore;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace ay.Wpf.Theme.Element
{
    public abstract class ElementThemeResourceDictionaryBase : ThemeResourceDictionary
    {
        private static Color GoldAccent = Color.FromArgb(byte.MaxValue, 244, 179, 0);

        private static Color LightGreenAccent = Color.FromArgb(byte.MaxValue, 120, 186, 0);

        private static Color BlueAccent = Color.FromArgb(byte.MaxValue, 38, 115, 236);

        private static Color MediumVioletRedAccent = Color.FromArgb(byte.MaxValue, 174, 17, 61);

        private static Color BrownAccent = Color.FromArgb(byte.MaxValue, 99, 47, 0);

        private static Color DarkRedAccent = Color.FromArgb(byte.MaxValue, 176, 30, 0);

        private static Color PaleVioletRedAccent = Color.FromArgb(byte.MaxValue, 193, 0, 79);

        private static Color DarkVioletAccent = Color.FromArgb(byte.MaxValue, 114, 0, 172);

        private static Color DarkSlateBlueAccent = Color.FromArgb(byte.MaxValue, 70, 23, 180);

        private static Color RoyalBlueAccent = Color.FromArgb(byte.MaxValue, 0, 106, 193);

        private static Color TealAccent = Color.FromArgb(byte.MaxValue, 0, 130, 135);

        private static Color GreenAccent = Color.FromArgb(byte.MaxValue, 25, 153, 0);

        private static Color LimeAccent = Color.FromArgb(byte.MaxValue, 0, 193, 63);

        private static Color LightSalmonAccent = Color.FromArgb(byte.MaxValue, byte.MaxValue, 152, 29);

        private static Color RedAccent = Color.FromArgb(byte.MaxValue, byte.MaxValue, 46, 18);

        private static Color DeepPinkAccent = Color.FromArgb(byte.MaxValue, byte.MaxValue, 29, 119);

        private static Color PurpleAccent = Color.FromArgb(byte.MaxValue, 170, 64, byte.MaxValue);

        private static Color DeepSkyBlueAccent = Color.FromArgb(byte.MaxValue, 31, 174, byte.MaxValue);

        private static Color DarkTurquoiseAccent = Color.FromArgb(byte.MaxValue, 86, 197, byte.MaxValue);

        private static Color TurquoiseAccent = Color.FromArgb(byte.MaxValue, 0, 216, 204);

        private static Color YellowGreenAccent = Color.FromArgb(byte.MaxValue, 145, 209, 0);

        private static Color DarkYellowAccent = Color.FromArgb(byte.MaxValue, 225, 183, 0);

        private static Color PinkAccent = Color.FromArgb(byte.MaxValue, byte.MaxValue, 118, 188);

        private static Color LightSeaGreenAccent = Color.FromArgb(byte.MaxValue, 0, 164, 164);

        private static Color OrangeAccent = Color.FromArgb(byte.MaxValue, byte.MaxValue, 125, 35);

        private ElementAccentColor _accentColor;

        private Brush _accentBrush;
        public Brush AccentBrush
        {
            get
            {
                return _accentBrush;
            }
            set
            {
                if (_accentBrush != value)
                {
                    _accentBrush = value;
                    UpdateResources();
                }
            }
        }

        public ElementAccentColor AccentColor
        {
            get
            {
                return _accentColor;
            }
            set
            {
                if (_accentColor != value)
                {
                    _accentColor = value;
                    AccentBrush = GetAccentBrushFromAccentColor();
                }
            }
        }

        internal ColorModeEnum ColorMode
        {
            get;
            set;
        }

        protected virtual ElementAccentColor DefaultAccentColor
        {
            get
            {
                return ElementAccentColor.Blue;
            }
        }

        private static SolidColorBrush GetSolidColorBrushFromAccentColor(ElementAccentColor accentColor)
        {
            SolidColorBrush result = null;
            switch (accentColor)
            {
                case ElementAccentColor.Gold:
                    result = new SolidColorBrush(GoldAccent);
                    break;
                case ElementAccentColor.LightGreen:
                    result = new SolidColorBrush(LightGreenAccent);
                    break;
                case ElementAccentColor.Blue:
                    result = new SolidColorBrush(BlueAccent);
                    break;
                case ElementAccentColor.MediumVioletRed:
                    result = new SolidColorBrush(MediumVioletRedAccent);
                    break;
                case ElementAccentColor.Brown:
                    result = new SolidColorBrush(BrownAccent);
                    break;
                case ElementAccentColor.DarkRed:
                    result = new SolidColorBrush(DarkRedAccent);
                    break;
                case ElementAccentColor.PaleVioletRed:
                    result = new SolidColorBrush(PaleVioletRedAccent);
                    break;
                case ElementAccentColor.DarkViolet:
                    result = new SolidColorBrush(DarkVioletAccent);
                    break;
                case ElementAccentColor.DarkSlateBlue:
                    result = new SolidColorBrush(DarkSlateBlueAccent);
                    break;
                case ElementAccentColor.RoyalBlue:
                    result = new SolidColorBrush(RoyalBlueAccent);
                    break;
                case ElementAccentColor.Teal:
                    result = new SolidColorBrush(TealAccent);
                    break;
                case ElementAccentColor.Green:
                    result = new SolidColorBrush(GreenAccent);
                    break;
                case ElementAccentColor.Lime:
                    result = new SolidColorBrush(LimeAccent);
                    break;
                case ElementAccentColor.LightSalmon:
                    result = new SolidColorBrush(LightSalmonAccent);
                    break;
                case ElementAccentColor.Red:
                    result = new SolidColorBrush(RedAccent);
                    break;
                case ElementAccentColor.DeepPink:
                    result = new SolidColorBrush(DeepPinkAccent);
                    break;
                case ElementAccentColor.Purple:
                    result = new SolidColorBrush(PurpleAccent);
                    break;
                case ElementAccentColor.DeepSkyBlue:
                    result = new SolidColorBrush(DeepSkyBlueAccent);
                    break;
                case ElementAccentColor.DarkTurquoise:
                    result = new SolidColorBrush(DarkTurquoiseAccent);
                    break;
                case ElementAccentColor.Turquoise:
                    result = new SolidColorBrush(TurquoiseAccent);
                    break;
                case ElementAccentColor.YellowGreen:
                    result = new SolidColorBrush(YellowGreenAccent);
                    break;
                case ElementAccentColor.Orange:
                    result = new SolidColorBrush(OrangeAccent);
                    break;
                case ElementAccentColor.Pink:
                    result = new SolidColorBrush(PinkAccent);
                    break;
                case ElementAccentColor.LightSeaGreen:
                    result = new SolidColorBrush(LightSeaGreenAccent);
                    break;
                case ElementAccentColor.DarkYellow:
                    result = new SolidColorBrush(DarkYellowAccent);
                    break;
                case ElementAccentColor.None:
                    result = null;
                    break;
            }
            return result;
        }

        protected ElementThemeResourceDictionaryBase()
        {
            UpdateResources();
        }

        protected ElementThemeResourceDictionaryBase(ColorModeEnum colorMode)
        {
            ColorMode = colorMode;
            UpdateResources();
        }

        protected ElementThemeResourceDictionaryBase(Brush accentBrush, ColorModeEnum colorMode)
        {
            _accentBrush = accentBrush;
            ColorMode = colorMode;
            UpdateResources();
        }

        private ElementAccentColor GetDefaultAccentColor()
        {
            return DefaultAccentColor;
        }

        protected override void UpdateResources()
        {
            Clear();
            base.MergedDictionaries.Clear();
            _accentBrush = ((AccentBrush == null) ? GetAccentBrushFromAccentColor() : AccentBrush);
            if (base.StyleUsageMode == StyleUsageMode.Implicit)
            {
                List<Type> initialImplicitTypes = GetInitialImplicitTypes();
                PrepareStyles(initialImplicitTypes, AccentBrush, this);
            }
            else
            {
                List<Type> initialExplicitTypes = GetInitialExplicitTypes();
                PrepareStyles(initialExplicitTypes, AccentBrush, this);
            }
            AddMiscStyles();
        }

        protected virtual List<Type> GetInitialImplicitTypes()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof(ImplicitStyles));
            return list;
        }

        protected virtual List<Type> GetInitialExplicitTypes()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof(ExplicitStyles));
            return list;
        }

        protected virtual void AddMiscStyles()
        {
        }

        private void PrepareStyles(List<Type> typeList, Brush accentBrush, ResourceDictionary parent)
        {
            var templateDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string absolutePath = System.IO.Path.Combine(templateDirectory, "ay.Wpf.Theme.Element.ThemeConfig.Default.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(absolutePath);
            XmlNode node = doc.SelectSingleNode("Root");
            XmlNodeList childs = node.ChildNodes;

            foreach (Type type in typeList)
            {
                ElementStylesBase ElementStylesBase = Activator.CreateInstance(type, ColorMode) as ElementStylesBase;
                if (ElementStylesBase.MergedDatas.Count > 0)
                {
                    PrepareStyles(ElementStylesBase.MergedDatas, accentBrush, ElementStylesBase);
                }


                foreach (XmlNode ass in childs)
                {
                    var keyvaluedesc = ass.Attributes["Des"].Value;
                    if (keyvaluedesc == "自动计算") continue;
                    var keyname = ass.Name;
                    var keyvalue = ass.Attributes["value"].Value;
   
                    var _1 = HexToBrush.FromHex(keyvalue) as SolidColorBrush;
                    ElementStylesBase.EnsureResource(keyname, _1);
                    //添加color
                    ElementStylesBase.EnsureResource(keyname+".color", _1.Color);
                    if (keyname == "colorprimary")
                    {
                        SetColor(ElementStylesBase, _1, "colorprimary");
                    }
                    else if (keyname == "colorsuccess")
                    {
                        SetColor(ElementStylesBase, _1, "colorsuccess");
                    }
                    else if (keyname == "colorwarning")
                    {
                        SetColor(ElementStylesBase, _1, "colorwarning");
                    }
                    else if (keyname == "colordanger")
                    {
                        SetColor(ElementStylesBase, _1, "colordanger");
                    }
                    else if (keyname == "colorinfo")
                    {
                        SetColor(ElementStylesBase, _1, "colorinfo");
                    }
                }

                ElementStylesBase.Initialize();
                if (parent != null)
                {
                    parent.MergedDictionaries.Add(ElementStylesBase);
                }
            }
        }

        private static void SetColor(ElementStylesBase ElementStylesBase, SolidColorBrush _1, string keyname)
        {
            //var _color1 = HexToBrush.ToColor("#409EFF");
            //var _color2 = HexToBrush.ToColor("#D9ECFF");
            //var _color3 = HexToBrush.ToColor("#ECF5FF");
            //var _color4 = HexToBrush.ToColor("#66B1FF");
            //var _color5 = HexToBrush.ToColor("#C6E2FF");

            //HlsColor hsv1 = AyColorHelper.ConvertRgbToHsl(_color1);
            //HlsColor hsv2 = AyColorHelper.ConvertRgbToHsl(_color2);
            //HlsColor hsv3 = AyColorHelper.ConvertRgbToHsl(_color3);
            //HlsColor hsv4 = AyColorHelper.ConvertRgbToHsl(_color4);
            //HlsColor hsv5 = AyColorHelper.ConvertRgbToHsl(_color5);
            //Console.WriteLine((hsv2.H - hsv1.H).ToString() + "\t" + (hsv2.S - hsv1.S).ToString() + "\t" + (hsv2.L - hsv1.L).ToString());
            //Console.WriteLine((hsv3.H - hsv1.H).ToString() + "\t" + (hsv3.S - hsv1.S).ToString() + "\t" + (hsv3.L - hsv1.L).ToString());
            //Console.WriteLine((hsv4.H - hsv1.H).ToString() + "\t" + (hsv4.S - hsv1.S).ToString() + "\t" + (hsv4.L - hsv1.L).ToString());
            //Console.WriteLine((hsv5.H - hsv1.H).ToString() + "\t" + (hsv5.S - hsv1.S).ToString() + "\t" + (hsv5.L - hsv1.L).ToString());

            HlsColor hsl = AyColorHelper.ConvertRgbToHsl(_1.Color);
            var _a2 = hsl.H - 0.471204188481693;
            if (_a2 < 0)
            {
                _a2 = 0;
            }
            var _a3 = hsl.L + 0.3;
            if (_a3 > 1)
            {
                _a3 = 1;
            }
            HlsColor h = new HlsColor();
            h.A = hsl.A;
            h.H = _a2;
            h.L = _a3;
            h.S = hsl.S;
            Color l1Color = AyColorHelper.ConvertHslToRgb(h);

            var _a4 = hsl.H + 1.10774317993938;
            if (_a4 > 360)
            {
                _a4 = 360;
            }

            var _a5 = hsl.L + 0.337254901960784;
            if (_a5 > 1)
            {
                var _122 = _a5 - 1 + 0.03;
                _a5 = _a5 - _122;
            }
            HlsColor h2 = new HlsColor();
            h2.A = hsl.A;
            h2.H = _a4;
            h2.L = _a5;
            h2.S = hsl.S;
            Color l2Color = AyColorHelper.ConvertHslToRgb(h2);
            ElementStylesBase.EnsureResource(keyname + "1", new SolidColorBrush(l1Color));
            ElementStylesBase.EnsureResource(keyname + "2", new SolidColorBrush(l2Color));


            var _a6 = hsl.H + 0.117031105635988;
            if (_a6 > 360)
            {
                _a6 = 360;
            }

            var _a7 = hsl.L + 0.0745098039215686;
            if (_a7 > 1)
            {
                _a7 = 1;
            }
            HlsColor h3 = new HlsColor();
            h3.A = hsl.A;
            h3.H = _a6;
            h3.L = _a7;
            h3.S = hsl.S;
            Color l3Color = AyColorHelper.ConvertHslToRgb(h3);

            ElementStylesBase.EnsureResource(keyname + "3", new SolidColorBrush(l3Color));

            var _a8 = hsl.H + 0.226470230122999;
            if (_a8 > 360)
            {
                _a8 = 360;
            }
            var _a9 = hsl.S - 0.225225225225225;
            if (_a9 < 0)
            {
                _a9 = 0;
            }
            var _a10 = hsl.L - 0.0607843137254902;
            if (_a10 < 0)
            {
                _a10 = 0;
            }
            HlsColor h4 = new HlsColor();
            h4.A = hsl.A;
            h4.H = _a8;
            h4.L = _a10;
            h4.S = _a9;
            Color l4Color = AyColorHelper.ConvertHslToRgb(h4);
            ElementStylesBase.EnsureResource(keyname + "4", new SolidColorBrush(l4Color));


            //边框色

            var _a11 = hsl.H + 0.0551116009920349;
            if (_a11 > 360)
            {
                _a11 = 360;
            }
            var _a12 = hsl.L + 0.262745098039216;
            if (_a12 > 1)
            {
                _a12 = 1;
            }
            HlsColor h5 = new HlsColor();
            h5.A = hsl.A;
            h5.H = _a11;
            h5.L = _a12;
            h5.S = hsl.S;
            Color l5Color = AyColorHelper.ConvertHslToRgb(h5);
            ElementStylesBase.EnsureResource(keyname + "5", new SolidColorBrush(l5Color));
            //Console.WriteLine("<" + keyname + "1" + " value=\"" + l1Color.ToString() + "\" Des=\"自动计算\"/>");
            //Console.WriteLine("<" + keyname + "2" + " value=\"" + l2Color.ToString() + "\" Des=\"自动计算\"/>");
            //Console.WriteLine("<" + keyname + "3" + " value=\"" + l3Color.ToString() + "\" Des=\"自动计算\"/>");
            //Console.WriteLine("<" + keyname + "4" + " value=\"" + l4Color.ToString() + "\" Des=\"自动计算\"/>");
            //Console.WriteLine("<" + keyname + "5" + " value=\"" + l5Color.ToString() + "\" Des=\"自动计算\"/>");

            ElementStylesBase.EnsureResource(keyname + "1" + ".color", l1Color);
            ElementStylesBase.EnsureResource(keyname + "2" + ".color", l2Color);
            ElementStylesBase.EnsureResource(keyname + "3" + ".color", l3Color);
            ElementStylesBase.EnsureResource(keyname + "4" + ".color", l4Color);
            ElementStylesBase.EnsureResource(keyname + "5" + ".color", l5Color);
        }

        private Brush GetAccentBrushFromAccentColor()
        {
            ElementAccentColor accentColor = AccentColor;
            Brush solidColorBrushFromAccentColor = GetSolidColorBrushFromAccentColor(AccentColor);
            if (solidColorBrushFromAccentColor == null)
            {
                solidColorBrushFromAccentColor = GetSolidColorBrushFromAccentColor(GetDefaultAccentColor());
            }
            return solidColorBrushFromAccentColor;
        }
    }
}
