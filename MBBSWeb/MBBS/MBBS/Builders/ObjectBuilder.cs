//===========================================================================================
//Project: MBBS
//Description: 
//   Converts received JSON to Object(s)
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using MBBS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MBBS.Builders
{
    public class ObjectBuilder
    {
        public List<Answer> BuildAnswerList(string answerJSON)
        {
            List<Answer> answers = new List<Answer>();
            JsonConvert.DeserializeObject<Answer>(answerJSON);

            return answers;
        }

        public CompletedSurvey BuildCompletedSurvey(string answersJson, int studentID)
        {
            CompletedSurvey completedSurvey = new CompletedSurvey();

            JObject jObject = JObject.Parse(answersJson);
            JToken jModule = jObject["ModuleID"];
            completedSurvey.ModuleID = (string)jObject["ModuleID"];
            completedSurvey.DateCompleted = DateTime.Now;
            completedSurvey.StudentID = studentID;

            JToken[] jTokenAnswers = jObject["Answers"].ToArray();
            List<Answer> answers = new List<Answer>();
            foreach(JToken jAnswer in jTokenAnswers)
            {
                Answer answer = new Answer();
                answer.QuestionID = Convert.ToDouble(jAnswer["QuestionID"]);
                answer.Rating = Convert.ToInt32(jAnswer["Rating"]);
                answer.Comment = Convert.ToString(jAnswer["Comment"]);
                answers.Add(answer);
            }
            completedSurvey.Answers = answers;

            return completedSurvey;
        }

    }
}