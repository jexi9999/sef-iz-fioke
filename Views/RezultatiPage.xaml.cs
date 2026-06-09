using Microsoft.EntityFrameworkCore;
using SefIzFioke.Data;
using SefIzFioke.Dialogs;
using SefIzFioke.Helpers;
using SefIzFioke.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SefIzFioke.Views
{
    public partial class RezultatiPage : Page
    {
        private readonly FilteriPretrage _filteri;

        public RezultatiPage(FilteriPretrage filteri)
        {
            InitializeComponent();
            _filteri = filteri;
            UcitajRezultate();
        }

        private void UcitajRezultate()
        {
            using var db = new AppDbContext();
            var svi = db.Recepti
                .Include(r => r.Korisnik)
                .Include(r => r.Komentari)
                .ToList();

            // Prikupi sve aktivne filtere
            var aktivni = new List<Func<Recept, bool>>();

            if (!string.IsNullOrWhiteSpace(_filteri.ImeRecepta))
                aktivni.Add(r => r.Naziv.Contains(_filteri.ImeRecepta, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(_filteri.Dijeta))
                aktivni.Add(r => Sadrzi(r.DijetetskiRestrikcije, _filteri.Dijeta)
                               || Sadrzi(r.KratakOpis, _filteri.Dijeta));

            if (!string.IsNullOrWhiteSpace(_filteri.Alergije))
                aktivni.Add(r => Sadrzi(r.DijetetskiRestrikcije, _filteri.Alergije)
                               || Sadrzi(r.Namirnice, _filteri.Alergije)
                               || Sadrzi(r.Sastojci, _filteri.Alergije));

            if (!string.IsNullOrWhiteSpace(_filteri.Mleko))
                aktivni.Add(r => Sadrzi(r.Namirnice, _filteri.Mleko)
                               || Sadrzi(r.Sastojci, _filteri.Mleko)
                               || Sadrzi(r.DijetetskiRestrikcije, _filteri.Mleko));

            if (!string.IsNullOrWhiteSpace(_filteri.Meso))
                aktivni.Add(r => Sadrzi(r.Namirnice, _filteri.Meso)
                               || Sadrzi(r.Sastojci, _filteri.Meso)
                               || Sadrzi(r.DijetetskiRestrikcije, _filteri.Meso));

            if (!string.IsNullOrWhiteSpace(_filteri.Namirnice))
                aktivni.Add(r => Sadrzi(r.Namirnice, _filteri.Namirnice)
                               || Sadrzi(r.Sastojci, _filteri.Namirnice));

            if (!string.IsNullOrWhiteSpace(_filteri.Slatko))
                aktivni.Add(r => Sadrzi(r.Namirnice, _filteri.Slatko)
                               || Sadrzi(r.KratakOpis, _filteri.Slatko));

            if (!string.IsNullOrWhiteSpace(_filteri.Izbegavati))
                aktivni.Add(r => Sadrzi(r.DijetetskiRestrikcije, _filteri.Izbegavati)
                               || Sadrzi(r.Namirnice, _filteri.Izbegavati));

            if (!string.IsNullOrWhiteSpace(_filteri.Religija))
                aktivni.Add(r => Sadrzi(r.DijetetskiRestrikcije, _filteri.Religija));

            if (!string.IsNullOrWhiteSpace(_filteri.Intolerancije))
                aktivni.Add(r => Sadrzi(r.DijetetskiRestrikcije, _filteri.Intolerancije)
                               || Sadrzi(r.Namirnice, _filteri.Intolerancije));

            if (!string.IsNullOrWhiteSpace(_filteri.Vreme))
            {
                int maxMin = ParseMaxMinuta(_filteri.Vreme);
                if (maxMin > 0)
                    aktivni.Add(r => {
                        int min = ParseMinutaIzRecepta(r.VremePripreme);
                        return min == 0 || min <= maxMin;
                    });
            }

            if (!string.IsNullOrWhiteSpace(_filteri.Porcije))
            {
                var (pMin, pMax) = ParsePorcije(_filteri.Porcije);
                if (pMin > 0)
                    aktivni.Add(r => r.BrojPorcija == 0
                                  || (r.BrojPorcija >= pMin && (pMax == 0 || r.BrojPorcija <= pMax)));
            }

            // Ako nema aktivnih filtera — prikazi sve
            // Ako ima filtera — prikazi recept koji odgovara BAR JEDNOM filteru (OR logika)
            List<Recept> rezultati;
            if (aktivni.Count == 0)
                rezultati = svi;
            else
                rezultati = svi.Where(r => aktivni.Any(f => f(r))).ToList();

            rezultati = rezultati.OrderByDescending(r => r.Komentari.Count).ToList();
            ListaRecepata.ItemsSource = rezultati;
            TxtBrojRezultata.Text = $"Pronađeno: {rezultati.Count} recepata";
        }

        private static bool Sadrzi(string tekst, string termin) =>
            tekst?.Contains(termin, StringComparison.OrdinalIgnoreCase) == true;

        private static int ParseMaxMinuta(string vreme)
        {
            if (vreme.Contains("15")) return 15;
            if (vreme.Contains("30")) return 30;
            if (vreme.Contains("1 sat")) return 60;
            if (vreme.Contains("1-2")) return 120;
            if (vreme.Contains("Više")) return int.MaxValue;
            return 0;
        }

        private static int ParseMinutaIzRecepta(string vreme)
        {
            if (string.IsNullOrWhiteSpace(vreme)) return 0;
            int ukupno = 0;
            var satMatch = Regex.Match(vreme, @"(\d+)\s*(sat|sata|sati|h)", RegexOptions.IgnoreCase);
            if (satMatch.Success) ukupno += int.Parse(satMatch.Groups[1].Value) * 60;
            var minMatch = Regex.Match(vreme, @"(\d+)\s*(min|minut)", RegexOptions.IgnoreCase);
            if (minMatch.Success) ukupno += int.Parse(minMatch.Groups[1].Value);
            if (ukupno == 0 && Regex.IsMatch(vreme, @"^\d+$"))
                ukupno = int.Parse(vreme);
            return ukupno;
        }

        private static (int min, int max) ParsePorcije(string porcije)
        {
            var rangeMatch = Regex.Match(porcije, @"(\d+)-(\d+)");
            if (rangeMatch.Success)
                return (int.Parse(rangeMatch.Groups[1].Value), int.Parse(rangeMatch.Groups[2].Value));
            var plusMatch = Regex.Match(porcije, @"(\d+)\+");
            if (plusMatch.Success)
                return (int.Parse(plusMatch.Groups[1].Value), 0);
            var singleMatch = Regex.Match(porcije, @"^(\d+)");
            if (singleMatch.Success) { int n = int.Parse(singleMatch.Groups[1].Value); return (n, n); }
            return (0, 0);
        }

        private async void ImgKartica_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img
                && img.Tag is string path
                && !string.IsNullOrEmpty(path))
            {
                var bitmap = await Task.Run(() => SlikaHelper.UcitajSliku(path));
                if (bitmap != null)
                {
                    img.Source = bitmap;
                    // Sakrij placeholder emoji
                    if (img.Parent is System.Windows.Controls.Grid grid)
                        foreach (var child in grid.Children)
                            if (child is System.Windows.Controls.TextBlock tb)
                                tb.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void BtnPogledaj_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is Recept recept)
                NavigationHelper.NavigateTo(new DetaljiReceptaPage(recept));
        }

        private void BtnNazad_Click(object sender, RoutedEventArgs e) => NavigationHelper.GoBack();
        private void BtnPovratak_Click(object sender, RoutedEventArgs e) => NavigationHelper.NavigateTo(new GlavniEkranPage());

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.JePrijavljen)
                new ProfilWindow { Owner = Window.GetWindow(this) }.ShowDialog();
            else
                new LoginWindow { Owner = Window.GetWindow(this) }.ShowDialog();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e) => SessionManager.Odjavi();

        private void BtnPomoc_Click(object sender, RoutedEventArgs e)
        {
            new PomocWindow { Owner = System.Windows.Application.Current.MainWindow }.ShowDialog();
        }
    }
}