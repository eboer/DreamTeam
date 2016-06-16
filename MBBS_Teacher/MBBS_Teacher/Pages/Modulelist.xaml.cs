using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using MBBS_Teacher.Pages;

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
            getSubData();
            getNotSubData();
                
       }

        private async void getNotSubData()
        {
            try
            {
                List<Module> modules = new List<Module>();
                Task getModuleTask = Task.Run(() =>
                {
                    text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/AllModules");
                    Console.WriteLine(text);
                    modules = JsonConvert.DeserializeObject<List<Module>>(text);
                });
                await Task.WhenAll(getModuleTask);
                allModuleList.ItemsSource = modules;

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

                    }
                    else
                    {

                    }
                    text = exept.ToString();
                    Console.WriteLine(text);
                }
            }
        }

        private async void getSubData()
        {

            try
            {
                List<Module> modules = new List<Module>();
                Task getModuleTask = Task.Run(() =>
                {
                    text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/DocentModules", this.data.Token);
                    modules = JsonConvert.DeserializeObject<List<Module>>(text);
                });
                await Task.WhenAll(getModuleTask);
                List<Module> tempModuleList = new List<Module>();
                for(int i = 0; i < modules.Count; i++)
                {
                    Module englishMod = new Module();
                    englishMod.module_id = modules[i].module_id;
                    englishMod.module_name = modules[i].module_name;
                    englishMod.module_lang = "en";
                    tempModuleList.Add(englishMod);

                }
                
                modules.AddRange(tempModuleList);
                moduleList.ItemsSource = modules;
               
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
                       
                    }
                    else
                    {
                       
                    }
                    text = exept.ToString();
                    Console.WriteLine(text);
                }
            }

        }

        private void moduleList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
               
            var item = sender as ListViewItem;
            Module module = (Module)item.Content;
            if(string.IsNullOrEmpty(module.module_lang))
            {
                data.Lang = "nl";
            }
            else
            {
                data.Lang = module.module_lang;
            }
            if (item != null)
            {
                data.ModuleName = module.module_id;
                Switcher.Switch(new ModuleDetail(), data);
            }
         }

        private void allModuleList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var item = sender as ListViewItem;
            Module module = (Module)item.Content;
            
            if (item != null)
            {
                try
                {
                    WebRequestHelper.sendPostSignup("http://mbbsweb.azurewebsites.net/api/Module/AddDocentModule", module.module_id, data.Token);
                    getSubData();
                }
                catch
                {
                    //error
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Login());
        }
    }
}
