using Microsoft.EntityFrameworkCore;
using SefIzFioke.Models;
using System;
using System.IO;

namespace SefIzFioke.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Recept> Recepti { get; set; }
        public DbSet<Komentar> Komentari { get; set; }
        public DbSet<Upozorenje> Upozorenja { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SefIzFioke");
            Directory.CreateDirectory(dbFolder);
            string dbPath = Path.Combine(dbFolder, "baza.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Korisnik>()
                .HasIndex(k => k.KorisnickoIme).IsUnique();

            modelBuilder.Entity<Korisnik>()
                .HasIndex(k => k.Email).IsUnique();

            modelBuilder.Entity<Korisnik>()
                .HasMany(k => k.Recepti)
                .WithOne(r => r.Korisnik)
                .HasForeignKey(r => r.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Korisnik>()
                .HasMany(k => k.Komentari)
                .WithOne(k => k.Korisnik)
                .HasForeignKey(k => k.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Korisnik>()
                .HasMany(k => k.Upozorenja)
                .WithOne(u => u.Korisnik)
                .HasForeignKey(u => u.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recept>()
                .HasMany(r => r.Komentari)
                .WithOne(k => k.Recept)
                .HasForeignKey(k => k.ReceptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}