using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        public void PostAnswers(int userID, string test)
        {
            HttpContext context = HttpContext.Current;
          
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO ModuleContent(ModuleID, SubsectionID, LanguageID, AuthorID, Content, VersionNumber) " +
                "VALUES (@moduleID, @subsectionID, @languageID, @userID, @content, @version)", con);
            cmd.Parameters.AddWithValue("@moduleID", context.Request["moduleID"]);
            cmd.Parameters.AddWithValue("@subsectionID", context.Request["subsectionID"]);
            cmd.Parameters.AddWithValue("@languageID", context.Request["languageID"]);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@content", context.Request["content"]);
            SqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        }
    }
}