using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ay.Controls
{
    /// <summary>
    /// 返回N个值最小的值
    ///       <ProgressBar.Width>
    ///                     <MultiBinding Converter = "{StaticResource DoubleReturnMinValueConverter}" >
    ///                                                 < Binding ElementName="Name1" Path="ActualWidth" />
    ///                                                  <Binding ElementName = "Name2" Path="MinWidth" />
    ///                                               </MultiBinding>
    ///                 </ProgressBar.Width>
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class DoubleReturnMinValueConverter : MarkupExtension, IMultiValueConverter
    {
        private static DoubleReturnMinValueConverter _converter;
        public static DoubleReturnMinValueConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new DoubleReturnMinValueConverter();
                }
                return _converter;
            }
        }
        private static DoubleReturnMinValueConverter _converterXaml;
        private DoubleReturnMinValueConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new DoubleReturnMinValueConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new DoubleReturnMinValueConverter();
                }
                return _converterXaml;
            }
        }
        #region 属性

        #endregion

        #region 转换
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double n = (double)values[0];
            if (values.Length == 1) return n;
            for (int i = 1; i < values.Length; i++)
            {
                var _1 = (double)values[i];
                if (n > _1)
                {
                    n = _1;
                }
            }
            return n;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
