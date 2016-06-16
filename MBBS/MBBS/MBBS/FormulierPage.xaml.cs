using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace MBBS
{
    public partial class FormulierPage : ContentPage
    {
        private string moduleName;
        private string moduleID;
        private string token;
        List<Question> questions = new List<Question>();
        public FormulierPage(string token, string moduleID, string moduleName)
        {
            InitializeComponent();
            LoadModules();
            this.token = token;
            this.moduleName = moduleName;
            this.moduleID = moduleID;
            this.Title = moduleName;
        }


        private async void LoadModules()
        {
            try
            {
                WebRequestHelper help = new WebRequestHelper();
                string data =
                    await help.getData("http://mbbsweb.azurewebsites.net/api/Survey/GetSurveyQuestions?languageID=EN");
                questions = JsonConvert.DeserializeObject<List<Question>>(data);

            }
            catch (Exception)
            {

                Debug.WriteLine("No data found");
            }

        }


        private void Button_OnClicked(object sender, EventArgs e)
        {
            List<SurveyData> surveyData = new List<SurveyData>();
            var questionIDs = new List<string> {};

            foreach (var question in questions)
            {
                questionIDs.Add(question.QuestionID);
            }

            SurveyData tempData1 = new SurveyData();
            tempData1.QuestionID = questionIDs[0];
            tempData1.Rating = Star1Data.Text;
            tempData1.Comment = commentEntry1.Text;
            surveyData.Add(tempData1);

            SurveyData tempData2 = new SurveyData();
            tempData2.QuestionID = questionIDs[1];
            tempData2.Rating = Star2Data.Text;
            tempData2.Comment = commentEntry2.Text;
            surveyData.Add(tempData2);

            SurveyData tempData3 = new SurveyData();
            tempData3.QuestionID = questionIDs[2];
            tempData3.Rating = Star3Data.Text;
            tempData3.Comment = commentEntry3.Text;
            surveyData.Add(tempData3);

            SurveyData tempData4 = new SurveyData();
            tempData4.QuestionID = questionIDs[3];
            tempData4.Rating = Star4Data.Text;
            tempData4.Comment = commentEntry4.Text;
            surveyData.Add(tempData4);


            JObject jObject = new JObject();
            jObject["ModuleID"] = JToken.FromObject(moduleID);
            jObject["Answers"] = JToken.FromObject(surveyData);
            string json = JsonConvert.SerializeObject(jObject);

            string url = "http://mbbsweb.azurewebsites.net/api/Survey/PostSurveyAnswers?content=" + json;

            Debug.WriteLine(json);

            //MakeJsonRequest(url, json, token);
            DisplayAlert("Success!", "You have submitted the survey", "OK");
            Navigation.PopModalAsync();

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
