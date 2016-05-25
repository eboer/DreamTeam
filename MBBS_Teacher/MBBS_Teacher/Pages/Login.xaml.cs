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


        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Username.Text) || string.IsNullOrEmpty(Password.Password))
            {
                Error.Content = "Please fill in the email and password";
            }
            else
            {
                string text = null;
                
                
                    try
                    {

                        string url = "http://mbbsweb.azurewebsites.net/api/Account/Login?email=" + Username.Text + "&password=" + Password.Password;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Headers.Add("username", Username.Text);
                        request.Headers.Add("password", Password.Password);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        Stream resStream = response.GetResponseStream(); ;
                        StreamReader reader = new StreamReader(resStream);
                        text = reader.ReadToEnd();
                        Data data = new Data();
                        data.token = text;
                        data.token = data.token.TrimEnd('"');
                        data.token = data.token.TrimStart('"');
                        data.LoginName = Username.Text;
                        Switcher.Switch(new Modulelist(), data);
                    }
                    catch (WebException exept)
                    {
                        if (exept.Response != null)
                        {
                            using (var errorResponse = (HttpWebResponse)exept.Response)
                            {
                                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                                {
                                    string error = reader.ReadToEnd();
                                    Console.WriteLine(error);
                                }
                            }

                            if (exept.ToString().Contains("500"))
                            {
                                Error.Content = "The server is unavailable, please try again later ";
                            }
                            else
                            {
                                Error.Content = "Username and/or password incorrect";
                            }
                            text = exept.ToString();
                        }

                    }

                
                
                Console.WriteLine(text);
            }
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

    }
}
