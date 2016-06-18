//===========================================================================================
//Project: MBBS
//Description:
//   Queries for translation related calls.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MBBS.Database
{
	public class TranslationQueries
	{
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
        
        public string GetTranslation(string englishWord, string languageCode)
        {
            if (!languageCode.Equals("EN") && englishWord != null)
            {
                string translatedWord = null;
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Translation " +
                    "FROM WordTranslation " +
                    "WHERE Word = @word AND LanguageCode = @language", con);
                cmd.Parameters.AddWithValue("@word", englishWord);
                cmd.Parameters.AddWithValue("@language", languageCode);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    translatedWord = reader.GetString(0);
                }
                con.Close();
                if (string.IsNullOrEmpty(translatedWord))
                {
                    return englishWord;
                }
                else
                {
                    return translatedWord;
                }
            }

            return englishWord;
        } 

        public List<string> TranslateArray(string[] array, string languageID)
        {
            List<string> translatedArray = new List<string>();
            foreach(string word in array)
            {
                translatedArray.Add(GetTranslation(word, languageID));
            }
            return translatedArray;
        }
	}
}