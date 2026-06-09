using SefIzFioke.Helpers;
using SefIzFioke.Views;
using System.Windows;

namespace SefIzFioke.Dialogs
{
    public partial class ProfilWindow : Window
    {
        public ProfilWindow()
        {
            InitializeComponent();
            UcitajProfil();
        }

        private void UcitajProfil()
        {
            var k = SessionManager.TrenutniKorisnik;
            if (k == null) { Close(); return; }

            TxtKorisnickoIme.Text = k.KorisnickoIme;

            string imePrezime = $"{k.Ime} {k.Prezime}".Trim();
            TxtImePrezime.Text = string.IsNullOrEmpty(imePrezime) ? "—" : imePrezime;
            TxtEmail.Text = k.Email;

            if (k.IsAdmin)
                BadgeAdmin.Visibility = Visibility.Visible;

            // Profilna slika
            if (!string.IsNullOrEmpty(k.ProfilnaSlika))
            {
                var bitmap = SlikaHelper.UcitajSliku(k.ProfilnaSlika);
                if (bitmap != null)
                {
                    ImgAvatar.Source = bitmap;
                    ImgAvatar.Visibility = Visibility.Visible;
                    TxtAvatarEmoji.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BtnZatvori_Click(object sender, RoutedEventArgs e) => Close();

        private void BtnMojiRecepti_Click(object sender, RoutedEventArgs e)
        {
            Close();
            NavigationHelper.NavigateTo(new MojiReceptiPage());
        }

        private void BtnOdjava_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.Odjavi();
            Close();
        }
    }
}