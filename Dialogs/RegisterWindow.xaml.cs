using Microsoft.Win32;
using SefIzFioke.Data;
using SefIzFioke.Helpers;
using SefIzFioke.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SefIzFioke.Dialogs
{
    public partial class RegisterWindow : Window
    {
        private string _putanjaSlike = "";

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnDodajFoto_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Slike (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Izaberite profilnu fotografiju"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new System.Uri(dlg.FileName);
                    bitmap.EndInit();
                    bitmap.Freeze();

                    _putanjaSlike = dlg.FileName;
                    ImgProfilna.Source = bitmap;
                    ImgProfilna.Visibility = Visibility.Visible;
                    TxtAvatarEmoji.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    MessageBox.Show("Nije moguće učitati izabranu sliku.", "Greška",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
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

        private void BtnRegistruj_Click(object sender, RoutedEventArgs e)
        {
            string korisnickoIme = UzmiTekst(TxtKorisnickoIme);
            string ime = UzmiTekst(TxtIme);
            string prezime = UzmiTekst(TxtPrezime);
            string email = UzmiTekst(TxtEmail);
            string lozinka = PbLozinka.Password;

            if (string.IsNullOrEmpty(korisnickoIme) || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(lozinka))
            {
                PrikaziGresku("Korisničko ime, email i lozinka su obavezni.");
                return;
            }

            if (lozinka.Length < 6)
            {
                PrikaziGresku("Lozinka mora imati najmanje 6 karaktera.");
                return;
            }

            using var db = new AppDbContext();

            if (db.Korisnici.Any(k => k.KorisnickoIme == korisnickoIme))
            {
                PrikaziGresku("Korisničko ime je zauzeto.");
                return;
            }

            if (db.Korisnici.Any(k => k.Email == email))
            {
                PrikaziGresku("Email adresa je već registrovana.");
                return;
            }

            var noviKorisnik = new Korisnik
            {
                KorisnickoIme = korisnickoIme,
                Ime = ime,
                Prezime = prezime,
                Email = email,
                Lozinka = PasswordHelper.Hash(lozinka),
                ProfilnaSlika = _putanjaSlike
            };

            db.Korisnici.Add(noviKorisnik);
            db.SaveChanges();

            SessionManager.TrenutniKorisnik = noviKorisnik;
            MessageBox.Show($"Dobrodošli, {korisnickoIme}! Registracija je uspešna.",
                "Registracija uspešna", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TxtLogin_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
            var login = new LoginWindow { Owner = Owner };
            login.ShowDialog();
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