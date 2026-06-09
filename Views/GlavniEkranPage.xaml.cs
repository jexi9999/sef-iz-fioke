using Microsoft.EntityFrameworkCore;
using SefIzFioke.Data;
using SefIzFioke.Dialogs;
using SefIzFioke.Helpers;
using SefIzFioke.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SefIzFioke.Views
{
    public partial class GlavniEkranPage : Page
    {
        private List<Recept> _recepti = new();
        private int _trenutniIndex = 0;

        private static readonly Dictionary<string, (string emoji, string boja1, string boja2)> _teme = new()
        {
            { "sarma",      ("🥬", "#5C7A3E", "#3A5228") },
            { "lazanje",    ("🍝", "#8B4513", "#5C2E00") },
            { "sufle",      ("🍫", "#3D1C02", "#6B3A2A") },
            { "cokolad",    ("🍫", "#3D1C02", "#6B3A2A") },
            { "baget",      ("🥖", "#C8860A", "#8B5E0A") },
            { "palacink",   ("🥞", "#D4822A", "#A05A1E") },
            { "salata",     ("🥗", "#2E7D32", "#1B5E20") },
            { "supa",       ("🍲", "#8B6914", "#5C4A0A") },
            { "pica",       ("🍕", "#C0392B", "#922B21") },
            { "torta",      ("🎂", "#7B1FA2", "#4A0072") },
            { "piletina",   ("🍗", "#F57F17", "#E65100") },
            { "riba",       ("🐟", "#0277BD", "#01579B") },
        };

        private static (string emoji, string boja1, string boja2) UzmiTemu(string naziv)
        {
            string n = naziv.ToLower();
            foreach (var kv in _teme)
                if (n.Contains(kv.Key))
                    return kv.Value;
            return ("🍽", "#4A7A8F", "#2E5266");
        }

        public GlavniEkranPage()
        {
            InitializeComponent();
            UcitajRecepte();
            AzurirajAdminDugme();
        }

        private void AzurirajAdminDugme()
        {
            BtnAdmin.Visibility = (SessionManager.JePrijavljen && SessionManager.TrenutniKorisnik!.IsAdmin)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UcitajRecepte()
        {
            using var db = new AppDbContext();
            _recepti = db.Recepti
                .Include(r => r.Korisnik)
                .Include(r => r.Komentari)
                .OrderByDescending(r => r.Komentari.Count)
                .ToList();

            KreirajIndikatore();
            if (_recepti.Count > 0) PrikaziRecept(_trenutniIndex);
            else PrikaziPrazan();
        }

        private void KreirajIndikatore()
        {
            PanelIndikatora.Children.Clear();
            for (int i = 0; i < _recepti.Count; i++)
            {
                var el = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Margin = new Thickness(4, 0, 4, 0),
                    Fill = i == 0
                        ? Brushes.White
                        : new SolidColorBrush(Color.FromArgb(120, 255, 255, 255))
                };
                PanelIndikatora.Children.Add(el);
            }
        }

        private void AzurirajIndikatore()
        {
            for (int i = 0; i < PanelIndikatora.Children.Count; i++)
            {
                if (PanelIndikatora.Children[i] is Ellipse el)
                    el.Fill = i == _trenutniIndex
                        ? Brushes.White
                        : new SolidColorBrush(Color.FromArgb(120, 255, 255, 255));
            }
        }

        private void PrikaziRecept(int index)
        {
            var r = _recepti[index];
            TxtNaziv.Text = r.Naziv;
            TxtAutor.Text = $"Postavio: {r.Korisnik?.KorisnickoIme ?? "Nepoznat"}   ⭐ {r.Komentari.Count} recenzija";
            TxtOpis.Text = r.KratakOpis;
            TxtVremeTag.Text = $"⏱ {r.VremePripreme}";
            TxtPorcijeTag.Text = $"🍽 {r.BrojPorcija} porcija";

            // Prvo prikaži emoji/boju, pa učitaj sliku u pozadini
            var (emoji, boja1, boja2) = UzmiTemu(r.Naziv);
            TxtEmoji.Text = emoji;
            var grad = (LinearGradientBrush)GridEmoji.Background;
            grad.GradientStops[0].Color = (Color)ColorConverter.ConvertFromString(boja1);
            grad.GradientStops[1].Color = (Color)ColorConverter.ConvertFromString(boja2);
            ImgRecept.Source = null;
            ImgRecept.Visibility = Visibility.Collapsed;
            GridEmoji.Visibility = Visibility.Visible;

            AzurirajIndikatore();

            // Async učitavanje slike
            if (!string.IsNullOrEmpty(r.SlikaPath))
            {
                string putanja = r.SlikaPath;
                _ = UcitajSlikuAsync(putanja);
            }
        }

        private async Task UcitajSlikuAsync(string putanja)
        {
            var slika = await Task.Run(() => SlikaHelper.UcitajSliku(putanja));
            if (slika != null)
            {
                ImgRecept.Source = slika;
                ImgRecept.Visibility = Visibility.Visible;
                GridEmoji.Visibility = Visibility.Collapsed;
            }
        }

        private void PrikaziPrazan()
        {
            TxtNaziv.Text = "Nema recepata";
            TxtAutor.Text = "Budite prvi koji će dodati recept!";
            TxtOpis.Text = "";
            TxtVremeTag.Text = "";
            TxtPorcijeTag.Text = "";
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_recepti.Count == 0) return;
            _trenutniIndex = (_trenutniIndex - 1 + _recepti.Count) % _recepti.Count;
            PrikaziRecept(_trenutniIndex);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_recepti.Count == 0) return;
            _trenutniIndex = (_trenutniIndex + 1) % _recepti.Count;
            PrikaziRecept(_trenutniIndex);
        }

        private void KarticaRecept_Click(object sender, MouseButtonEventArgs e)
        {
            if (_recepti.Count == 0) return;
            NavigationHelper.NavigateTo(new DetaljiReceptaPage(_recepti[_trenutniIndex]));
        }

        private void BtnPretraga_Click(object sender, RoutedEventArgs e)
            => NavigationHelper.NavigateTo(new PretragaPage());

        private void BtnUnosRecepta_Click(object sender, RoutedEventArgs e)
        {
            if (!SessionManager.JePrijavljen)
            {
                MessageBox.Show("Morate biti prijavljeni da biste dodali recept.",
                    "Potrebna prijava", MessageBoxButton.OK, MessageBoxImage.Information);
                OtvoriLogin();
                return;
            }
            NavigationHelper.NavigateTo(new UnosReceptaPage());
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
            => NavigationHelper.NavigateTo(new AdminPage());

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.JePrijavljen)
                new ProfilWindow { Owner = Window.GetWindow(this) }.ShowDialog();
            else
                OtvoriLogin();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.JePrijavljen)
            {
                SessionManager.Odjavi();
                AzurirajAdminDugme();
                MessageBox.Show("Uspešno ste se odjavili.", "Odjava",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OtvoriLogin()
        {
            var dlg = new LoginWindow { Owner = Window.GetWindow(this) };
            dlg.ShowDialog();
            AzurirajAdminDugme();
        }

        private void BtnPomoc_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new PomocWindow { Owner = System.Windows.Application.Current.MainWindow }.ShowDialog();
        }
    }
}