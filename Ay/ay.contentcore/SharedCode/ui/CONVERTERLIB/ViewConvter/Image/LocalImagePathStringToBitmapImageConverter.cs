using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;

namespace ay.Controls
{
    /// <summary>
    /// 绝对路径转图片
    /// 请用Image控件显示
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class LocalImagePathStringToBitmapImageConverter : MarkupExtension, IValueConverter
    {
        public LocalImagePathStringToBitmapImageConverter()
        {

        }
        public LocalImagePathStringToBitmapImageConverter(double? width,double? height)
        {
            this.DecodePixelWidth = width;
            this.DecodePixelHeight = height;
        }
        private static LocalImagePathStringToBitmapImageConverter _converter;
        public static LocalImagePathStringToBitmapImageConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new LocalImagePathStringToBitmapImageConverter();
                }
                return _converter;
            }
        }


        private static LocalImagePathStringToBitmapImageConverter _converterXaml;
        private LocalImagePathStringToBitmapImageConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new LocalImagePathStringToBitmapImageConverter(DecodePixelWidth, DecodePixelHeight);
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new LocalImagePathStringToBitmapImageConverter(DecodePixelWidth, DecodePixelHeight);
                }
                return _converterXaml;
            }
        }

        #region 属性
        //使用double用来利于绑定
        private double? _decodePixelWidth;
        private double? _decodePixelHeight;
        //调整图像宽度
        public double? DecodePixelWidth
        {
            get
            {
                return _decodePixelWidth;
            }
            set
            {
                _decodePixelWidth = value;
            }
        }
        //调整图像高度
        public double? DecodePixelHeight
        {
            get
            {
                return _decodePixelHeight;
            }
            set
            {
                _decodePixelHeight = value;
            }
        }
        #endregion

        #region 转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;//得到文件路径
                                          //如果文件路径存在
            if (!path.IsNullOrWhiteSpace())
            {
                if (!System.IO.File.Exists(path)) return DependencyProperty.UnsetValue;//返回未设置依赖值
                //创建一个新的BitmapImage对象以及一个新的文件流
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();//开始更新状态
                                        //指定BitmapImage的StreamSource为按指定路径打开的文件流
                bitmapImage.StreamSource = new FileStream(path, FileMode.Open, FileAccess.Read);
                if (DecodePixelWidth.HasValue)
                {
                    bitmapImage.DecodePixelWidth = (int)DecodePixelWidth.Value;//设置图像的宽度
                }
                if (DecodePixelHeight.HasValue)
                {
                    bitmapImage.DecodePixelHeight = (int)DecodePixelHeight.Value;//设置图像的高度
                }
      
                                                                        //加载Image后以便立即释放流
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();//结束更新
                                      //清除流以避免在尝试删除图像时出现文件访问异常
                bitmapImage.StreamSource.Dispose();
                return bitmapImage;//返回BitmapImage
            }
            else
            {
                return DependencyProperty.UnsetValue;//返回未设置依赖值
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }


}
