using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace ay.Controls
{
    public class RectVisualConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double x = 0;
            double y = 0;
            double width = 0;
            double height = 0;
            if (values[0] != DependencyProperty.UnsetValue)
            {
                x = (double)values[0];
            }
            if (values[1] != DependencyProperty.UnsetValue)
            {
                y = (double)values[1];
            }
            if (values[2] != DependencyProperty.UnsetValue)
            {
                width = (double)values[2];
            }
            if (values[3] != DependencyProperty.UnsetValue)
            {
                height = (double)values[3];
            }

            return new System.Windows.Rect(x, y, width, height);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
