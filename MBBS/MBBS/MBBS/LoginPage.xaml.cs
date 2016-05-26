using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Application.Current.MainPage = new NavigationPage(new HomePage())
            {
                BarBackgroundColor = Color.FromHex("#003788")
            };
        }

        private void ButtonRegister_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegisterPage());
        }
    }
}
