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
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class CornerRadiusToRadiusX : MarkupExtension, IValueConverter
    {
        private static CornerRadiusToRadiusX _converter;
        public static CornerRadiusToRadiusX Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new CornerRadiusToRadiusX();
                }
                return _converter;
            }
        }
        private static CornerRadiusToRadiusX _converterXaml;
        private CornerRadiusToRadiusX _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new CornerRadiusToRadiusX();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new CornerRadiusToRadiusX();
                }
                return _converterXaml;
            }
        }

        #region 属性

        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _1=(CornerRadius)value;
            return _1.TopLeft;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _1 = (double)value;
            return new CornerRadius(_1,_1,_1,_1);
        }
        #endregion
    }

}
