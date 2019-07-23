using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ay.Controls
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class BoolToValueConverter : MarkupExtension, IValueConverter
    {
        private static BoolToValueConverter _converter;
        public static BoolToValueConverter Instance
        {
            get
            {
                if (_converter == null)
                {
                    _converter = new BoolToValueConverter();
                }
                return _converter;
            }
        }
        private static BoolToValueConverter _converterXaml;
        private BoolToValueConverter _converterXamlResource;
        public bool IsResource { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsResource)
            {
                _converterXamlResource = new BoolToValueConverter();
                return _converterXamlResource;
            }
            else
            {
                if (_converterXaml == null)
                {
                    _converterXaml = new BoolToValueConverter();
                }
                return _converterXaml;
            }
        }
        #region IValueConverter Members

        /// <summary>
        /// The value to convert to if the input boolean is true
        /// </summary>
        public object TrueValue { get; set; }
        /// <summary>
        /// The value to convert to if the input boolean is false
        /// </summary>
        public object FalseValue { get; set; }
        /// <summary>
        /// The value to convert to if the input boolean is true and in Silverlight
        /// </summary>
        public object TrueSLValue { get; set; }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = false;
            if (value is bool)
            {
                v = (bool)value;
            }
            if (value is int)
            {
                v = ((int)value) >= 1;
            }

            return v ?
#if !SILVERLIGHT
                TrueValue
#else
                TrueSLValue ?? TrueValue
#endif
                :
                FalseValue;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
