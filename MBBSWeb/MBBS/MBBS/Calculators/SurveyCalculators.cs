using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBBS.Database;

namespace MBBS.Calculators
{
    public class SurveyCalculators
    {
        public Dictionary<int, double> CalculateRating(Dictionary <int, List<int>> resultList)
        {
            
            Dictionary<int, double> results = new Dictionary<int, double>();
            foreach(KeyValuePair<int, List<int>> year in resultList)
            {
                int yearTotal = 0;
                int yearNumber = 0;
                foreach(int rating in year.Value)
                {
                    if(rating != 0)
                    {
                        yearTotal += rating;
                        yearNumber++;
                    }
                }
                double ratingResult = (double)yearTotal / (double)yearNumber;
                results.Add(year.Key, Math.Round(ratingResult, 1)); 
            }
            return results;
        }

        public Dictionary<int, double> CalculateSubsectionRating(Dictionary<int, List<int>> resultList)
        {
            string yearNumber;
            Dictionary<int, double> results = new Dictionary<int, double>();
            foreach (KeyValuePair<int, List<int>> result in resultList)
            {
                int subsectionTotal = 0;
                //int year = result.
               // if()
                foreach (int rating in result.Value)
                {

                    
                }
                //double ratingResult = (double)yearTotal / (double)yearNumber;
                //results.Add(result.Key, Math.Round(ratingResult, 1));
            }
            return results;
        }

        public Dictionary<int, double> GetAverageRatingPerYear(string moduleID)
        {
            SurveyQueries query = new SurveyQueries();
            return CalculateRating(query.GetAllSurveyResults(moduleID));
        }

        public Dictionary<int, double> GetAverageRatingPerYearPerSubsection(string moduleID, int subsectionID)
        {
            SurveyQueries query = new SurveyQueries();
            return CalculateRating(query.GetSubsectionSurveyResults(moduleID, subsectionID));
        }

        public Dictionary<int, double> GetAverageRatingPerSubsection(string moduleID)
        {
            SurveyQueries query = new SurveyQueries();
            //Dictionary<int, List<int>> results = query.GetCurrentSubsectionSurveyResults(moduleID);

            return CalculateRating(query.GetCurrentSubsectionSurveyResults(moduleID));
        }

    }
}