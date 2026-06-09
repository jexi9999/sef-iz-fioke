using System;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;

namespace SefIzFioke.Helpers
{
    public static class SlikaHelper
    {
        private static readonly HttpClient _http = new HttpClient();

        static SlikaHelper()
        {
            _http.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        }

        public static BitmapImage? UcitajSliku(string? putanja)
        {
            if (string.IsNullOrEmpty(putanja)) return null;

            try
            {
                if (putanja.StartsWith("http://") || putanja.StartsWith("https://"))
                {
                    byte[] bytes = _http.GetByteArrayAsync(putanja).GetAwaiter().GetResult();
                    var ms = new MemoryStream(bytes);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
                else if (File.Exists(putanja))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(putanja);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}