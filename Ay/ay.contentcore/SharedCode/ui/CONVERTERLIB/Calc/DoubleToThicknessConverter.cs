using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ay.Controls
{
    /// <summary>
    /// 将一个double的数字，转换为一个thickness的指定方位的值
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class DoubleToThicknessConverter : MarkupExtension, IValueConverter
    {
        private static DoubleToThicknessConverter _converter;
        public static DoubleToThicknessConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new DoubleToThicknessConverter();
                }
                return _converter;
            }
        }
        private static DoubleToThicknessConverter _converterXaml;
        private DoubleToThicknessConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new DoubleToThicknessConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new DoubleToThicknessConverter();
                }
                return _converterXaml;
            }
        }

        #region 属性

        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string t = parameter as string;
            if (t == null)
            {
                t = "1";
            }
            double v = value.ToInt();
            switch (t)
            {
                case "1":
                    return new Thickness(0, v, 0, 0);
                case "2":
                    return new Thickness(0, 0, 0, v);
                case "3":
                    return new Thickness(v, 0, 0, 0);
                case "4":
                    return new Thickness(0, 0, v, 0);
                default:
                    return new Thickness(0, v, 0, 0);
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string t = parameter as string;
            if (t == null)
            {
                t = "1";
            }
            Thickness v = (Thickness)value;
            switch (t)
            {
                case "1":
                    return v.Top;
                case "2":
                    return v.Bottom;
                case "3":
                    return v.Left;
                case "4":
                    return v.Right;
                default:
                    return v.Top;
            }
        }
        #endregion
    }


}
