using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MBBS
{
    public partial class HomePage : ContentPage
    {
        private string token;
        List<Module> modules = new List<Module>();

        public HomePage(string token)
        {
            this.token = token;
            LoadModules();
            InitializeComponent();

            foreach (Module m in modules)
            {
                MessageLabel.Text += m.module_name;
            }

            Debug.WriteLine(token); //weghalen
        }

        private async void LoadModules()
        {
            try
            {
                WebRequestHelper help = new WebRequestHelper();
                string data = await help.getData("http://mbbsweb.azurewebsites.net/api/Module/AllModules");
                modules = JsonConvert.DeserializeObject<List<Module>>(data);
                if (string.IsNullOrEmpty(data) || modules == null)
                {
                    MessageLabel.Text = "No data found.";
                }
                else
                {
                    ModuleListView.ItemsSource = modules;
                }
            }
            catch (Exception)
            {
                MessageLabel.Text = "Er is iets fout gegaan met het ophalen van de modules, probeer het later nog eens!";
            }
        }

        private async void ModuleListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var stack = Navigation.NavigationStack; // Create a stack
            if (stack[stack.Count - 1].GetType() != typeof (FormulierPage)) // Avoid opening multiple windows when spam clicking
            {
                if (e.Item != null)
                {
                    Module m = (Module) this.ModuleListView.SelectedItem;
                    await Navigation.PushAsync(new FormulierPage(token, m.module_id, m.module_name));
                }
            }


        } 

    }
}
