using SefIzFioke.Dialogs;
using Microsoft.Win32;
using SefIzFioke.Data;
using SefIzFioke.Helpers;
using SefIzFioke.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SefIzFioke.Views
{
    public partial class UnosReceptaPage : Page
    {
        private string _odabranaSlikaPath = "";
        private string _odabranVideoPath = "";
        private Recept? _recept = null; // null = novi recept, != null = edit mode

        public UnosReceptaPage()
        {
            InitializeComponent();
        }

        // Edit mode konstruktor
        public UnosReceptaPage(Recept recept)
        {
            InitializeComponent();
            _recept = recept;
            _odabranaSlikaPath = recept.SlikaPath;
            _odabranVideoPath = recept.VideoPath;

            // Popuni sva polja
            PopuniPolje(TxtNaziv, recept.Naziv);
            PopuniPolje(TxtKratakOpis, recept.KratakOpis);
            PopuniPolje(TxtNamirnice, recept.Namirnice);
            PopuniPolje(TxtSastojci, recept.Sastojci);
            PopuniPolje(TxtNacinPripreme, recept.NacinPripreme);
            PopuniPolje(TxtRestrikcije, recept.DijetetskiRestrikcije);
            PopuniPolje(TxtNapomene, recept.Napomene);
            PopuniPolje(TxtVremePripreme, recept.VremePripreme);
            PopuniPolje(TxtBrojPorcija, recept.BrojPorcija > 0 ? recept.BrojPorcija.ToString() : "");

            if (!string.IsNullOrEmpty(recept.VideoPath))
                TxtVideoPath.Text = System.IO.Path.GetFileName(recept.VideoPath);

            // Prikaži postojeću sliku ako postoji
            if (!string.IsNullOrEmpty(recept.SlikaPath) && System.IO.File.Exists(recept.SlikaPath))
            {
                var img = new Image
                {
                    Width = 60,
                    Height = 60,
                    Margin = new Thickness(3),
                    Stretch = System.Windows.Media.Stretch.UniformToFill
                };
                img.Source = new BitmapImage(new Uri(recept.SlikaPath));
                PanelSlike.Children.Add(new Border
                {
                    Width = 64,
                    Height = 64,
                    CornerRadius = new CornerRadius(6),
                    ClipToBounds = true,
                    Margin = new Thickness(2),
                    Child = img
                });
            }

            // Promeni natpis dugmeta i naslova
            BtnPostavi.Content = "Sačuvaj izmene";
        }

        private void PopuniPolje(TextBox tb, string vrednost)
        {
            if (!string.IsNullOrEmpty(vrednost))
            {
                tb.Text = vrednost;
                tb.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void BtnDodajSlike_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Slike|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = true,
                Title = "Izaberite slike"
            };

            if (dlg.ShowDialog() == true)
            {
                _odabranaSlikaPath = dlg.FileNames[0]; // glavna slika
                PanelSlike.Children.Clear();
                foreach (string path in dlg.FileNames)
                {
                    var img = new Image
                    {
                        Width = 60,
                        Height = 60,
                        Margin = new Thickness(3),
                        Stretch = System.Windows.Media.Stretch.UniformToFill
                    };
                    img.Source = new BitmapImage(new Uri(path));
                    var border = new Border
                    {
                        Width = 64,
                        Height = 64,
                        CornerRadius = new CornerRadius(6),
                        ClipToBounds = true,
                        Margin = new Thickness(2),
                        Child = img
                    };
                    PanelSlike.Children.Add(border);
                }
            }
        }

        private void BtnDodajVideo_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Video|*.mp4;*.avi;*.mov;*.mkv",
                Title = "Izaberite video"
            };
            if (dlg.ShowDialog() == true)
            {
                _odabranVideoPath = dlg.FileName;
                TxtVideoPath.Text = Path.GetFileName(dlg.FileName);
            }
        }

        private void BtnPostavi_Click(object sender, RoutedEventArgs e)
        {
            if (!SessionManager.JePrijavljen)
            {
                MessageBox.Show("Morate biti prijavljeni da biste dodali recept.",
                    "Potrebna prijava", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string naziv = UzmiTekst(TxtNaziv);
            if (string.IsNullOrEmpty(naziv))
            {
                MessageBox.Show("Unesite naziv recepta.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int.TryParse(UzmiTekst(TxtBrojPorcija), out int brojPorcija);

            using var db = new AppDbContext();

            if (_recept != null)
            {
                // EDIT MODE — pronađi i ažuriraj postojeći recept
                var postojeci = db.Recepti.Find(_recept.Id);
                if (postojeci == null)
                {
                    MessageBox.Show("Recept nije pronađen u bazi.", "Greška",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                postojeci.Naziv = naziv;
                postojeci.KratakOpis = UzmiTekst(TxtKratakOpis);
                postojeci.Namirnice = UzmiTekst(TxtNamirnice);
                postojeci.Sastojci = UzmiTekst(TxtSastojci);
                postojeci.NacinPripreme = UzmiTekst(TxtNacinPripreme);
                postojeci.DijetetskiRestrikcije = UzmiTekst(TxtRestrikcije);
                postojeci.Napomene = UzmiTekst(TxtNapomene);
                postojeci.BrojPorcija = brojPorcija;
                postojeci.VremePripreme = UzmiTekst(TxtVremePripreme);
                if (!string.IsNullOrEmpty(_odabranaSlikaPath))
                    postojeci.SlikaPath = _odabranaSlikaPath;
                if (!string.IsNullOrEmpty(_odabranVideoPath))
                    postojeci.VideoPath = _odabranVideoPath;

                db.SaveChanges();
                MessageBox.Show("Recept je uspešno izmenjen!", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // NOVI RECEPT
                db.Recepti.Add(new Recept
                {
                    Naziv = naziv,
                    KratakOpis = UzmiTekst(TxtKratakOpis),
                    Namirnice = UzmiTekst(TxtNamirnice),
                    Sastojci = UzmiTekst(TxtSastojci),
                    NacinPripreme = UzmiTekst(TxtNacinPripreme),
                    DijetetskiRestrikcije = UzmiTekst(TxtRestrikcije),
                    Napomene = UzmiTekst(TxtNapomene),
                    BrojPorcija = brojPorcija,
                    VremePripreme = UzmiTekst(TxtVremePripreme),
                    SlikaPath = _odabranaSlikaPath,
                    VideoPath = _odabranVideoPath,
                    KorisnikId = SessionManager.TrenutniKorisnik!.Id
                });
                db.SaveChanges();
                MessageBox.Show("Recept je uspešno dodat!", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            NavigationHelper.NavigateTo(new GlavniEkranPage());
        }

        private string UzmiTekst(TextBox tb)
        {
            string t = tb.Text.Trim();
            return t == tb.Tag?.ToString() ? "" : t;
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

        private void BtnNazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(new GlavniEkranPage());
        }

        private void BtnPovratak_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(new GlavniEkranPage());
        }

        private void BtnPomoc_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new PomocWindow { Owner = System.Windows.Application.Current.MainWindow }.ShowDialog();
        }
    }
}