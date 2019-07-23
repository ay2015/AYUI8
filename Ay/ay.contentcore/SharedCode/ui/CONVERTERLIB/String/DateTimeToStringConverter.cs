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
    public class DateTimeToStringConverter : MarkupExtension, IValueConverter
    {
        private static DateTimeToStringConverter _converter;
        public static DateTimeToStringConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new DateTimeToStringConverter();
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
            DateTime vi = value.ToDateTime();
            string t = parameter as string;
            return vi.ToString(t);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
