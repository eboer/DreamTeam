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
            string email = Email.Text;
            string fName = FirstName.Text;
            string lName = Lastname.Text;
            string pass = Password.Password;
            string error = null;
            string text = null;
            Task t = Task.Factory.StartNew(() => {
                try
                {
                    text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Account/Register?email=" + email + "&firstName=" + fName + "&lastName=" + lName + "&password=" + pass + "&userType=2");
                }
                catch (WebException exept)
                {
                    if (exept.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)exept.Response)
                        {
                            using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                            {
                                error = reader.ReadToEnd();
                                Console.WriteLine(error);
                            }
                        }

                        if (exept.ToString().Contains("500"))
                        {
                            error = "The server is unavailable, please try again later ";
                        }
                        else
                        {
                            error = "Username and/or password incorrect";
                        }
                    }
                }
            });
            Task.WaitAll(t);
            if(text != null)
            {
                Msg.Foreground = new SolidColorBrush(Colors.Green);
                Msg.Content = "Er is met succes een account aangemaakt";
            }
            else
            {
                Msg.Content = "Er is een probleem met het aanmaken van een account";
            } 

        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Login());
        }
    }
}
