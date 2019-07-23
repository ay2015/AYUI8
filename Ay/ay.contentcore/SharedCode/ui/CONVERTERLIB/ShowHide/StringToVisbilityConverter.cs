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
    [ValueConversion(typeof(string),typeof(Visibility),ParameterType =typeof(string))]
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class StringToVisbilityConverter : MarkupExtension, IValueConverter
    {
        private static StringToVisbilityConverter _converter;
        public static StringToVisbilityConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new StringToVisbilityConverter();
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
            string _1 = value as string;
            if (_1.IsNullAndTrimAndEmpty())
            {
                return Visibility.Collapsed;
            }
            var _2 = parameter as string;
            if (!_2.IsNullAndTrimAndEmpty())
            {
                if (_2.ToStringList('#').Contains(_1))
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
