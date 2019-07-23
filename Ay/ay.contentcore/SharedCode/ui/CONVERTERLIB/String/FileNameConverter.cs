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
    [ValueConversion(typeof(string), typeof(string))]
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class FileNameConverter : MarkupExtension, IValueConverter
    {
        private static FileNameConverter _converter;
        public static FileNameConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new FileNameConverter();
                }
                return _converter;
            }
        }

        private FileNameConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new FileNameConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new FileNameConverter();
                }
                return _converterXaml;
            }

        }

        private static FileNameConverter _converterXaml;

        public FileNameConverter()
        {

        }

        //public FileNameConverter(string property)
        //{

        //}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = value.ToObjectString();
            return System.IO.Path.GetFileName(fileName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
