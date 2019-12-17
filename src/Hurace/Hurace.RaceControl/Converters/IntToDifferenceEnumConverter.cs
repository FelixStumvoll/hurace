using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.Converters
{
    public class IntToDifferenceEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DifferenceType.Higher;
            var intVal = (int) value;
            if (intVal == 0) return DifferenceType.Equal;
            return intVal > 0 ? DifferenceType.Higher : DifferenceType.Lower;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}