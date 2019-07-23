using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ay.Controls
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class DoubleExtCalcConverter : MarkupExtension, IValueConverter
    {
        private static DoubleExtCalcConverter _converter;
        public static DoubleExtCalcConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new DoubleExtCalcConverter();
                }
                return _converter;
            }
        }
        private static DoubleExtCalcConverter _converterXaml;
        private DoubleExtCalcConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new DoubleExtCalcConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new DoubleExtCalcConverter();
                }
                return _converterXaml;
            }
        }

        #region 属性

        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string t = parameter as string;
            double vi = value.ToDouble();
            var ps = t.Split('#');
            double ps0 = value.ToDouble();
            double ps1 = ps[1].ToDouble();
            if (ps == null)
            {
                return 0;
            }
            if (ps.Length == 2)
            {
                char d = ps[0][0];
                switch (d)
                {
                    case '1':
                        return ps0 + ps1;

                    case '2':
                        return ps0 - ps1;

                    case '3':
                        return ps0 * ps1;

                    case '4':
                        if (ps1 == 0)
                        {
                            return ps1;
                        }
                        return ps0 / ps1;

                    default:
                        return ps0 + ps1; ;
                }
            }
            return 0;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
