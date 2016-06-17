using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    if (EmailEntry.Text.Contains("stenden.com"))
                    {
                        string url = null;
                        try
                        {
                            // Add input to the uri
                            url = "http://mbbsweb.azurewebsites.net/api/Account/Register?email=" + EmailEntry.Text +
                                  "&firstName=" + FirstNameEntry.Text + "&lastName=" + LastNameEntry.Text + "&password=" +
                                  PasswordEntry.Text;

                            // Try to register
                            MakeGetRequest(url);
                        }
                        catch (WebException exept)
                        {
                            if (exept.Response != null)
                            {
                                using (var errorResponse = (HttpWebResponse) exept.Response)
                                {
                                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                                    {
                                        MessageLabel.Text = reader.ReadToEnd();
                                        Debug.WriteLine(MessageLabel.Text);
                                    }
                                }

                                if (exept.ToString().Contains("500"))
                                {
                                    MessageLabel.Text = "The server is unavailable";
                                }
                                else
                                {
                                    MessageLabel.Text = "Username and/or password incorrect";
                                }
                            }
                        }
                        if (url != null)
                        {
                            // Display success message
                            DisplayAlert("Success!", "You have registered an account", "OK");
                            // Pop current page and navigate to the last page
                            Navigation.PopModalAsync();
                        }
                        else
                        {
                            MessageLabel.Text = "Error creating account";
                        }
                    }
                    else
                    {
                        // Error message
                        MessageLabel.Text = "Use @Stenden.com";
                    }
                }
                else
                {
                    MessageLabel.Text = "The passwords do not match.";
                }
            }
        }

        // Send register input to the database
        public static async void MakeGetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();
        }
    }
}
