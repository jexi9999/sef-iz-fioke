using SefIzFioke.Dialogs;
using Microsoft.EntityFrameworkCore;
using SefIzFioke.Data;
using SefIzFioke.Helpers;
using SefIzFioke.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SefIzFioke.Views
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            UcitajKorisnike();
            UcitajRecepte();
        }

        private void UcitajKorisnike()
        {
            using var db = new AppDbContext();
            var korisnici = db.Korisnici
                .Include(k => k.Recepti)
                .Include(k => k.Komentari)
                .Include(k => k.Upozorenja)
                .Where(k => !k.IsAdmin)
                .OrderBy(k => k.KorisnickoIme)
                .ToList();

            ListaKorisnika.ItemsSource = korisnici;
            TxtBrojKorisnika.Text = $"{korisnici.Count}";
        }

        private void UcitajRecepte()
        {
            using var db = new AppDbContext();
            var recepti = db.Recepti
                .Include(r => r.Korisnik)
                .OrderBy(r => r.Naziv)
                .ToList();

            ListaRecepataAdmin.ItemsSource = recepti;
            TxtBrojRecepata.Text = $"{recepti.Count}";
        }

        // ---- Tab switching ----
        private void BtnTabKorisnici_Click(object sender, MouseButtonEventArgs e)
        {
            PanelKorisnici.Visibility = Visibility.Visible;
            PanelRecepti.Visibility = Visibility.Collapsed;
            BtnTabKorisnici.Background = Brushes.White;
            BtnTabRecepti.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#3D6B7F"));
        }

        private void BtnTabRecepti_Click(object sender, MouseButtonEventArgs e)
        {
            PanelKorisnici.Visibility = Visibility.Collapsed;
            PanelRecepti.Visibility = Visibility.Visible;
            BtnTabRecepti.Background = Brushes.White;
            BtnTabKorisnici.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#3D6B7F"));
        }

        // ---- Slike recepata ----
        private void ImgAdminRecept_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image img && img.DataContext is Recept r)
            {
                var bitmap = SlikaHelper.UcitajSliku(r.SlikaPath);
                if (bitmap != null) img.Source = bitmap;
            }
        }

        // ---- Admin akcije na receptima ----
        private void BtnUrediAdmin_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is Recept recept)
                NavigationHelper.NavigateTo(new UnosReceptaPage(recept));
        }

        private void BtnObrisiRecept_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not Recept recept) return;

            var potvrda = MessageBox.Show(
                $"Obrisati recept \"{recept.Naziv}\"?\nOva akcija će obrisati i sve komentare.",
                "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (potvrda != MessageBoxResult.Yes) return;

            using var db = new AppDbContext();
            var r = db.Recepti.Find(recept.Id);
            if (r != null) { db.Recepti.Remove(r); db.SaveChanges(); }
            UcitajRecepte();
        }

        // ---- Akcije na korisnicima ----
        private void BtnUpozori_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not Korisnik korisnik) return;

            var prozor = new Window
            {
                Title = $"Upozorenje za {korisnik.KorisnickoIme}",
                Width = 420,
                Height = 260,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this),
                ResizeMode = ResizeMode.NoResize,
                Background = Brushes.White
            };

            var panel = new StackPanel { Margin = new Thickness(24) };
            panel.Children.Add(new TextBlock
            {
                Text = $"Unesite poruku upozorenja za korisnika \"{korisnik.KorisnickoIme}\":",
                FontSize = 13,
                Margin = new Thickness(0, 0, 0, 10),
                TextWrapping = TextWrapping.Wrap
            });

            var txtPoruka = new TextBox
            {
                Height = 90,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                Margin = new Thickness(0, 0, 0, 16),
                Padding = new Thickness(8),
                FontSize = 13,
                BorderBrush = Brushes.LightGray
            };
            panel.Children.Add(txtPoruka);

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var btnOdustani = new Button { Content = "Odustani", Width = 100, Height = 34, Margin = new Thickness(0, 0, 10, 0) };
            btnOdustani.Click += (s, ev) => prozor.Close();

            var btnPosalji = new Button
            {
                Content = "Pošalji upozorenje",
                Height = 34,
                Padding = new Thickness(14, 0, 14, 0),
                Background = Brushes.OrangeRed,
                Foreground = Brushes.White
            };
            btnPosalji.Click += (s, ev) =>
            {
                string poruka = txtPoruka.Text.Trim();
                if (string.IsNullOrEmpty(poruka))
                {
                    MessageBox.Show("Unesite poruku.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using var db = new AppDbContext();
                db.Upozorenja.Add(new Upozorenje { Poruka = poruka, KorisnikId = korisnik.Id });
                db.SaveChanges();
                prozor.Close();
                MessageBox.Show($"Upozorenje je poslato korisniku \"{korisnik.KorisnickoIme}\".",
                    "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            };

            btnPanel.Children.Add(btnOdustani);
            btnPanel.Children.Add(btnPosalji);
            panel.Children.Add(btnPanel);
            prozor.Content = panel;
            prozor.ShowDialog();
        }

        private void BtnIzbaci_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not Korisnik korisnik) return;

            var potvrda = MessageBox.Show(
                $"Da li ste sigurni da želite da OBRIŠETE korisnika \"{korisnik.KorisnickoIme}\"?\n\nOva akcija će obrisati i sve njegove recepte i komentare.",
                "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (potvrda != MessageBoxResult.Yes) return;

            using var db = new AppDbContext();
            var k = db.Korisnici.Find(korisnik.Id);
            if (k != null)
            {
                db.Korisnici.Remove(k);
                db.SaveChanges();
                MessageBox.Show($"Korisnik \"{korisnik.KorisnickoIme}\" je obrisan.",
                    "Obrisano", MessageBoxButton.OK, MessageBoxImage.Information);
                UcitajKorisnike();
            }
        }

        private void BtnNazad_Click(object sender, RoutedEventArgs e) => NavigationHelper.GoBack();

        private void BtnPovratak_Click(object sender, RoutedEventArgs e)
            => NavigationHelper.NavigateTo(new GlavniEkranPage());

        private void BtnPomoc_Click(object sender, RoutedEventArgs e)
            => new PomocWindow { Owner = System.Windows.Application.Current.MainWindow }.ShowDialog();
    }
}