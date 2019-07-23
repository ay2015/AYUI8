using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ay.Controls
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class ImageSourceConverter : MarkupExtension, IValueConverter
    {
        private static ImageSourceConverter _converter;
        public static ImageSourceConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new ImageSourceConverter();
                }
                return _converter;
            }
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!targetType.IsAssignableFrom(typeof(ImageSource)))
            {
                return DependencyProperty.UnsetValue;
            }
            if (value == null || value is ImageSource)
            {
                return value;
            }
          
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.UriSource = new Uri(value.ToString(), UriKind.Relative);
            bi.EndInit();

            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


}
