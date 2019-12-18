using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.Converters
{
    public class TimespanDifferenceEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DifferenceType.Higher;
            var intVal = (TimeSpan) value;
            if (intVal == TimeSpan.Zero) return DifferenceType.Equal;
            return intVal > TimeSpan.Zero ? DifferenceType.Higher : DifferenceType.Lower;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}