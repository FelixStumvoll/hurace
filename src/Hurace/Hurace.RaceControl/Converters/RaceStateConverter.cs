using System;
using System.Globalization;
using System.Windows.Data;

namespace Hurace.RaceControl.Converters
{
    public class RaceStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (int) (value ?? 0);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}