using Microsoft.EntityFrameworkCore;
using SefIzFioke.Data;
using SefIzFioke.Dialogs;
using SefIzFioke.Helpers;
using SefIzFioke.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SefIzFioke.Views
{
    public partial class DetaljiReceptaPage : Page
    {
        private readonly Recept _recept;
        private int _ocena = 0;

        public DetaljiReceptaPage(Recept recept)
        {
            InitializeComponent();
            _recept = recept;
            PrikaziRecept();
            UcitajKomentare();
        }

        private void PrikaziRecept()
        {
            TxtNaziv.Text = _recept.Naziv;
            TxtAutor.Text = $"Autor: {_recept.Korisnik?.KorisnickoIme ?? "Nepoznat"}";
            TxtKratakOpis.Text = _recept.KratakOpis;
            TxtRestrikcije.Text = _recept.DijetetskiRestrikcije;
            TxtSastojci.Text = _recept.Sastojci;
            TxtNapomene.Text = _recept.Napomene;
            TxtNacinPripreme.Text = _recept.NacinPripreme;
            TxtVremeTag.Text = $"⏱ {_recept.VremePripreme}";
            TxtPorcijeTag.Text = $"🍽 {_recept.BrojPorcija} porcija";

            // Prikaži dugme Uredi ako je korisnik autor ili admin
            var k = SessionManager.TrenutniKorisnik;
            if (k != null && (k.Id == _recept.KorisnikId || k.IsAdmin))
                BtnUredi.Visibility = Visibility.Visible;

            var slika = SlikaHelper.UcitajSliku(_recept.SlikaPath);
            if (slika != null)
            {
                ImgRecept.Source = slika;
                TxtNemaSliku.Visibility = Visibility.Collapsed;
            }
            else
            {
                TxtNemaSliku.Visibility = Visibility.Visible;
            }
        }

        private void UcitajKomentare()
        {
            using var db = new AppDbContext();
            var komentari = db.Komentari
                .Include(k => k.Korisnik)
                .Where(k => k.ReceptId == _recept.Id)
                .ToList();

            var vm = komentari.Select(k => new KomentarVM
            {
                Komentar = k,
                Avatar = SlikaHelper.UcitajSliku(k.Korisnik?.ProfilnaSlika)
            }).ToList();

            ListaKomentara.ItemsSource = vm;
            TxtBrojKomentara.Text = $"Komentari: ({komentari.Count})";
        }

        private void ImgAvatar_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img
                && img.DataContext is KomentarVM vm
                && vm.Avatar != null)
            {
                img.Source = vm.Avatar;
            }
        }

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (!int.TryParse(btn.Tag?.ToString(), out int ocena)) return;

            _ocena = ocena;

            var zvezde = new[] { Star1, Star2, Star3, Star4, Star5 };
            for (int i = 0; i < zvezde.Length; i++)
                zvezde[i].Content = i < ocena ? "⭐" : "☆";

            TxtOcenaLabel.Text = ocena switch
            {
                1 => "(1 — Loše)",
                2 => "(2 — Može bolje)",
                3 => "(3 — Solidno)",
                4 => "(4 — Dobro)",
                5 => "(5 — Odlično!)",
                _ => ""
            };
        }

        private void BtnPostavi_Click(object sender, RoutedEventArgs e)
        {
            if (!SessionManager.JePrijavljen)
            {
                MessageBox.Show("Morate biti prijavljeni da biste ostavili recenziju.",
                    "Potrebna prijava", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string tekst = TxtKomentar.Text.Trim();
            if (string.IsNullOrEmpty(tekst) && _ocena == 0)
            {
                MessageBox.Show("Unesite komentar ili izaberite ocenu.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new AppDbContext();
            db.Komentari.Add(new Komentar
            {
                Tekst = tekst,
                Ocena = _ocena,
                ReceptId = _recept.Id,
                KorisnikId = SessionManager.TrenutniKorisnik!.Id
            });
            db.SaveChanges();

            // Reset forme
            TxtKomentar.Text = "";
            _ocena = 0;
            foreach (var s in new[] { Star1, Star2, Star3, Star4, Star5 })
                s.Content = "☆";
            TxtOcenaLabel.Text = "(nije ocenjeno)";

            UcitajKomentare();
        }

        private void BtnPonistiKomentar_Click(object sender, RoutedEventArgs e)
        {
            TxtKomentar.Text = "";
            _ocena = 0;
            foreach (var s in new[] { Star1, Star2, Star3, Star4, Star5 })
                s.Content = "☆";
            TxtOcenaLabel.Text = "(nije ocenjeno)";
        }

        private void BtnGalerija_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Galerija slika će biti prikazana ovde.", "Galerija",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ← Nazad (prethodna stranica)
        private void BtnNazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }

        // Povratak na pocetni meni
        private void BtnPovratak_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(new GlavniEkranPage());
        }

        private void BtnUredi_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(new UnosReceptaPage(_recept));
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.JePrijavljen)
                new ProfilWindow { Owner = Window.GetWindow(this) }.ShowDialog();
            else
                new LoginWindow { Owner = Window.GetWindow(this) }.ShowDialog();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.Odjavi();
        }

        private void BtnPomoc_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new PomocWindow { Owner = System.Windows.Application.Current.MainWindow }.ShowDialog();
        }
    }
    public class KomentarVM
    {
        public Komentar Komentar { get; set; } = null!;
        public System.Windows.Media.Imaging.BitmapImage? Avatar { get; set; }
    }

}