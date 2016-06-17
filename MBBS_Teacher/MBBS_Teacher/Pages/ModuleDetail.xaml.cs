using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public async void UtilizeState(object state)
        {
            data = (Data)state;
            Console.WriteLine(data.Lang);
            this.header.Content = data.ModuleName;
            this.SettingsList.Items.Add(new ListViewItem { Content = "Download module" });
            this.SettingsList.Items.Add(new ListViewItem { Content = "Check review" });
            try
            {
                List<Subsecties> sub = new List<Subsecties>();
                string text = null;
                Task t = Task.Run(() =>
                {
                    text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetSubsectionNames?languageID=" + data.Lang, this.data.Token);
                    sub = JsonConvert.DeserializeObject<List<Subsecties>>(text);

                });

                await Task.WhenAll(t);

                foreach (Subsecties subsec in sub)
                {
                    this.EditView.Items.Add(new ListViewItem { Content = subsec.SubsectionID + " " + subsec.SubsectionName });

                }
            }
            catch(WebException exept)
            {
                Console.WriteLine(exept.ToString());
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

        private async void moduleListDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popUp.IsOpen = true;
            var item = sender as ListViewItem;
            if (item != null)
            {
                if (item.ToString().Contains("Download module"))
                {
                    this.popUp.IsOpen = true;
                    try
                    {
                        Task t = Task.Run(async () =>
                        {
                            PdfWriter writer = new PdfWriter(data);
                            await writer.drawPdf();
                        });
                        await Task.WhenAll(t);
                    }
                    catch
                    {
                        Console.WriteLine("Error");
                    }
                    
                    this.popUp.IsOpen = false;
                }
                else
                {
                    data.ModuleChange = item.Content.ToString().Split(' ')[0];
                    Switcher.Switch(new LineChart(), data);
                }
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
