using System.Windows.Controls;

namespace SefIzFioke.Helpers
{
    public static class NavigationHelper
    {
        public static Frame? MainFrame { get; set; }

        public static void NavigateTo(Page page)
        {
            MainFrame?.Navigate(page);
        }

        public static void GoBack()
        {
            if (MainFrame?.CanGoBack == true)
                MainFrame.GoBack();
        }
    }
}