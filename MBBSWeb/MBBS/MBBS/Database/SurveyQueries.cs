using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MBBS.Database
{
    public class SurveyQueries
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
        public Dictionary<int, List<int>> GetAllSurveyResults(string moduleID)
        {
            con.Open();
            Dictionary<int, List<int>> surveyResults = new Dictionary<int, List<int>>();
            SqlCommand cmd = new SqlCommand("SELECT CompletedSurvey.DateCompleted, Answer.Rating " +
                                            "FROM CompletedSurvey " +
                                            "INNER JOIN Answer " +
                                            "ON  CompletedSurvey.CompletedSurveyID = Answer.CompletedSurveyID " +
                                            "WHERE CompletedSurvey.ModuleID = @moduleID " +
                                            "ORDER BY DateCompleted", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID);
            SqlDataReader reader = cmd.ExecuteReader();
            int currentYear = 0;
            List<int> ratings = new List<int>();
            while (reader.Read())
            {

                int year = reader.GetDateTime(0).Year;
                if(year != currentYear)
                {
                    if(currentYear != 0)
                    {
                        surveyResults.Add(currentYear, ratings);
                    }
                    ratings = new List<int>();
                    currentYear = year;
                }
                ratings.Add(reader.GetInt32(1));
            }
            if(currentYear != 0)
            {
                surveyResults.Add(currentYear, ratings);
            }
            con.Close();
            return surveyResults;
        }
    }
}