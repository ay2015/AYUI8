using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ay.Controls
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class ByteArrayToBitmapImageConverter : MarkupExtension, IValueConverter
    {
        private static ByteArrayToBitmapImageConverter _converter;
        public static ByteArrayToBitmapImageConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new ByteArrayToBitmapImageConverter();
                }
                return _converter;
            }
        }
        public ByteArrayToBitmapImageConverter()
        {

        }
        public ByteArrayToBitmapImageConverter(int? Width,int? Height)
        {
            this.DecodePixelWidth = Width;
            this.DecodePixelHeight = Height;
        }
     

        private static ByteArrayToBitmapImageConverter _converterXaml;
        private ByteArrayToBitmapImageConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new ByteArrayToBitmapImageConverter(DecodePixelWidth, DecodePixelHeight);
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new ByteArrayToBitmapImageConverter(DecodePixelWidth, DecodePixelHeight);
                }
                return _converterXaml;
            }
        }

        #region 属性
        private int? width;

        public int? DecodePixelWidth
        {
            get { return width; }
            set { width = value; }
        }
        private int? height;

        public int? DecodePixelHeight
        {
            get { return height; }
            set { height = value; }
        }
        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageResources.ByteArrayToBitmapImage((byte[])value, DecodePixelWidth, DecodePixelHeight);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
