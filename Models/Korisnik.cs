using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SefIzFioke.Models
{
    public class Korisnik
    {
        public int Id { get; set; }

        [Required]
        public string KorisnickoIme { get; set; } = string.Empty;

        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Lozinka { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;
        public bool JeBlokirn { get; set; } = false;

        public List<Recept> Recepti { get; set; } = new();
        public List<Komentar> Komentari { get; set; } = new();
        public List<Upozorenje> Upozorenja { get; set; } = new();
        public string? ProfilnaSlika { get; set; }
    }
}