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
            InitializeComponent();
            ModuleListView.ItemsSource = new List<Module>
            {
                new Module {Name = "C#"},
                new Module {Name = "Java Finals"},
                new Module {Name = "Project website"}
            };
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FormulierPage());
        }
    }
}
