using SefIzFioke.Data;
using SefIzFioke.Dialogs;
using SefIzFioke.Helpers;
using SefIzFioke.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SefIzFioke.Views
{
    public partial class MojiReceptiPage : Page
    {
        public MojiReceptiPage()
        {
            InitializeComponent();
            UcitajRecepte();
        }

        private void UcitajRecepte()
        {
            if (!SessionManager.JePrijavljen)
            {
                NavigationHelper.NavigateTo(new GlavniEkranPage());
                return;
            }

            using var db = new AppDbContext();
            var recepti = db.Recepti
                .Where(r => r.KorisnikId == SessionManager.TrenutniKorisnik!.Id)
                .OrderBy(r => r.Naziv)
                .ToList();

            ListaRecepata.ItemsSource = recepti;
            TxtBrojRecepata.Text = $"{recepti.Count} recepata";

            if (recepti.Count == 0)
                PanelPrazan.Visibility = Visibility.Visible;
        }

        private void ImgKartica_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image img && img.DataContext is Recept r)
            {
                var bitmap = SlikaHelper.UcitajSliku(r.SlikaPath);
                if (bitmap != null)
                    img.Source = bitmap;
            }
        }

        private void BtnUredi_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is Recept recept)
                NavigationHelper.NavigateTo(new UnosReceptaPage(recept));
        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not Recept recept) return;

            var potvrda = MessageBox.Show(
                $"Da li ste sigurni da želite da obrišete recept \"{recept.Naziv}\"?",
                "Potvrda brisanja",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (potvrda != MessageBoxResult.Yes) return;

            using var db = new AppDbContext();
            var r = db.Recepti.Find(recept.Id);
            if (r != null)
            {
                db.Recepti.Remove(r);
                db.SaveChanges();
            }
            UcitajRecepte();
        }

        private void BtnDodajRecept_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(new UnosReceptaPage());
        }

        private void BtnNazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }

        private void BtnProfil_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.JePrijavljen)
                new ProfilWindow { Owner = Window.GetWindow(this) }.ShowDialog();
            else
                new LoginWindow { Owner = Window.GetWindow(this) }.ShowDialog();
        }

        private void BtnPomoc_Click(object sender, RoutedEventArgs e)
        {
            new PomocWindow { Owner = System.Windows.Application.Current.MainWindow }.ShowDialog();
        }
    }
}