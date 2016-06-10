using System.Windows.Controls;

namespace MBBS_Teacher
{
    static class Switcher
    {
        public static MainWindow mainWindow;
        public static void Switch(UserControl newPage)
        {
            mainWindow.Navigate(newPage);
        }
        public static void Switch(UserControl newPage, object state)
        {         
            mainWindow.Navigate(newPage, state);
        }
    }
}
