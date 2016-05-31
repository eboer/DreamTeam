using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MBBS
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            //OI
            InitializeComponent();
            ModuleListView.ItemsSource = new List<Module>
            {
                new Module {module_name = "C#"},
                new Module {module_name = "Java Finals"},
                new Module {module_name = "Project website"}
            };
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FormulierPage());
        }
    }
}
