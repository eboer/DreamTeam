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
                this.EditView.Items.Add(new ListViewItem { Content = subsec.SubsectionID + " " + subsec.SubsectionName });

            }
            
        }
        private void moduleList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                data.ModuleChange = item.Content.ToString().Split(' ')[0]; 
                Switcher.Switch(new ModuleChange(), data);
            }
        }

        private void moduleListDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                data.ModuleChange = item.Content.ToString().Split(' ')[0];
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
