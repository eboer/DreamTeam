using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MBBS_Teacher.Pages
{
    /// <summary>
    /// Interaction logic for ModuleChange.xaml
    /// </summary>
    public partial class ModuleChange : UserControl, ISwitchable
    {
        Data data;
        public ModuleChange()
        {
            InitializeComponent();
        }

        public async void UtilizeState(object state)
        {
            data = (Data)state;
            string moduleResond = null;
            Task moduleTask = Task.Run(() =>
            {
                try
                {
                    moduleResond = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetData?moduleID=+" + data.ModuleName + "&subsectionID=" + data.ModuleChange + "&languageID=en", data.token);
                }
                catch (WebException exept)
                {
                    Console.WriteLine(exept.ToString());
                }
            });
            await Task.WhenAll(moduleTask);
            Module.SubsectionData respondString = JsonConvert.DeserializeObject<Module.SubsectionData>(moduleResond);
            this.header.Content = data.ModuleName;
            content.AppendText(respondString.Content);
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ModuleDetail(), data);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            WebRequestHelper.sendPostData("http://mbbsweb.azurewebsites.net/api/Module/PostData", data.token, data.ModuleName, data.ModuleChange, "en", new TextRange(content.Document.ContentStart, content.Document.ContentEnd).Text);
            Switcher.Switch(new ModuleChange(), data);
        }

        private void saveAndExit_Click(object sender, RoutedEventArgs e)
        {
            WebRequestHelper.sendPostData("http://mbbsweb.azurewebsites.net/api/Module/PostData", data.token, data.ModuleName, data.ModuleChange, "en", new TextRange(content.Document.ContentStart, content.Document.ContentEnd).Text);
            Switcher.Switch(new ModuleDetail(), data);
        }
    }
}
