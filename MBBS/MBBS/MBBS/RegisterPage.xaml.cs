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
            MakeGetRequest(url);
        }

        public static async void MakeGetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();
        }
    }
}
