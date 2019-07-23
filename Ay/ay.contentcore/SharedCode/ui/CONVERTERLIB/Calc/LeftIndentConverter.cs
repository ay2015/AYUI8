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
    /// <summary>
    /// 距离左侧偏移多少
    /// 应用：树节点
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class LeftIndentConverter : MarkupExtension, IValueConverter
    {
        private static LeftIndentConverter _converter;
        public static LeftIndentConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new LeftIndentConverter();
                }
                return _converter;
            }
        }

        public LeftIndentConverter()
        {

        }
        public LeftIndentConverter(double Indent, double MarginLeft)
        {
            this.Indent = Indent;
            this.MarginLeft = MarginLeft;
        }


        private LeftIndentConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new LeftIndentConverter(Indent, MarginLeft);
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new LeftIndentConverter(Indent, MarginLeft);
                }
                return _converterXaml;
            }
        }


        private static LeftIndentConverter _converterXaml;


        #region 属性
        /// <summary>
        /// 每次深度缩进的单位长度
        /// </summary>
        public double Indent { get; set; }
        /// <summary>
        /// 额外的距离
        /// 计算this.MarginLeft + this.Indent * depth
        /// </summary>
        public double MarginLeft { get; set; }
        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int depth = value.ToInt();
            if (depth == 0)
            {
                return new Thickness(0);
            }
            return new Thickness(this.MarginLeft + this.Indent * depth, 0, 0, 0);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
