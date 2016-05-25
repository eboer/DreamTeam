using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
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
            Console.WriteLine("do this once");
            getData();
                
       }

       private void getData()
       {
            Console.WriteLine("Dolan pleaz stahp");
            try
            {
                string url = "http://mbbsweb.azurewebsites.net/api/Module/DocentModules";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Headers.Add("Authorization", this.data.token);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream resStream = response.GetResponseStream(); ;
                StreamReader reader = new StreamReader(resStream);
                text = reader.ReadToEnd();
               
                
                Console.WriteLine("verwacht hier iets:" + text);
                List<Module> t = JsonConvert.DeserializeObject<List<Module>>(text);
                foreach(Module module in t)
                {
                    moduleList.Items.Add(new ListViewItem { Content = module.module_name });
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
                data.ModuleName = item.Content.ToString();
                //Switcher.Switch(new ModuleDetail(), data);
            }
         }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Login());
        }
    }
}
