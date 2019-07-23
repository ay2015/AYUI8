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
    /// 画刷的颜色转字符串
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class SolidColorBrushToStringConverter : MarkupExtension, IValueConverter
    {
        private static SolidColorBrushToStringConverter _converter;
        public static SolidColorBrushToStringConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new SolidColorBrushToStringConverter();
                }
                return _converter;
            }
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
        #region 属性

        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = value as SolidColorBrush;
            if (brush != null)
                return brush.Color.ToString();

            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
