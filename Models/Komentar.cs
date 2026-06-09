using System.ComponentModel.DataAnnotations;

namespace SefIzFioke.Models
{
    public class Komentar
    {
        public int Id { get; set; }

        public string Tekst { get; set; } = string.Empty;

        public int Ocena { get; set; } = 0; // 1-5 zvezdica, 0 = nije ocenjen

        public string MediaPath { get; set; } = string.Empty;

        public int ReceptId { get; set; }
        public Recept? Recept { get; set; }

        public int KorisnikId { get; set; }
        public Korisnik? Korisnik { get; set; }
    }
}