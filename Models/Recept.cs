using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SefIzFioke.Models
{
    public class Recept
    {
        public int Id { get; set; }

        [Required]
        public string Naziv { get; set; } = string.Empty;

        public string KratakOpis { get; set; } = string.Empty;

        // Kategorije namirnica (npr. "Mleko,Meso")
        public string Namirnice { get; set; } = string.Empty;

        // Pun spisak sastojaka
        public string Sastojci { get; set; } = string.Empty;

        public string NacinPripreme { get; set; } = string.Empty;

        public string DijetetskiRestrikcije { get; set; } = string.Empty;

        public string Napomene { get; set; } = string.Empty;

        public int BrojPorcija { get; set; }

        public string VremePripreme { get; set; } = string.Empty;

        // Putanja do glavne slike (lokalni fajl)
        public string SlikaPath { get; set; } = string.Empty;

        // Putanje do dodatnih slika (odvojene sa |)
        public string DodatneSlika { get; set; } = string.Empty;

        // Putanja do videa
        public string VideoPath { get; set; } = string.Empty;

        public int KorisnikId { get; set; }
        public Korisnik? Korisnik { get; set; }

        public List<Komentar> Komentari { get; set; } = new();
    }
}
