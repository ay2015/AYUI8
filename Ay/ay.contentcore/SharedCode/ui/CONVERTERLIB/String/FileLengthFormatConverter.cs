using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using ay.contentcore;

namespace ay.Controls
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class FileLengthFormatConverter : MarkupExtension, IValueConverter
    {
        private static FileLengthFormatConverter _converter;
        public static FileLengthFormatConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new FileLengthFormatConverter();
                }
                return _converter;
            }
        }
        private static FileLengthFormatConverter _converterXaml;
        private FileLengthFormatConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new FileLengthFormatConverter(ZeroString);
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new FileLengthFormatConverter(ZeroString);
                }
                return _converterXaml;
            }
        }
        public FileLengthFormatConverter()
        {
                
        }
        public FileLengthFormatConverter(string ZeroString)
        {
            this.ZeroString = ZeroString;
        }
        #region 属性
        public string ZeroString { get; set; } = "-";
        #endregion


        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double ext =value.ToDouble();
            string para = parameter.ToObjectString();
            if (ext == 0)
            {
                if (para == "1") return "0B";
                return ZeroString;
            }
            return AyFuncDisk.Instance.GetFileOrDirectoryFormatedSize(ext);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
