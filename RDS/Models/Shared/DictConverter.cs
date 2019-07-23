using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace RDS.Models
{
         // <ay:AyText FontSize = "24" Text="{Binding Sex,Converter={md:DictConverter},ConverterParameter='sex'}"/>
    /// <summary>
    /// Instance后台编码单例使用
    /// _converterXamlResource用于资源模式的多例
    /// _converterXaml用于直接使用转换器的单例
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class DictConverter : MarkupExtension, IValueConverter
    {
        private static DictConverter _converter;
        public static DictConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new DictConverter();
                }
                return _converter;
            }
        }

        private DictConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new DictConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new DictConverter();
                }
                return _converterXaml;
            }

        }

        private static DictConverter _converterXaml;

        public DictConverter()
        {

        }

        //public DictConverter(string property)
        //{

        //}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _1 = parameter as string;
            if (value == null) return "";
            var _2=AppBase.Instance._dicPool.FirstOrDefault(x=>x.Value==value.ToString() && x.field==_1);
            if (_2 == null)
            {
                return "";
            }
            else
            {
                return _2.Text;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _1 = parameter as string;
            var _2 = AppBase.Instance._dicPool.FirstOrDefault(x => x.Text == value.ToString() && x.field == _1);
            if (_2 == null)
            {
                return "";
            }
            else
            {
                return _2.Value;
            }
        }
    }
}
