﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Hurace.RaceControl.Converters
{
    public class IsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || string.IsNullOrEmpty((string) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}