using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Windows.UI.Xaml.Controls;

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
            //LoadModules();
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

                }
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
            int test = 5;
            string rating = "1";
            foreach (var question in questions)
            {
                SurveyData tempData = new SurveyData();
                tempData.QuestionID = question.QuestionID;
                tempData.Rating = rating;

                //tempData.Comment = "abc";

                tempData.Comment = "test " + test;
                /*Debug.WriteLine(tempData.QuestionID);
                Debug.WriteLine(tempData.Rating);
                Debug.WriteLine(tempData.Comment);*/
                test++;

                surveyData.Add(tempData);
                
            }
            
            JObject jObject = new JObject();
            jObject["ModuleID"] = JToken.FromObject(moduleID);
            jObject["Answers"] = JToken.FromObject(surveyData);
            string json = JsonConvert.SerializeObject(jObject);
            
            Debug.WriteLine(json);

            string url = "http://mbbsweb.azurewebsites.net/api/Survey/PostSurveyAnswers?content=" + json;

            MakeJsonRequest(url, json, token);
            DisplayAlert("Success!", "You have submitted the survey", "OK");
            //Navigation.PopModalAsync();
        }

        public static async void MakeJsonRequest(string url, string json, string authorization)
        {
            //create the webrequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            
            request.Headers["Authorization"] = authorization;
            //get the respnse
            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();
            //read the webresponse
            StreamReader reader = new StreamReader(respStream);
            string text = reader.ReadToEnd();
        }
    }
}
