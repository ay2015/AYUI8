using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ay.Controls
{
    /// <summary>
    /// )=100:123123,(100&)=50:234234,(50&)=20:456456#999999
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class IfElseColorConverter : MarkupExtension, IValueConverter
    {
        private static IfElseColorConverter _converter;
        public static IfElseColorConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new IfElseColorConverter();
                }
                return _converter;
            }
        }

        private static IfElseColorConverter _converterXaml;

        private IfElseColorConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new IfElseColorConverter(Formatter);
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new IfElseColorConverter(Formatter);
                }
                return _converterXaml;
            }

        }
        public IfElseColorConverter()
        {

        }
        private string _Formatter;

        public string Formatter
        {
            get { return _Formatter; }
            set { _Formatter = value; }
        }
        public IfElseColorConverter(string formatter)
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
                GroupResult = CompareSubGroup(SubGroup, _value);
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
                    return Binding.DoNothing;
                }

            }
            if (GroupResult == "None")
            {
                return DependencyProperty.UnsetValue;
            }
            return HexToBrush.FromHex("#"+GroupResult);

        }

        public static string CompareSubGroup(string SubGroup, double _value)
        {

            string SubGroupResult = null;

            if (SubGroup.IndexOf("&") > -1)
            {
                var _232 = SubGroup.Split(':');
                var _11 = _232[0].Split('&');
                bool yanZhengResult = true;
                for (int kk = 0; kk < _11.Length; kk++)
                {
                    string _singleStr = _11[kk];

                    if (_singleStr.IndexOf("(=") == 0)
                    {
                        var _111 = _singleStr.Substring(2);
                        double _123 = _111.ToDouble();
                        if (_value <= _123)
                        {
                            continue;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过
                            break;
                        }
                    }
                    else if (_singleStr.IndexOf(")=") == 0)
                    {
                        var _111 = _singleStr.Substring(2);
                        double _123 = _111.ToDouble();
                        if (_value >= _123)
                        {
                            continue;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过
                            break;
                        }
                    }
                    else if (_singleStr.IndexOf("==") == 0)
                    {
                        var _111 = _singleStr.Substring(2);
                        double _123 = _111.ToDouble();
                        if (_value == _123)
                        {
                            continue;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过
                            break;
                        }
                    }
                    else if (_singleStr.IndexOf("(") == 0)
                    {
                        var _111 = _singleStr.Substring(1);
                        double _123 = _111.ToDouble();
                        if (_value < _123)
                        {
                            continue;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过
                            break;
                        }
                    }
                    else
                    if (_singleStr.IndexOf(")") == 0)
                    {
                        var _111 = _singleStr.Substring(1);
                        double _123 = _111.ToDouble();
                        if (_value > _123)
                        {
                            continue;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过
                            break;
                        }
                    }

                }
                if (yanZhengResult)
                {
                    SubGroupResult = _232[1];
                }
            }
            else if (SubGroup.IndexOf("|") > -1)
            {
                var _232 = SubGroup.Split(':');
                var _11 = _232[0].Split('|');
                bool yanZhengResult = true;
                for (int kk = 0; kk < _11.Length; kk++)
                {
                    string _singleStr = _11[kk];

                    if (_singleStr.IndexOf("(=") == 0)
                    {
                        var _111 = _singleStr.Substring(2);
                        double _123 = _111.ToDouble();
                        if (_value <= _123)
                        {
                            yanZhengResult = true;//验证通过
                            break;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过

                        }
                    }

                    else if (_singleStr.IndexOf(")=") == 0)
                    {
                        var _111 = _singleStr.Substring(2);
                        double _123 = _111.ToDouble();
                        if (_value >= _123)
                        {
                            yanZhengResult = true;//验证通过
                            break;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过

                        }
                    }
                    else if (_singleStr.IndexOf("==") == 0)
                    {
                        var _111 = _singleStr.Substring(2);
                        double _123 = _111.ToDouble();
                        if (_value == _123)
                        {
                            yanZhengResult = true;//验证通过
                            break;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过

                        }
                    }
                    else if (_singleStr.IndexOf("(") == 0)
                    {
                        var _111 = _singleStr.Substring(1);
                        double _123 = _111.ToDouble();
                        if (_value < _123)
                        {
                            yanZhengResult = true;//验证通过
                            break;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过

                        }
                    }
                    else
                    if (_singleStr.IndexOf(")") == 0)
                    {
                        var _111 = _singleStr.Substring(1);
                        double _123 = _111.ToDouble();
                        if (_value > _123)
                        {
                            yanZhengResult = true;//验证通过
                            break;
                        }
                        else
                        {
                            yanZhengResult = false;//验证不通过

                        }
                    }

                }
                if (yanZhengResult)
                {
                    SubGroupResult = _232[1];
                }
            }
            else
            {
                SubGroupResult = GetExpressResult(SubGroup, _value);
            }

            return SubGroupResult;
        }

        private static string GetExpressResult(string d, double _value)
        {
            string _result = null;
            if (d.IndexOf(")=") == 0)
            {
                var _111 = d.Substring(2).Split(':');
                double _123 = _111[0].ToDouble();
                if (_value >= _123)
                {
                    _result = _111[1];
                }
            }
            else
          if (d.IndexOf(">") == 0)
            {
                var _111 = d.Substring(1).Split(':');
                double _123 = _111[0].ToDouble();
                if (_value > _123)
                {
                    _result = _111[1];
                }
            }
            else if (d.IndexOf("(=") == 0)
            {
                var _111 = d.Substring(2).Split(':');
                double _123 = _111[0].ToDouble();
                if (_value <= _123)
                {
                    _result = _111[1];
                }
            }
            else if (d.IndexOf("==") == 0)
            {
                var _111 = d.Substring(2).Split(':');
                double _123 = _111[0].ToDouble();
                if (_value == _123)
                {
                    _result = _111[1];
                }
            }
            else if (d.IndexOf("(") == 0)
            {
                var _111 = d.Substring(1).Split(':');
                double _123 = _111[0].ToDouble();
                if (_value < _123)
                {
                    _result = _111[1];
                }
            }


            return _result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
