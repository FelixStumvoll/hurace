using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.Dal.Domain;

namespace Hurace.RaceControl.Converters
{
    public class RaceIdToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            ((int?) value ?? -1) != -1;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}