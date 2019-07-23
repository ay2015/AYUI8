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
    /// 画刷转color，color转画刷
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class SolidColorBrushToColorConverter : MarkupExtension, IValueConverter
    {
        private static SolidColorBrushToColorConverter _converter;
        public static SolidColorBrushToColorConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new SolidColorBrushToColorConverter();
                }
                return _converter;
            }
        }
        private static SolidColorBrushToColorConverter _converterXaml;
        private SolidColorBrushToColorConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new SolidColorBrushToColorConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new SolidColorBrushToColorConverter();
                }
                return _converterXaml;
            }
        }
        #region 属性

        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = value as SolidColorBrush;
            if (brush != null)
                return brush.Color;

            return default(System.Windows.Media.Color?);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                System.Windows.Media.Color color = (System.Windows.Media.Color)value;
                return new SolidColorBrush(color);
            }

            return default(SolidColorBrush);
        }
        #endregion
    }


}
