using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

        private void ButtonSubmit_OnClicked(object sender, EventArgs e)
        {
            List<SurveyData> surveyData = new List<SurveyData>();
            int test = 1;
            foreach (var question in questions)
            {
                SurveyData tempData = new SurveyData();
                tempData.QuestionID = question.QuestionID;
                tempData.Rating = 5;
                tempData.Comment = "test " + test;
                Debug.WriteLine(tempData.QuestionID);
                Debug.WriteLine(tempData.Rating);
                Debug.WriteLine(tempData.Comment);
                test++;

                surveyData.Add(tempData);
                
            }
            string json = JsonConvert.SerializeObject(surveyData);
            Debug.WriteLine(json);
            
            string url = "http://mbbsweb.azurewebsites.net/api/Survey/PostSurveyAnswers";

            MakeGetRequest(url);
            DisplayAlert("Success!", "You have submitted the survey", "OK");
            //Navigation.PopModalAsync();
        }

        public static async void MakeGetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();
        }

    }


}
