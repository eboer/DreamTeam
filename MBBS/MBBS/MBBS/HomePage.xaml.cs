using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MBBS
{
    public partial class HomePage : ContentPage
    {
        public HomePage(string token)
        {
            InitializeComponent();
            ModuleListView.ItemsSource = new List<Module>
            {
                new Module {module_name = "C#"},
                new Module {module_name = "Java Finals"},
                new Module {module_name = "Project website"}
            };
            Debug.WriteLine(token); //weghalen
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FormulierPage());
        }
    }
}
