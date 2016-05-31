using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MBBS_Teacher;
using Xamarin.Forms;

namespace MBBS
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void MenuItem_OnButtonSignUpClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegisterPage());
        }

        private async void ButtonLogin_OnClicked(object sender, EventArgs e)
        {
            WebRequestHelper help = new WebRequestHelper();
            if (string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
            {
                MessageLabel.Text = "Please fill in the email and password";
            }
            else
            {
                string uName = EmailEntry.Text;
                string pWord = PasswordEntry.Text;
                string text = null;
                string errorText = null;
                Data data = new Data();

                try
                {
                    text = await help.getData("http://mbbsweb.azurewebsites.net/api/Account/Login?email=" + uName + "&password=" + pWord);
                    Debug.WriteLine(text);
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
                                Debug.WriteLine(error);
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

                if (!string.IsNullOrEmpty(text))
                {
                    data.token = text;
                    data.token = data.token.TrimEnd('"');
                    data.token = data.token.TrimStart('"');
                    data.LoginName = EmailEntry.Text;
                    Debug.WriteLine((errorText));
                    Application.Current.MainPage = new NavigationPage(new HomePage(data.token))
                    {
                        BarBackgroundColor = Color.FromHex("#003788")
                    };

                }
                else
                {
                    MessageLabel.Text = errorText;
                }
            }


           
        }

        private void ButtonRegister_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegisterPage());
        }
    }
}
