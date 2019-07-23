using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace ay.Controls
{
    /// <summary>
    /// 0|1:男,2:女,3:不男不女#未知
    /// 0:男,1:女,2:不男不女#未知
    /// 0:男:女
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class IfElseConverter : MarkupExtension, IValueConverter
    {
        private static IfElseConverter _converter;
        public static IfElseConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new IfElseConverter();
                }
                return _converter;
            }
        }

        private IfElseConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new IfElseConverter(Formatter);
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new IfElseConverter(Formatter);
                }
                return _converterXaml;
            }

        }

        private static IfElseConverter _converterXaml;

        public IfElseConverter()
        {

        }
        private string _Formatter;

        public string Formatter
        {
            get { return _Formatter; }
            set { _Formatter = value; }
        }
        public IfElseConverter(string formatter)
        {
            this.Formatter = formatter;
        }
        private static readonly char[] expressionSplit = new char[] { ':' };
        private static readonly char[] groupEndSplit = new char[] { '#' };
        private static readonly char[] groupSplit = new char[] { ',' };
        private static readonly char[] valueSplit = new char[] { '|' };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /*
            parameter：v1|v2|v3:r1:2
            */
            var _1 = Formatter.ToObjectString();
            if (_1 == "")
            {
                return "";
            }
            if (_1.IndexOf(',') > -1)
            {
                string[] arrayEndGroup = _1.Split(groupEndSplit);
                string[] array1 = arrayEndGroup[0].Split(groupSplit);

                string valStr = value.ToObjectString();
                for (int i = 0; i < array1.Length; i++)
                {
                    var _21 = array1[i];
                    string[] array = _21.Split(expressionSplit);
                    if (array[0].Contains(valueSplit[0]))
                    {
                        string[] valStrArray = array[0].Split(valueSplit);
                        if (valStrArray.Contains(valStr))
                        {
                            return array[1].Replace("{0}", value.ToObjectString());
                        }
                    }
                    else
                    {
                        if (valStr.Equals(array[0]))
                        {
                            return array[1].Replace("{0}", value.ToObjectString());
                        }
                    }
                }
                if (arrayEndGroup.Length == 2)
                {
                    return arrayEndGroup[1].Replace("{0}", value.ToObjectString());
                }
                return "";
            }
            else
            {
                string[] array = _1.Split(expressionSplit);
                if (value == null)
                    return array[1];
                string valStr = value.ToString();
                if (string.IsNullOrEmpty(valStr))
                    return array[1];
                if (array[0].Contains("|"))
                {
                    string[] valStrArray = array[0].Split(valueSplit);
                    return valStrArray.Contains(valStr) ? array[1].Replace("{0}", value.ToObjectString()) : array[2].Replace("{0}", value.ToObjectString());
                }
                else
                {
                    return valStr.Equals(array[0]) ? array[1].Replace("{0}", value.ToObjectString()) : array[2].Replace("{0}", value.ToObjectString());
                }
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
