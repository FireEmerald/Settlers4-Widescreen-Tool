using System;
using System.Globalization;
using System.Windows.Data;

namespace Settlers.Toolbox.Views.Converter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) throw new InvalidOperationException($"Passed type is invalid, {nameof(InvertBoolConverter)} requires a {typeof(bool)}.");

            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}