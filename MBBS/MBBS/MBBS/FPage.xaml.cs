using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MBBS
{
    public partial class FPage : ContentPage
    {
        private string moduleName;
        private string moduleID;
        private string token;
        List<Question> questions = new List<Question>();

        public FPage(string token, string moduleID, string moduleName)
        {
            InitializeComponent();
            LoadModules();
            this.token = token;
            this.moduleName = moduleName;
            this.moduleID = moduleID;
            TitleLabel.Text = moduleName;
        }

        private async void LoadModules()
        {
            try
            {
                WebRequestHelper help = new WebRequestHelper();
                string data = await help.getData("http://mbbsweb.azurewebsites.net/api/Survey/GetSurveyQuestions?languageID=EN");
                questions = JsonConvert.DeserializeObject<List<Question>>(data);
                if (string.IsNullOrEmpty(data) || questions == null)
                {
                    MessageLabel.Text = "No data found.";
                }
                else
                {
                    ModuleListView.ItemsSource = questions;
                }
            }
            catch (Exception)
            {

                MessageLabel.Text = "No data found.";
            }

        }
    }


}
