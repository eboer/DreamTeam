using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MBBS_Teacher.Pages
{
    /// <summary>
    /// Interaction logic for ModuleDetail.xaml
    /// </summary>
    public partial class ModuleDetail : UserControl, ISwitchable
    {
        Data data;
        Dictionary<string,string> moduleData = new Dictionary<string, string>();
        public ModuleDetail()
        {
            InitializeComponent();
            
        }

        public void UtilizeState(object state)
        {
            data = (Data)state;
            this.header.Content = data.ModuleName;
            this.SettingsList.Items.Add(new ListViewItem { Content = "Download module" });
            this.SettingsList.Items.Add(new ListViewItem { Content = "Check review" });

            List<Subsecties> sub = new List<Subsecties>();
            string text = null;
            Task t = Task.Factory.StartNew(() => {
                text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetSubsectionNames?languageID=en", this.data.token);
                sub = JsonConvert.DeserializeObject<List<Subsecties>>(text);

            });

            Task.WaitAll(t);

            foreach (Subsecties subsec in sub)
            {

                string moduleResond = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetData?moduleID=+" + data.ModuleName + "&subsectionID=" + subsec.SubsectionID + "&languageID=en", data.token);
                SubsectionData respondString = JsonConvert.DeserializeObject<SubsectionData>(moduleResond);
                moduleData.Add(subsec.SubsectionName, respondString.Content);
                                              //  WebRequestHelper.sendPostData("http://mbbsweb.azurewebsites.net/api/Module/PostData", data.token, data.ModuleName, subsec.SubsectionID, "en", "dit is cheap en dirty");
  
                this.EditView.Items.Add(new ListViewItem { Content = subsec.SubsectionID + " " + subsec.SubsectionName });

            }
            Dictionary<string,string> p = Module.getModuleData(data.token, data.ModuleName);
            foreach(KeyValuePair<string,string> k in p)
            {
                Console.WriteLine(k.Key + " " + k.Value);
            }
            Console.WriteLine("lol");
        }
        private void moduleList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                data.ModuleChange = item.Content.ToString();
                Switcher.Switch(new ModuleChange(), data);
            }
        }

        private void moduleListDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                data.ModuleChange = item.Content.ToString();
                Switcher.Switch(new LineChart(), data);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Modulelist(),data);
        }

        private class SubsectionData
        {
            [JsonProperty("AuthorID")]
            public string AuthorID { get; set; }
            [JsonProperty("Content")]
            public string Content { get; set; }
            [JsonProperty("VersionNumber")]
            public string VersionNumber { get; set; }

        }
    }
}
