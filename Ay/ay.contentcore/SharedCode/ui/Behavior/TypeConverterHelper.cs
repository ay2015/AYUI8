using System;
using System.ComponentModel;

namespace ay.contentcore
{
    internal static class TypeConverterHelper
    {
        internal static object DoConversionFrom(TypeConverter converter, object value)
        {
            object result = value;
            try
            {
                if (converter == null)
                {
                    return result;
                }
                if (value == null)
                {
                    return result;
                }
                if (!converter.CanConvertFrom(value.GetType()))
                {
                    return result;
                }
                result = converter.ConvertFrom(value);
                return result;
            }
            catch (Exception e)
            {
                if (!ShouldEatException(e))
                {
                    throw;
                }
                return result;
            }
        }

        private static bool ShouldEatException(Exception e)
        {
            bool flag = false;
            if (e.InnerException != null)
            {
                flag |= ShouldEatException(e.InnerException);
            }
            return flag | (e is FormatException);
        }

        internal static TypeConverter GetTypeConverter(Type type)
        {
            return TypeDescriptor.GetConverter(type);
        }
    }
}
