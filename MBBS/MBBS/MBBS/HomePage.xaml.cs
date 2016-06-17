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

        // Constructor
        public HomePage(string token)
        {
            InitializeComponent();
            LoadModules();
            this.token = token;

            foreach (Module m in modules)
            {
                MessageLabel.Text += m.module_name;
            }
        }

        // Load the modules
        private async void LoadModules()
        {
            try
            {
                // Load the modules
                WebRequestHelper help = new WebRequestHelper();
                string data = await help.getData("http://mbbsweb.azurewebsites.net/api/Module/AllModules");
                modules = JsonConvert.DeserializeObject<List<Module>>(data);
                if (string.IsNullOrEmpty(data) || modules == null)
                {
                    // Error message
                    MessageLabel.Text = "No data found.";
                }
                else
                {
                    // Add source to modulelistview
                    ModuleListView.ItemsSource = modules;
                }
            }
            catch (Exception)
            {
                // Error message
                MessageLabel.Text = "Er is iets fout gegaan met het ophalen van de modules, probeer het later nog eens!";
            }
        }

        // On select module
        private async void ModuleListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Create stack for navigation
            var stack = Navigation.NavigationStack; 
            if (stack[stack.Count - 1].GetType() != typeof(FormulierPage))
            {
                if (e.Item != null)
                {
                    // Push selected module as new page
                    Module m = (Module)this.ModuleListView.SelectedItem;
                    await Navigation.PushAsync(new FormulierPage(token, m.module_id, m.module_name));
                }
            }
        } 

    }
}
