using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;

namespace MBBS
{
    public partial class RegisterPage : ContentPage
    {
        // Constructor
        public RegisterPage()
        {
            InitializeComponent();
        }

        // Register with provided input
        private void ButtonRegister_OnClicked(object sender, EventArgs e)
        {
            // Add input to the uri
            string url = "http://mbbsweb.azurewebsites.net/api/Account/Register?email=" + EmailEntry.Text + "&firstName=" + FirstNameEntry.Text + "&lastName=" + LastNameEntry.Text + "&password=" + PasswordEntry.Text;

            // Check if the input fields are not null or empty
            if (string.IsNullOrEmpty(FirstNameEntry.Text) || string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(LastNameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
            {
                // Error message
                MessageLabel.Text = "One or more fields are empty.";
            }
            else
            {
                // Check if both password fields have the same input
                if (PasswordEntry.Text == PasswordEntry2.Text)
                {
                    // Try to register
                    MakeGetRequest(url);
                    // Display success message
                    DisplayAlert("Success!", "You have registered an account", "OK");
                    // Pop current page and navigate to the last page
                    Navigation.PopModalAsync();
                }
                else
                {
                    // Error message
                    MessageLabel.Text = "The passwords do not match.";
                }
            }
        }

        // Send register input to the database
        public static async void MakeGetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();
        }
    }
}
