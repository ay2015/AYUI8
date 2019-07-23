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
    public class DoubleZeroToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static DoubleZeroToVisibilityConverter _converter;
        public static DoubleZeroToVisibilityConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new DoubleZeroToVisibilityConverter();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">等于1 表示反转结果</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = parameter.ToObjectString();
            if (p != "1")
            {
                var d = value.ToDouble();
                if (d > 0)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            else
            {
                var d = value.ToDouble();
                if (d ==0)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
          

        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
