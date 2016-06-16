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

        // Constructor
        public FormulierPage(string token, string moduleID, string moduleName)
        {
            InitializeComponent();
            LoadModules();
            this.token = token;
            this.moduleName = moduleName;
            this.moduleID = moduleID;
            this.Title = moduleName;
        }

        // Load the questions
        private async void LoadModules()
        {
            try
            {
                // Load the questions
                WebRequestHelper help = new WebRequestHelper();
                string data =
                    await help.getData("http://mbbsweb.azurewebsites.net/api/Survey/GetSurveyQuestions?languageID=EN");
                questions = JsonConvert.DeserializeObject<List<Question>>(data);
            }
            catch (Exception)
            {
                // Display no data found
                Debug.WriteLine("No data found");
            }
        }

        // Submit the survey data
        private void Button_OnClicked(object sender, EventArgs e)
        {
            List<SurveyData> surveyData = new List<SurveyData>();
            var questionIDs = new List<string> {};

            // Make an list of just the questionIDs
            foreach (var question in questions)
            {
                questionIDs.Add(question.QuestionID);
            }

            // Add question one input to survey data
            SurveyData tempData1 = new SurveyData();
            tempData1.QuestionID = questionIDs[0];
            tempData1.Rating = Star1Data.Text;
            tempData1.Comment = commentEntry1.Text;
            surveyData.Add(tempData1);

            // Add question two input to survey data
            SurveyData tempData2 = new SurveyData();
            tempData2.QuestionID = questionIDs[1];
            tempData2.Rating = Star2Data.Text;
            tempData2.Comment = commentEntry2.Text;
            surveyData.Add(tempData2);

            // Add question three input to survey data
            SurveyData tempData3 = new SurveyData();
            tempData3.QuestionID = questionIDs[2];
            tempData3.Rating = Star3Data.Text;
            tempData3.Comment = commentEntry3.Text;
            surveyData.Add(tempData3);

            // Add question four input to survey data
            SurveyData tempData4 = new SurveyData();
            tempData4.QuestionID = questionIDs[3];
            tempData4.Rating = Star4Data.Text;
            tempData4.Comment = commentEntry4.Text;
            surveyData.Add(tempData4);

            // Convert to json string and add moduleID
            JObject jObject = new JObject();
            jObject["ModuleID"] = JToken.FromObject(moduleID);
            jObject["Answers"] = JToken.FromObject(surveyData);
            string json = JsonConvert.SerializeObject(jObject);

            // Add json string to uri
            string url = "http://mbbsweb.azurewebsites.net/api/Survey/PostSurveyAnswers?content=" + json;

            // Test
            Debug.WriteLine(json);

            // Submit the json data
            MakeJsonRequest(url, json, token);

            // Display success alert
            DisplayAlert("Success!", "You have submitted the survey", "OK");

            // Pop current page and navigate to the last page
            Navigation.PopAsync();

        }

        public static async void MakeJsonRequest(string url, string json, string authorization)
        {
            // Create the webrequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers["Authorization"] = authorization;

            // Get the response
            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();

            // Read the webresponse
            StreamReader reader = new StreamReader(respStream);
            string text = reader.ReadToEnd();
        }
    }
}
