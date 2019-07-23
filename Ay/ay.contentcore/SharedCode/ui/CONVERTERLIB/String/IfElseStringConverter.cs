using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ay.Controls
{
    /// <summary>
    /// >=100:123123,<100&>=50:234234,<50&>=20:456456#999999
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class IfElseStringConverter : MarkupExtension, IValueConverter
    {
        private static IfElseStringConverter _converter;
        public static IfElseStringConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new IfElseStringConverter();
                }
                return _converter;
            }
        }

        private IfElseStringConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new IfElseStringConverter(Formatter);
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new IfElseStringConverter(Formatter);
                }
                return _converterXaml;
            }

        }

        private static IfElseStringConverter _converterXaml;


        public IfElseStringConverter()
        {

        }
        private string _Formatter;

        public string Formatter
        {
            get { return _Formatter; }
            set { _Formatter = value; }
        }
        public IfElseStringConverter(string formatter)
        {
            this.Formatter = formatter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _formatter = Formatter.ToObjectString();
            double _value = value.ToDouble();
            string[] arrayEndGroup = _formatter.Split('#');
            string GroupResult = null;
            string[] _subgroupArray = arrayEndGroup[0].Split(',');
            foreach (var SubGroup in _subgroupArray)
            {
                GroupResult = IfElseColorConverter.CompareSubGroup(SubGroup, _value);
                if (GroupResult == null)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (GroupResult == null)
            {
                if (arrayEndGroup.Length > 1)
                {
                    GroupResult = arrayEndGroup[1];
                }
                else
                {
                    GroupResult = "";
                }

            }
            return GroupResult.Replace("{0}", value.ToObjectString());

        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
