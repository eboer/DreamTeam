//===========================================================================================
//Project: MBBS
//Description:
//   Queries for survey related calls. Function names self explanatory.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using MBBS.Models;

namespace MBBS.Database
{
    public class SurveyQueries
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);

        public Dictionary<object, List<int>> CalculateRatingPerSubsection(SqlDataReader reader)
        {
            Dictionary<object, List<int>> surveyResults = new Dictionary<object, List<int>>();
            int currentYear = 0;
            string currentSubsection = null;
            List<int> ratings = new List<int>();

            while (reader.Read())
            {   
                int year = reader.GetDateTime(0).Year;
                if (year > currentYear)
                {
                    currentYear = year;
                }
                string subsectionName = reader.GetString(3);
                if(currentSubsection != reader.GetString(3) && currentSubsection != null)
                {
                    surveyResults.Add(currentSubsection, ratings);
                    ratings = new List<int>();
                    currentSubsection = reader.GetString(3);
                }  
                if(currentSubsection == null)
                {
                    currentSubsection = reader.GetString(3);
                }             
                if (year == currentYear)
                {
                    ratings.Add(reader.GetInt32(1));
                }
            }
            if (currentYear != 0)
            {
                surveyResults.Add(currentSubsection, ratings);
            }
            return surveyResults;
        }

        public Dictionary<object, List<int>> CalculateRatingPerYear(SqlDataReader reader)
        {
            Dictionary<object, List<int>> surveyResults = new Dictionary<object, List<int>>();
            int currentYear = 0;
            List<int> ratings = new List<int>();
            while (reader.Read())
            {
                int year = reader.GetDateTime(0).Year;
                if (year != currentYear)
                {
                    if (currentYear != 0)
                    {
                        surveyResults.Add(currentYear, ratings);
                    }
                    ratings = new List<int>();
                    currentYear = year;
                }
                ratings.Add(reader.GetInt32(1));
            }
            if (currentYear != 0)
            {
                surveyResults.Add(currentYear, ratings);
            }
            return surveyResults;
        }

        public Dictionary<object, List<int>> GetAllSurveyResults(string moduleID)
        {
            con.Open();
            Dictionary<object, List<int>> surveyResults = new Dictionary<object, List<int>>();
            SqlCommand cmd = new SqlCommand("SELECT CompletedSurvey.DateCompleted, Answer.Rating " +
                                            "FROM CompletedSurvey " +
                                            "INNER JOIN Answer " +
                                            "ON  CompletedSurvey.CompletedSurveyID = Answer.CompletedSurveyID " +
                                            "WHERE CompletedSurvey.ModuleID = @moduleID " +
                                            "ORDER BY DateCompleted DESC", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            SqlDataReader reader = cmd.ExecuteReader();
            surveyResults = CalculateRatingPerYear(reader);
            con.Close();
            return surveyResults;
        }

        public List<Answer> GetComments(string moduleID, string languageID)
        {
            con.Open();
            List<Answer> comments = new List<Answer>();
            Dictionary<object, List<int>> surveyResults = new Dictionary<object, List<int>>();
            SqlCommand cmd = new SqlCommand("SELECT " +
                "Answer.QuestionID, QuestionTranslation.QuestionText, Subsection.SubsectionID, " +
                "Subsection.SubsectionName, CompletedSurvey.DateCompleted, Answer.Rating, Answer.Comment " +
                "FROM CompletedSurvey " +
                "INNER JOIN Answer " +
                "ON  CompletedSurvey.CompletedSurveyID = Answer.CompletedSurveyID " +
                "INNER JOIN SurveyQuestion " +
                "ON Answer.QuestionID = SurveyQuestion.QuestionID " + 
                "INNER JOIN Subsection " +
                "ON SurveyQuestion.SubsectionID = Subsection.SubsectionID " +
                "INNER JOIN QuestionTranslation " +
                "ON SurveyQuestion.QuestionID = QuestionTranslation.QuestionID " +
                "WHERE CompletedSurvey.ModuleID = @moduleID " +
                "AND Answer.Comment IS NOT NULL " +
                "AND QuestionTranslation.LanguageID = @languageID " +
                "ORDER BY DateCompleted DESC", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            cmd.Parameters.AddWithValue("@languageID", languageID.Trim().ToUpper());
            
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Answer answer = new Answer();    
                answer.QuestionID = reader.GetDouble(0);
                answer.QuestionText = reader.GetString(1);
                answer.SubsectionID = reader.GetInt32(2);
                answer.SubsectionName = reader.GetString(3);
                answer.DateCompleted = reader.GetDateTime(4);
                answer.Rating = reader.GetInt32(5);
                answer.Comment = reader.GetString(6);
                comments.Add(answer);
            }

            con.Close();
            return comments;
        }

        public Dictionary<object, List<int>> GetCurrentSubsectionSurveyResults(string moduleID)
        {
            con.Open();
            Dictionary<object, List<int>> surveyResults = new Dictionary<object, List<int>>();
            SqlCommand cmd = new SqlCommand("SELECT CompletedSurvey.DateCompleted, Answer.Rating, SurveyQuestion.SubsectionID, Subsection.SubsectionName " +
            "FROM CompletedSurvey " +
            "INNER JOIN Answer " +
            "ON  CompletedSurvey.CompletedSurveyID = Answer.CompletedSurveyID " +
            "INNER JOIN SurveyQuestion " +
            "ON Answer.QuestionID = SurveyQuestion.QuestionID " +
            "INNER JOIN Subsection " +
            "ON SurveyQuestion.SubsectionID = Subsection.SubsectionID " +
            "WHERE CompletedSurvey.ModuleID = @moduleID " +
            "ORDER BY Subsection.SubsectionID, DateCompleted", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            SqlDataReader reader = cmd.ExecuteReader();
            surveyResults = CalculateRatingPerSubsection(reader);
            con.Close();
            return surveyResults;
        }

        public Dictionary<object, List<int>> GetSubsectionSurveyResults(string moduleID, int subsectionID)
        {
            con.Open();
            Dictionary<object, List<int>> surveyResults = new Dictionary<object, List<int>>();
            SqlCommand cmd = new SqlCommand("SELECT CompletedSurvey.DateCompleted, Answer.Rating, SurveyQuestion.SubsectionID " +
            "FROM CompletedSurvey " +
            "INNER JOIN Answer " +
            "ON  CompletedSurvey.CompletedSurveyID = Answer.CompletedSurveyID " +
            "INNER JOIN SurveyQuestion " +
            "ON Answer.QuestionID = SurveyQuestion.QuestionID " +
            "WHERE CompletedSurvey.ModuleID = @moduleID " +
            "AND SurveyQuestion.SubsectionID = @subsectionID " +
            "ORDER BY DateCompleted", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            cmd.Parameters.AddWithValue("@subsectionID", subsectionID);
            SqlDataReader reader = cmd.ExecuteReader();
            surveyResults = CalculateRatingPerYear(reader);
            con.Close();
            return surveyResults;
        }

        public List<Question> GetQuestions(string languageID)
        {
            List<Question> questions = new List<Question>();
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SubsectionID, QuestionTypeID, QuestionText, SurveyQuestion.QuestionID " + 
                "FROM SurveyQuestion " +
                "INNER JOIN QuestionTranslation " + 
                "ON SurveyQuestion.QuestionID = QuestionTranslation.QuestionID " +
                "WHERE LanguageID = @languageID", con);
            cmd.Parameters.AddWithValue("@languageID", languageID);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Question question = new Question();
                question.SubsectionID = reader.GetInt32(0);
                question.QuestionTypeID = reader.GetInt32(1);
                question.QuestionText = reader.GetString(2);
                question.QuestionID = reader.GetDouble(3);
                questions.Add(question);
            }
            con.Close();
            return questions;
        } 

        public void PostAnswers(CompletedSurvey completedSurvey)
        {
            HttpContext context = HttpContext.Current;
            int completedSurveyID = PostCompletedSurvey(completedSurvey.ModuleID, completedSurvey.StudentID);
            string valueString = "";
            foreach (Answer answer in completedSurvey.Answers)
            { 
                string addValue = string.Format("({0}, {1}, {2}, '{3}')", completedSurveyID, answer.QuestionID, answer.Rating, answer.Comment);
                if (!string.IsNullOrEmpty(valueString))
                {
                    valueString += ",";
                }
                valueString += addValue;
            }
            if(!string.IsNullOrEmpty(valueString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(string.Format("INSERT INTO Answer(CompletedSurveyID, QuestionID, Rating, Comment) " +
                    "VALUES {0}", valueString), con);
                SqlDataReader reader = cmd.ExecuteReader();
                con.Close();
            }
           
        }

        public int PostCompletedSurvey(string moduleID, int userID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO CompletedSurvey(ModuleID, StudentID, DateCompleted) " +
                            "OUTPUT inserted.CompletedSurveyID " +
                            "VALUES(@moduleID, @userID, @dateCompleted)", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@dateCompleted", DateTime.Now);
            SqlDataReader reader = cmd.ExecuteReader();


            int completedSurveyID = 0;
            while (reader.Read())
            {
               completedSurveyID = reader.GetInt32(0);
            }
            con.Close();
            return completedSurveyID;
        }
    }
}