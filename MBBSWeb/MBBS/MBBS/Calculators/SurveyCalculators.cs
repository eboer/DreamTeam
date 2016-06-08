using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBBS.Database;

namespace MBBS.Calculators
{
    public class SurveyCalculators
    {
        public Dictionary<object, double> CalculateRating(Dictionary <object, List<int>> resultList)
        {
            
            Dictionary<object, double> results = new Dictionary<object, double>();
            foreach(KeyValuePair<object, List<int>> year in resultList)
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
                if(yearTotal != 0)
                {
                    results.Add(year.Key, Math.Round(ratingResult, 1));
                }
                
            }
            return results;
        }

        public Dictionary<object, double> GetAverageRatingPerYear(string moduleID)
        {
            SurveyQueries query = new SurveyQueries();
            return CalculateRating(query.GetAllSurveyResults(moduleID));
        }

        public Dictionary<object, double> GetAverageRatingPerYearPerSubsection(string moduleID, int subsectionID)
        {
            SurveyQueries query = new SurveyQueries();
            return CalculateRating(query.GetSubsectionSurveyResults(moduleID, subsectionID));
        }

        public Dictionary<object, double> GetAverageRatingPerSubsection(string moduleID)
        {
            SurveyQueries query = new SurveyQueries();
            return CalculateRating(query.GetCurrentSubsectionSurveyResults(moduleID));
        }

    }
}