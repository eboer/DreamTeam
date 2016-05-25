using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MBBS_Teacher.Pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl, ISwitchable
    {
        public Register()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Email_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox Email = (TextBox)sender;
            Email.Text = string.Empty;
        }

        private void FirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox FirstName = (TextBox)sender;
            FirstName.Text = string.Empty;
        }

        private void Lastname_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox LastName = (TextBox)sender;
            LastName.Text = string.Empty;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://mbbsweb.azurewebsites.net/api/Account/Register?email=" + Email.Text + "&firstName=" + FirstName.Text + "&lastName=" + Lastname.Text + "&password=" + Password.Password + "&userType=2";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Login());
        }
    }
}
