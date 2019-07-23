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
    public class ScaleToPercentConverter : MarkupExtension, IValueConverter
    {
        private static ScaleToPercentConverter _converter;
        public static ScaleToPercentConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new ScaleToPercentConverter();
                }
                return _converter;
            }
        }
        private static ScaleToPercentConverter _converterXaml;
        private ScaleToPercentConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new ScaleToPercentConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new ScaleToPercentConverter();
                }
                return _converterXaml;
            }
        }
        #region 属性

        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)(int)(value.ToDouble() * 100.0);    
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 100.0;
        }
        #endregion
    }


}
