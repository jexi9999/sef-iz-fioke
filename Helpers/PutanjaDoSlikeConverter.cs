using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SefIzFioke.Helpers
{
    public class PutanjaDoSlikeConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
                return SlikaHelper.UcitajSliku(path);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}