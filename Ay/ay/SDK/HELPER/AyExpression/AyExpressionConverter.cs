
using System;
using System.Globalization;
using System.Windows.Data;

namespace ay.AyExpression
{
    public class AyExpressionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            string ayexpression = parameter as string;
            return AyExpression.GetMaskedValue(ayexpression, text).ToObjectString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }

    }

    public class AyExpressionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value[0] as string;
            string ayexpression = value[1] as string;

            return AyExpression.GetMaskedValue(ayexpression, text).ToObjectString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }


}
