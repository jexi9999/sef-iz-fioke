using SefIzFioke.Models;

namespace SefIzFioke.Helpers
{
    /// <summary>
    /// Cuva informacije o trenutno ulogovanom korisniku.
    /// </summary>
    public static class SessionManager
    {
        public static Korisnik? TrenutniKorisnik { get; set; } = null;

        public static bool JePrijavljen => TrenutniKorisnik != null;

        public static void Odjavi()
        {
            TrenutniKorisnik = null;
        }
    }
}
