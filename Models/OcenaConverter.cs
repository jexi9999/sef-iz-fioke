using System;
using System.Globalization;
using System.Windows.Data;

namespace SefIzFioke.Helpers
{
    public class OcenaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int ocena && ocena > 0)
            {
                string zvezde = new string('⭐', ocena) + new string('☆', 5 - ocena);
                return zvezde;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}