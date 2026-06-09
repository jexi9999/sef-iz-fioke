using System.Windows;
using System.Windows.Controls;

namespace SefIzFioke.Dialogs
{
    public partial class PomocWindow : Window
    {
        public PomocWindow()
        {
            InitializeComponent();
        }

        private void LstKategorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstKategorije.SelectedItem is not ListBoxItem item) return;

            // Sakrij sve panele
            PocetnaHelp.Visibility = Visibility.Collapsed;
            PretragaHelp.Visibility = Visibility.Collapsed;
            ReceptHelp.Visibility = Visibility.Collapsed;
            NalogHelp.Visibility = Visibility.Collapsed;
            AdminHelp.Visibility = Visibility.Collapsed;
            OstaloHelp.Visibility = Visibility.Collapsed;
            PocetnaPorukaPanel.Visibility = Visibility.Collapsed;

            string tag = item.Tag?.ToString() ?? "";

            switch (tag)
            {
                case "pocetna":
                    PocetnaHelp.Visibility = Visibility.Visible;
                    TxtNaslovKategorije.Text = "🏠  Početna strana";
                    break;
                case "pretraga":
                    PretragaHelp.Visibility = Visibility.Visible;
                    TxtNaslovKategorije.Text = "🔍  Pretraga recepata";
                    break;
                case "recept":
                    ReceptHelp.Visibility = Visibility.Visible;
                    TxtNaslovKategorije.Text = "📋  Dodavanje recepta";
                    break;
                case "nalog":
                    NalogHelp.Visibility = Visibility.Visible;
                    TxtNaslovKategorije.Text = "👤  Nalog i profil";
                    break;
                case "admin":
                    AdminHelp.Visibility = Visibility.Visible;
                    TxtNaslovKategorije.Text = "🛡  Admin panel";
                    break;
                case "ostalo":
                    OstaloHelp.Visibility = Visibility.Visible;
                    TxtNaslovKategorije.Text = "ℹ️  O aplikaciji";
                    break;
            }
        }

        private void BtnZatvori_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}