using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.Dal.Domain;

namespace Hurace.RaceControl.Converters
{
    public class RaceRunningBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Race) value)?.RaceStateId == (int) Constants.RaceState.Running;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}