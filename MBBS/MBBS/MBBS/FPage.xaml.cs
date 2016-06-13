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
                /*Debug.WriteLine(tempData.QuestionID);
                Debug.WriteLine(tempData.Rating);
                Debug.WriteLine(tempData.Comment);*/
                test++;

                surveyData.Add(tempData);
                
            }
            
            JObject jObject = new JObject();
            jObject["ModuleID"] = JToken.FromObject(moduleID);
            jObject["Answers"] = JToken.FromObject(surveyData);
            String json = JsonConvert.SerializeObject(jObject);
            
            Debug.WriteLine(json);

            string url = "http://mbbsweb.azurewebsites.net/api/Survey/PostSurveyAnswers?content=" + json;

            MakeJsonRequest(url, json);
            DisplayAlert("Success!", "You have submitted the survey", "OK");
            //Navigation.PopModalAsync();
        }

        public static async void MakeJsonRequest(string url, string json)
        {
            /*var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            response = await client.PostAsync(url, content);
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"                 TodoItem succesfully saved.");
            }*/


            /*var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            //byte[] jsonArray = GetBytes(json);
            byte[] array = Encoding.UTF8.GetBytes(json);
            request.Content = new ByteArrayContent(array);
            //request.ContentType = "application/json";
            var response = await client.SendAsync(request);*/


            //WebClient wc = new WebClient();



            /*HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "POST";
            //request.ContentType = "application/json";
            //request.Length = data.Length;
            
            

            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();*/

            //create the webrequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //get the respnse
            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();
            //read the webresponse
            StreamReader reader = new StreamReader(respStream);
            string text = reader.ReadToEnd();





        }
        

    }


}
