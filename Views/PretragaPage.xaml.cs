using SefIzFioke.Dialogs;
using SefIzFioke.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace SefIzFioke.Views
{
    public partial class PretragaPage : Page
    {
        public PretragaPage()
        {
            InitializeComponent();
        }

        private string GetCombo(ComboBox cmb)
        {
            string val = (cmb.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
            // Ignorisi sve default vrednosti (Sve..., Bez filtera)
            if (val.StartsWith("Sve") || val == "Bez filtera") return "";
            return val;
        }

        private void BtnPretrazi_Click(object sender, RoutedEventArgs e)
        {
            var filteri = new FilteriPretrage
            {
                ImeRecepta = TxtImeRecepta.Text.Trim(),
                Dijeta = GetCombo(CmbDijeta),
                Alergije = GetCombo(CmbAlergije),
                Mleko = GetCombo(CmbMleko),
                Meso = GetCombo(CmbMeso),
                Namirnice = GetCombo(CmbNamirnice),
                Slatko = GetCombo(CmbSlatko),
                Izbegavati = GetCombo(CmbIzbegavati),
                Religija = GetCombo(CmbReligija),
                Intolerancije = GetCombo(CmbIntolerancije),
                Vreme = GetCombo(CmbVreme),
                Porcije = GetCombo(CmbPorcije),
            };
            NavigationHelper.NavigateTo(new RezultatiPage(filteri));
        }

        private void BtnPonisti_Click(object sender, RoutedEventArgs e)
        {
            TxtImeRecepta.Text = "";
            CmbMleko.SelectedIndex = 0;
            CmbMeso.SelectedIndex = 0;
            CmbNamirnice.SelectedIndex = 0;
            CmbSlatko.SelectedIndex = 0;
            CmbAlergije.SelectedIndex = 0;
            CmbDijeta.SelectedIndex = 0;
            CmbIzbegavati.SelectedIndex = 0;
            CmbReligija.SelectedIndex = 0;
            CmbIntolerancije.SelectedIndex = 0;
            CmbVreme.SelectedIndex = 0;
            CmbPorcije.SelectedIndex = 0;
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

    public class FilteriPretrage
    {
        public string ImeRecepta { get; set; } = "";
        public string Dijeta { get; set; } = "";
        public string Alergije { get; set; } = "";
        public string Mleko { get; set; } = "";
        public string Meso { get; set; } = "";
        public string Namirnice { get; set; } = "";
        public string Slatko { get; set; } = "";
        public string Izbegavati { get; set; } = "";
        public string Religija { get; set; } = "";
        public string Intolerancije { get; set; } = "";
        public string Vreme { get; set; } = "";
        public string Porcije { get; set; } = "";
    }
}