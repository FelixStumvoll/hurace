using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.Dal.Domain;
using static Hurace.Dal.Domain.Constants;
using RaceState = Hurace.Dal.Domain.RaceState;

namespace Hurace.RaceControl.Converters
{
    public class RaceStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) (value ?? 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}