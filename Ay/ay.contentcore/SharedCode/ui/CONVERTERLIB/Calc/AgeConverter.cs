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
    public class AgeConverter : MarkupExtension, IValueConverter
    {
         private static AgeConverter _converter;
        public static AgeConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new AgeConverter();
                }
                return _converter;
            }
        }

        private AgeConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new AgeConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new AgeConverter();
                }
                return _converterXaml;
            }

        }

        private static AgeConverter _converterXaml;

        public AgeConverter()
        {

        }

        //public AgeConverter(string property)
        //{

        //}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int age = 0;
            DateTime datetime;
            if (value == null)
            {
                return age;
            }
            if (value.GetType() == typeof(DateTime))
            {
                datetime = (DateTime)value;
                if (datetime == DateTime.MinValue)
                    age = 0;
                else
                    age = DateTime.Now.Year - datetime.Year;
            }

            else if (value.GetType() == typeof(String))
            {
                if (DateTime.TryParse((string)value, out datetime))
                {
                    if (datetime == DateTime.MinValue)
                        age = 0;
                    else
                        age = DateTime.Now.Year - datetime.Year;
                }
            }
            else if (value.GetType() == typeof(int))
            {
                age = (int)value;
            }
            else if (value.GetType() == typeof(DateTimeOffset))
            {
                DateTimeOffset dt;
                DateTimeOffset.TryParse(value.ToString(), out dt);
                if (dt.Date != DateTime.MinValue.Date)
                {
                    age = DateTime.Now.Year - dt.ToUniversalTime().Year;
                }
                else
                    age = 0;
            }
            return age;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int age = 0;
            DateTime datetime;
            if (value == null)
            {
                return age;
            }
            if (value.GetType() == typeof(DateTime))
            {
                datetime = (DateTime)value;
                age = DateTime.Now.Year - datetime.Year;
            }
            else if (value.GetType() == typeof(String))
            {
                if (DateTime.TryParse((string)value, out datetime))
                {
                    age = DateTime.Now.Year - datetime.Year;
                }
            }
            else if (value.GetType() == typeof(int))
            {
                age = (int)value;
            }
            return age;
        }
    }
}
