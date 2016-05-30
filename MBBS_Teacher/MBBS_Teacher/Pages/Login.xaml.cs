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
                string uName = Username.Text;
                string pWord = Password.Password;
                string text = null;
                string errorText = null;
                Data data = new Data();
                Task t = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Account/Login?email=" + uName + "&password=" + pWord);
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
                                errorText = "The server is unavailable, please try again later ";
                            }
                            else
                            {
                                errorText = "Username and/or password incorrect";
                            }
                        }

                    }
                });
                
                Task.WaitAll(t);
                if (text != null)
                {
                    data.token = text;
                    data.token = data.token.TrimEnd('"');
                    data.token = data.token.TrimStart('"');
                    data.LoginName = Username.Text;
                    Switcher.Switch(new PdfCreater(), data);
                }
                else
                {
                    Error.Content = errorText;
                }
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
