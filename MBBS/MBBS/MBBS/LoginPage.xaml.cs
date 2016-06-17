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
        // Constructor
        public LoginPage()
        {
            InitializeComponent();
        }
        
        // Go to the registerpage when the menu item signup is clicked
        private void MenuItem_OnButtonSignUpClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegisterPage());
        }

        // Login with provided input
        private async void ButtonLogin_OnClicked(object sender, EventArgs e)
        {
            WebRequestHelper help = new WebRequestHelper();
            // Check if input is null or empty
            if (string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
            {
                // Error message
                MessageLabel.Text = "Please fill in the email and password";
            }
            else
            {
                string uName = EmailEntry.Text;
                string pWord = PasswordEntry.Text;
                string url = null;
                string errorText = null;
                Data data = new Data();

                try
                {
                    // Try to login
                    url = await help.getData("http://mbbsweb.azurewebsites.net/api/Account/Login?email=" + uName + "&password=" + pWord);
                    Debug.WriteLine(url);
                }
                catch (WebException exept)
                {
                    // In case of exepctions
                    if (exept.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)exept.Response)
                        {
                            // Get the response
                            using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                            {
                                string error = reader.ReadToEnd();
                                Debug.WriteLine(error);
                            }
                        }

                        // Server unavailable error
                        if (exept.ToString().Contains("500"))
                        {
                            errorText = "The server is unavailable, please try again later ";
                        }

                        // Username/password error
                        else
                        {
                            errorText = "Username and/or password incorrect";
                        }
                    }
                }

                // If the url is not empty
                if (!string.IsNullOrEmpty(url))
                {
                    // Create the token
                    data.token = url;
                    data.token = data.token.TrimEnd('"');
                    data.token = data.token.TrimStart('"');
                    data.LoginName = EmailEntry.Text;
                    Debug.WriteLine((errorText));

                    // Change the mainpage in the module home page with token (logged in)
                    Application.Current.MainPage = new NavigationPage(new HomePage(data.token))
                    {
                        BarBackgroundColor = Color.FromHex("#003788")
                    };
                }
                else
                {
                    // Error message
                    MessageLabel.Text = errorText;
                }
            }
        }

        // Go to the registerpage when the register button is clicked
        private void ButtonRegister_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegisterPage());
        }
    }
}
