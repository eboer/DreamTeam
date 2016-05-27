using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MBBS
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        

        private void ButtonRegister_OnClicked(object sender, EventArgs e)
        {
            string url = "http://mbbsweb.azurewebsites.net/api/Account/Register?email=" + EmailEntry.Text + "&firstName=" + FirstNameEntry.Text + "&lastName=" + LastNameEntry.Text + "&password=" + PasswordEntry.Text;
            if (PasswordEntry.Text == PasswordEntry2.Text)
            {
                MakeGetRequest(url);
                DisplayAlert("Success!", "You have registered an account", "OK");
                Navigation.PopModalAsync();
            }
            else
            {
                MessageLabel.Text = "The passwords do not match.";
            }
        }

        public static async void MakeGetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();
        }
    }
}
