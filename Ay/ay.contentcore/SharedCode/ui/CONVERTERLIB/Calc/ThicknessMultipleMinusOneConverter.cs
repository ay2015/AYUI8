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
    /// 把Thickness的值都乘以 -1 得到新的Thickness
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class ThicknessMultipleMinusOneConverter : MarkupExtension, IValueConverter
    {
        private static ThicknessMultipleMinusOneConverter _converter;
        public static ThicknessMultipleMinusOneConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new ThicknessMultipleMinusOneConverter();
                }
                return _converter;
            }
        }
        private static ThicknessMultipleMinusOneConverter _converterXaml;
        private ThicknessMultipleMinusOneConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new ThicknessMultipleMinusOneConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new ThicknessMultipleMinusOneConverter();
                }
                return _converterXaml;
            }
        }
        #region 属性

        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _1 = (Thickness)value;
            return new Thickness(_1.Left * (-1), _1.Top * (-1), _1.Right * (-1), _1.Bottom * (-1));
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
