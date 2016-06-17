using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MBBS_Teacher.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl, ISwitchable
    {
        public Login()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox Username = (TextBox)sender;
            Username.Text = string.Empty;
            

        }

        private void footer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Switcher.Switch(new Register());
        }

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            popUp.IsOpen = true;
            if (string.IsNullOrEmpty(Username.Text) || string.IsNullOrEmpty(Password.Password))
            {
                Error.Content = "Please fill in the email and password";
            }
            else
            {
                string uName = Username.Text;
                string pWord = Password.Password;
                string text = null;
                string errorText = null;
                Data data = new Data();
                Task loginTask = Task.Run(() =>
                {
                    try
                    {
                        text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Account/Login?email=" + uName + "&password=" + pWord);
                    }
                    catch (WebException exept)
                    {
                        if (exept != null)
                        {
                            if (exept.ToString().Contains("500"))
                            {
                                errorText = "The server is unavailable, please try again later ";
                            }
                            else if (exept.ToString().Contains("De externe naam kan niet worden omgezet: 'mbbsweb.azurewebsites.net'")) 
                            {
                                errorText = "There is a problem with your internet connection";
                            }
                            else
                            {
                                errorText = "Username and/or password incorrect";
                            }
                        }

                    }
                });
                await Task.WhenAll(loginTask);
                Console.WriteLine(errorText);
                if (text != null)
                {
                    data.Token = text;
                    data.Token = data.Token.TrimEnd('"');
                    data.Token = data.Token.TrimStart('"');
                    data.LoginName = Username.Text;
                    Switcher.Switch(new Modulelist(), data);
                }
                else
                {
                    Error.Content = errorText;
                }
            }
        }
    }
}
