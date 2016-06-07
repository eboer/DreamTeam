using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;

namespace MBBS_Teacher.Pages
{
    /// <summary>
    /// Interaction logic for Modulelist.xaml
    /// </summary>
    public partial class Modulelist : UserControl, ISwitchable
    {
        Data data;
        private string text;
        
        public object Error { get; private set; }

        public Modulelist()
        {
            InitializeComponent();
           

        }

        public void UtilizeState(object state)
        {
            data = (Data)state;
            this.header.Content = "Hello " + data.LoginName + ", Select a module";
            this.footer.Content = data.LoginName + "'s modules";
            getData();
                
       }

        private async void getData()
        {

            try
            {
                List<Module> modules = new List<Module>();
                Task getModuleTask = Task.Run(() =>
                {
                    text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/DocentModules", this.data.token);
                    modules = JsonConvert.DeserializeObject<List<Module>>(text);
                });
                await Task.WhenAll(getModuleTask);
                foreach (Module module in modules)
                {
                    moduleList.Items.Add(new ListViewItem { Content = module.module_id + " " + module.module_name });
                }

            }
            catch (WebException exept)
            {
                if (exept.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)exept.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            Console.WriteLine(error);
                        }
                    }

                    if (exept.ToString().Contains("500"))
                    {
                        //   Error.Content = "The server is unavailable, please try again later ";
                    }
                    else
                    {
                        // Error.Content = "Username and/or password incorrect";
                    }
                    text = exept.ToString();
                    Console.WriteLine(text);
                }
            }

        }

        private void moduleList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
               
            var item = sender as ListViewItem;
            if (item != null)
            {
                data.ModuleName = item.Content.ToString().Substring(0,4);
                Switcher.Switch(new ModuleDetail(), data);
            }
         }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Login());
        }
    }
}
