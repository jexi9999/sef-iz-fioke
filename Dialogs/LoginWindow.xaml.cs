using Microsoft.EntityFrameworkCore;
using SefIzFioke.Data;
using SefIzFioke.Helpers;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SefIzFioke.Dialogs
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == tb.Tag?.ToString())
            {
                tb.Text = "";
                tb.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = tb.Tag?.ToString();
                tb.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private string UzmiTekst(TextBox tb)
        {
            string tekst = tb.Text.Trim();
            return tekst == tb.Tag?.ToString() ? "" : tekst;
        }

        private void BtnLoguj_Click(object sender, RoutedEventArgs e)
        {
            string korisnickoIme = UzmiTekst(TxtKorisnickoIme);
            string lozinka = PbLozinka.Password;

            if (string.IsNullOrEmpty(korisnickoIme) || string.IsNullOrEmpty(lozinka))
            {
                PrikaziGresku("Popunite sva polja.");
                return;
            }

            using var db = new AppDbContext();
            string hashLozinke = PasswordHelper.Hash(lozinka);

            var korisnik = db.Korisnici
                .FirstOrDefault(k => k.KorisnickoIme == korisnickoIme
                                  && k.Lozinka == hashLozinke);

            if (korisnik == null)
            {
                PrikaziGresku("Pogrešno korisničko ime ili lozinka.");
                return;
            }

            SessionManager.TrenutniKorisnik = korisnik;
            MessageBox.Show($"Dobrodošli, {korisnik.KorisnickoIme}!", "Prijava uspešna",
                MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TxtZaboravio_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Funkcija resetovanja lozinke će biti implementirana.",
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TxtRegistracija_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
            var reg = new RegisterWindow { Owner = Owner };
            reg.ShowDialog();
        }

        private void PrikaziGresku(string poruka)
        {
            TxtGreska.Text = poruka;
            TxtGreska.Visibility = Visibility.Visible;
        }

        private bool _lozinkaVidljiva = false;

        private void BtnToggleLozinka_Click(object sender, RoutedEventArgs e)
        {
            _lozinkaVidljiva = !_lozinkaVidljiva;
            if (_lozinkaVidljiva)
            {
                TxtLozinkaVidljiva.Text = PbLozinka.Password;
                TxtLozinkaVidljiva.Visibility = System.Windows.Visibility.Visible;
                PbLozinka.Visibility = System.Windows.Visibility.Collapsed;
                BtnToggleLozinka.Content = "🙈";
            }
            else
            {
                PbLozinka.Password = TxtLozinkaVidljiva.Text;
                PbLozinka.Visibility = System.Windows.Visibility.Visible;
                TxtLozinkaVidljiva.Visibility = System.Windows.Visibility.Collapsed;
                BtnToggleLozinka.Content = "👁";
            }
        }
    }
}