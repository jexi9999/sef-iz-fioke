using System;

namespace SefIzFioke.Models
{
    public class Upozorenje
    {
        public int Id { get; set; }
        public string Poruka { get; set; } = string.Empty;
        public DateTime DatumSlanja { get; set; } = DateTime.Now;

        public int KorisnikId { get; set; }
        public Korisnik? Korisnik { get; set; }
    }
}