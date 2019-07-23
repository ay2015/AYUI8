using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace ay.Controls
{
    /// <summary>
    /// Instance后台编码单例使用
    /// _converterXamlResource用于资源模式的多例
    /// _converterXaml用于直接使用转换器的单例
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class DateToSmartStringConverter : MarkupExtension, IValueConverter
    {
        private static DateToSmartStringConverter _converter;
        public static DateToSmartStringConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new DateToSmartStringConverter();
                }
                return _converter;
            }
        }

        private DateToSmartStringConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new DateToSmartStringConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new DateToSmartStringConverter();
                }
                return _converterXaml;
            }

        }

        private static DateToSmartStringConverter _converterXaml;

        public DateToSmartStringConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AyCommon.GetDateString(value.ToDateTime());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
