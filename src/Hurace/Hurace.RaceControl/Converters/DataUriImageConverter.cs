using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Hurace.RaceControl.Converters
{
    public class DataUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var data = (string) value;
            if(!data.StartsWith("data:image")) return value;
            var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = System.Convert.FromBase64String(base64Data);

            var image = new BitmapImage();
            using (var mem = new MemoryStream(binData))
            {
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}