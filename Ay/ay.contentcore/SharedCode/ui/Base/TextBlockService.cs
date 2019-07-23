
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ay.Controls
{
    /// <summary>
    /// 生日：2016-10-27 23:41:03
    /// 理想：简化xaml写法，设置 TextBlock的样式
    /// </summary>
    public class TextBlockService : DependencyObject
    {
        public TextBlockService()
        {

        }
        public static string GetTextStyle(DependencyObject obj)
        {
            return (string)obj.GetValue(TextStyleProperty);
        }

        public static void SetTextStyle(DependencyObject obj, string value)
        {
            obj.SetValue(TextStyleProperty, value);
        }
        ///设置TextBlock属性，顺序为： 字号 字体颜色 fontweight 字体 背景色 xie（是否斜体）  
        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.RegisterAttached("TextStyle", typeof(string), typeof(TextBlockService), new PropertyMetadata(null, OnTextStyleChanged));

        private static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _0 = d as TextBlock;
            if (_0!=null)
            {
                var _1 = (string)e.NewValue;
                if (!_1.IsNullAndTrimAndEmpty())
                {
                    string _2 = _1.Trim().Replace("，", " ").Replace(",", " ");
                    var _2s = _2.Split(' ');
                    foreach (var item in _2s)
                    {
                        //字号 字体颜色 fontweight 字体 背景色 xie（是否斜体）
                        if (_2s.Length >= 1 && _2s[0] != "?")
                            _0.FontSize = _2s[0].ToDouble();
                        if (_2s.Length >= 2 && _2s[1] != "?")
                        {
                            if (_2s[1].IndexOf("Ay") > -1)
                            {
                                _0.SetResourceReference(TextBlock.ForegroundProperty, _2s[1]);
                            }
                            else
                            {
                                TextBlock.SetForeground(_0, HexToBrush.FromHex(_2s[1]));
                            }
                        }
                        if (_2s.Length >= 3 && _2s[2] != "?")
                        {
                            _0.FontWeight = _2s[2].ToFontWeight();
                        }
                        if (_2s.Length >= 4 && _2s[3] != "?")
                        {
                            _0.FontFamily = new FontFamily(_2s[3]);
                        }
                        if (_2s.Length >= 5 && _2s[4] != "?")
                        {
                            if (_2s[4].IndexOf("Ay") > -1)
                            {
                                _0.SetResourceReference(TextBlock.BackgroundProperty, _2s[4]);
                            }
                            else
                            {
                                TextBlock.SetForeground(_0, HexToBrush.FromHex(_2s[4]));
                            }
                        }
                        if (_2s.Length >= 6 && _2s[5] != "?")
                        {
                            if (_2s[5] == "xie" || _2s[5].ToLower() == "true")
                            {
                                _0.FontStyle = FontStyles.Italic;
                            }

                        }
                    }
                }
            }
        }


    }
}
