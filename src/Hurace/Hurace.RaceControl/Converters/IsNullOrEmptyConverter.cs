using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.RaceControl.Extensions;

namespace Hurace.RaceControl.Converters
{
    public class IsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null || ((string) value).IsNullOrEmpty();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}