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
    public class DoubleToRoundDoubleConverter : MarkupExtension, IValueConverter
    {
        private static DoubleToRoundDoubleConverter _converter;
        public static DoubleToRoundDoubleConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new DoubleToRoundDoubleConverter();
                }
                return _converter;
            }
        }
        public DoubleToRoundDoubleConverter()
        {

        }
        public DoubleToRoundDoubleConverter(int Digits)
        {
            this.Digits = Digits;
        }
        private static DoubleToRoundDoubleConverter _converterXaml;


        private DoubleToRoundDoubleConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new DoubleToRoundDoubleConverter(Digits);
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new DoubleToRoundDoubleConverter(Digits);
                }
                return _converterXaml;
            }
        }
        #region 属性
        /// <summary>
        /// 保留小数的位数
        /// </summary>
        public int Digits { get; set; } = 3;
        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter != null)
                {
                    double digits = parameter.ToDouble();
                    double number = Double.Parse(value.ToString());

                    if (digits.Equals(Double.NaN) && Math.Abs(number) <
                                    Math.Pow(10, Digits) && number != 0)
                    {
                        return Math.Round(number, (int)Math.Floor
                                     (Digits - Math.Log10(Math.Abs(number))));
                    }
                    else if (digits.Equals(Double.NaN) && Math.Abs(number) >=
                                      Math.Pow(10, Digits))
                    {
                        return Math.Round(number, 0);
                    }
                    else if (!digits.Equals(Double.NaN))
                    {
                        return Math.Round(number, (int)digits);
                    }
                }

                return value;
            }
            catch
            {
                return value;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
