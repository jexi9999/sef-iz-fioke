using SefIzFioke.Helpers;
using SefIzFioke.Views;
using System.Windows;

namespace SefIzFioke
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            NavigationHelper.MainFrame = MainFrame;

            // Pocetna stranica
            MainFrame.Navigate(new GlavniEkranPage());
        }
    }
}
